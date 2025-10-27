using Basket.Api.Entities;
using Basket.Api.GrpcServices;
using Basket.Api.Repositories;
using EventBus.Messages.Events.Basket;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace Basket.Api.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class BasketController(IBaskRepository baskRepository
    , DiscountGrpcService discountGrpcService
    , IPublishEndpoint publishEndpoint
    , ILogger<BasketController> logger) : ControllerBase
{
    private readonly IBaskRepository _baskRepository = baskRepository;
    private readonly DiscountGrpcService _discountGrpcService = discountGrpcService;
    private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;
    private readonly ILogger<BasketController> _logger = logger;

    [HttpGet("{userName}", Name = nameof(GetBasket))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ShoppingCart))]
    public async Task<ActionResult<ShoppingCart>> GetBasket(string userName)
    {
        var basket = await _baskRepository.GetBasket(userName);

        basket ??= new ShoppingCart(userName);

        return Ok(basket);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ShoppingCart))]
    public async Task<ActionResult<ShoppingCart>> UpdateBasket([FromBody] ShoppingCart basket)
    {
        //Communicate with Discount.Grpc
        //to calculate latest prices of product into shopping cart

        foreach (var item in basket.Items)
        {
            try
            {
                var coupon = await _discountGrpcService.GetDiscount(item.ProductName);
                item.Price -= coupon.Amount;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error to get {Product} coupon", item.ProductName);
            }
        }

        return Ok(await _baskRepository.UpdateBasket(basket));
    }

    [HttpDelete("{userName}", Name = nameof(DeleteBasket))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteBasket(string userName)
    {
        await _baskRepository.DeleteBasket(userName);
        return Ok();
    }

    [HttpPost("[action]")]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Checkout([FromBody] BasketCheckout basketCheckout)
    {
        // get existing basket with total price
        // create basketCheckoutEvent -- set TotalPrice on basketCheckout eventMessage
        // send checkout event to rabbitmq
        // remove the basket
        _logger.LogInformation("Basket checkout initiated for user: {UserName}"
            , basketCheckout.UserName);

        var basket = await _baskRepository.GetBasket(basketCheckout.UserName);
        if (basket == null)
        {
            _logger.LogError("Error: NotFount basket for user: {UserName}", basketCheckout.UserName);
            return BadRequest();
        }

        var checkoutEvent = new BasketCheckoutIntegrationEvent
        {
            UserName = basketCheckout.UserName,
            TotalPrice = basket.TotalPrice,
            FirstName = basketCheckout.FirstName,
            LastName = basketCheckout.LastName,
            Email = basketCheckout.Email,
            Address = basketCheckout.Address,
            Country = basketCheckout.Country,
            State = basketCheckout.State,
            ZipCode = basketCheckout.ZipCode,
            CardName = basketCheckout.CardName,
            CardNumber = basketCheckout.CardNumber,
            Expiration = basketCheckout.Expiration,
            Cvv = basketCheckout.Cvv,
            PaymentMethod = basketCheckout.PaymentMethod,
        };

        _logger.LogInformation("Found basket for user: {UserName} and delet and trying to publish message broker", basketCheckout.UserName);

        try
        {
            await _publishEndpoint.Publish(checkoutEvent);

            await _baskRepository.DeleteBasket(basket.UserName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception happen: {Msg}", ex.Message);
        }

        _logger.LogInformation("Publish and delete success");

        return Accepted();
    }
}
