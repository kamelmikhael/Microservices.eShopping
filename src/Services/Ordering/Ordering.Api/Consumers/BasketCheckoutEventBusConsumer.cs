using EventBus.Messages.Events.Basket;
using MassTransit;
using MediatR;
using Ordering.Application.Orders;

namespace Ordering.Api.Consumers;

public class BasketCheckoutEventBusConsumer(
    ILogger<BasketCheckoutEventBusConsumer> logger
    , IMediator mediator) : IConsumer<BasketCheckoutIntegrationEvent>
{
    private readonly ILogger<BasketCheckoutEventBusConsumer> _logger = logger;
    private readonly IMediator _mediator = mediator;

    public async Task Consume(ConsumeContext<BasketCheckoutIntegrationEvent> context)
    {
        var request = new CheckoutOrder.Request
        {
            UserName = context.Message.UserName,
            TotalPrice = context.Message.TotalPrice,
            FirstName = context.Message.FirstName,
            LastName = context.Message.LastName,
            Email = context.Message.Email,
            Address = context.Message.Address,
            Country = context.Message.Country,
            State = context.Message.State,
            ZipCode = context.Message.ZipCode,
            CardName = context.Message.CardName,
            CardNumber = context.Message.CardNumber,
            Expiration = context.Message.Expiration,
            Cvv = context.Message.Cvv,
            PaymentMethod = context.Message.PaymentMethod,
        };

        var response = await _mediator.Send(new CheckoutOrder.Command(request));

        _logger.LogInformation(
            "BasketCheckoutEventBusConsumer consumed event {EventId} successfully. Created Order Id: '{OrderId}'. At {Time} UTC."
            , context.Message.Id
            , response.Id
            , DateTime.UtcNow.ToString());
    }
}
