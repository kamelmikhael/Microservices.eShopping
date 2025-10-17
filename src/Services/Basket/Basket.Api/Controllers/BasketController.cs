using Basket.Api.Entities;
using Basket.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Basket.Api.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class BasketController(IBaskRepository baskRepository) : ControllerBase
{
    private readonly IBaskRepository _baskRepository = baskRepository;

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
        return Ok(await _baskRepository.UpdateBasket(basket));
    }

    [HttpDelete("{userName}", Name = nameof(DeleteBasket))]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteBasket(string userName)
    {
        await _baskRepository.DeleteBasket(userName);
        return Ok();
    }
}
