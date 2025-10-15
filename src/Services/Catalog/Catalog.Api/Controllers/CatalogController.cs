using Catalog.Api.Features.Products;
using Catalog.Api.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Api.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class CatalogController(
    ILogger<CatalogController> logger
    , IPorductRepository repository) : ControllerBase
{
    private readonly ILogger<CatalogController> _logger = logger;
    private readonly IPorductRepository _repository = repository;

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Product>))]
    public async Task<IActionResult> GetProducts()
        => Ok(await _repository.GetProducts());

    [HttpGet("{id:length(24)}", Name = nameof(GetProductById))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Product))]
    public async Task<IActionResult> GetProductById(string id)
    {
        var product = await _repository.GetProduct(id);

        return product is null ? NotFound() : Ok(product);
    }

    [HttpGet("[action]/{category}", Name = nameof(GetProductByCategory))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Product>))]
    public async Task<IActionResult> GetProductByCategory(string category)
        => Ok(await _repository.GetProductByCategory(category));

    [HttpGet("[action]/{name}", Name = nameof(GetProductByName))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<Product>))]
    public async Task<IActionResult> GetProductByName(string name)
        => Ok(await _repository.GetProductByName(name));

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Product))]
    public async Task<IActionResult> CreateProduct([FromBody] Product product)
    {
        await _repository.CreateProduct(product);

        return CreatedAtRoute(nameof(GetProductById), new { id = product.Id}, product);
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateProduct([FromBody] Product product)
    {
        await _repository.UpdateProduct(product);

        return Ok();
    }

    [HttpDelete("{id:length(24)}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteProduct(string id)
    {
        await _repository.DeleteProduct(id);

        return Ok();
    }
}
