using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shopping.Aggregator.Models;
using Shopping.Aggregator.Services;

namespace Shopping.Aggregator.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class ShoppingController(ILogger<ShoppingController> logger
    , ICatalogService catalogService
    , IBasketService basketService
    , IOrderingService orderingService) : ControllerBase
{
    private readonly ILogger<ShoppingController> _logger = logger;
    private readonly ICatalogService _catalogService = catalogService;
    private readonly IBasketService _basketService = basketService;
    private readonly IOrderingService _orderingService = orderingService;

    [HttpGet("{userName}", Name = nameof(GetShopping))]
    public async Task<ActionResult<ShoppingModel>> GetShopping(string userName
        , CancellationToken cancellationToken)
    {
        var basket = await _basketService.GetBasket(userName, cancellationToken);
        var orders = await _orderingService.GetOrdersByUserName(userName, cancellationToken);

        foreach(var item in basket.Items)
        {
            try
            {
                var product = await _catalogService.GetCatalogAsync(item.ProductId, cancellationToken);
                item.Catagory = product.Catagory;
                item.Summary = product.Summary;
                item.Description = product.Description;
                item.ImageFile = product.ImageFile;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex
                    , "Error happen when trying get Product information from Catalog API. At {Time} UTC"
                    , DateTime.UtcNow);
            }
        }

        ShoppingModel model = new()
        {
            UserName = userName,
            Orders = orders,
            BasketWithProdects = basket,
        };

        return Ok(model);
    }
}
