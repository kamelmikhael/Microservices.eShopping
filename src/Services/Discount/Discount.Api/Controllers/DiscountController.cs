using Discount.Api.Entities;
using Discount.Api.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Discount.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DiscountController(IDiscountRepository repository) : ControllerBase
    {
        private readonly IDiscountRepository _repository = repository;

        [HttpGet("{productName}", Name = nameof(GetDiscount))]
        [ProducesResponseType(typeof(Coupon), StatusCodes.Status200OK)]
        public async Task<ActionResult<Coupon>> GetDiscount(string productName)
        {
            return Ok(await _repository.GetDiscount(productName));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> CreateDiscount([FromBody] Coupon coupon)
        {
            await _repository.CreateDiscount(coupon);
            return CreatedAtRoute(
                nameof(GetDiscount),
                new { productName = coupon.ProductName },
                coupon);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> UpdateDiscount([FromBody] Coupon coupon)
        {
            return Ok(await _repository.UpdateDiscount(coupon));
        }

        [HttpDelete("{productName}", Name = nameof(DeleteDiscount))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteDiscount(string productName)
        {
            return Ok(await _repository.DeleteDiscount(productName));
        }
    }
}
