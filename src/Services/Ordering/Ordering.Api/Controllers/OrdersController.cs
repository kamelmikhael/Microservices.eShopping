using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ordering.Application.Orders;

namespace Ordering.Api.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class OrdersController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet("{userName}", Name = nameof(GetOrdersByUserName))]
    [ProducesResponseType(typeof(IEnumerable<GetOrderList.Response>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetOrdersByUserName(string userName)
        => Ok(await _mediator.Send(new GetOrderList.Query(userName)));

    [HttpPost]
    [ProducesResponseType(typeof(IEnumerable<CheckoutOrder.Response>), StatusCodes.Status200OK)]
    public async Task<IActionResult> CheckoutOrder([FromBody] CheckoutOrder.Request request)
        => Ok(await _mediator.Send(new CheckoutOrder.Command(request)));

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> UpdateOrder([FromBody] UpdateOrder.Request request)
    {
        await _mediator.Send(new UpdateOrder.Command(request));
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> DeleteOrder(int id)
    {
        await _mediator.Send(new DeleteOrder.Command(id));
        return NoContent();
    }
}
