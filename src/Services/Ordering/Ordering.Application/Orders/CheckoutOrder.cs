using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Contracts.Persistence;
using Ordering.Domain.Orders;

namespace Ordering.Application.Orders;

public class CheckoutOrder
{
    public sealed record Request
    {
        public string UserName { get; set; } = default!;
        public decimal TotalPrice { get; set; }

        //Billing Address
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Address { get; set; } = default!;
        public string Country { get; set; } = default!;
        public string State { get; set; } = default!;
        public string ZipCode { get; set; } = default!;

        //Payment
        public string CardName { get; set; } = default!;
        public string CardNumber { get; set; } = default!;
        public string Expiration { get; set; } = default!;
        public string Cvv { get; set; } = default!;
        public int PaymentMethod { get; set; }
    }

    public sealed record Response(int Id);

    public sealed record Command(Request Input) : IRequest<Response>;

    internal sealed class Handler(
        IWriteRepository<Order> orderRepository
        , IEmailService emailService
        , ILogger<Handler> logger) : IRequestHandler<Command, Response>
    {
        private readonly IWriteRepository<Order> _orderRepository = orderRepository;
        private readonly IEmailService _emailService = emailService;
        private readonly ILogger<Handler> _logger = logger;

        public async Task<Response> Handle(Command command, CancellationToken cancellationToken)
        {
            Order newOrder = MapToOrder(command.Input);

            _orderRepository.Add(newOrder);

            await _orderRepository.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Order '{OrderId}' is successfully created.", newOrder.Id);

            await SendEmail(newOrder);

            return new Response(newOrder.Id);
        }

        private static Order MapToOrder(Request request)
        {
            return new Order
            {
                UserName = request.UserName,
                TotalPrice = request.TotalPrice,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Address = request.Address,
                Country = request.Country,
                State = request.State,
                ZipCode = request.ZipCode,
                CardName = request.CardName,
                CardNumber = request.CardNumber,
                Expiration = request.Expiration,
                Cvv = request.Cvv,
                PaymentMethod = request.PaymentMethod,
            };
        }

        private async Task SendEmail(Order order)
        {
            try
            {
                await _emailService.SendAsync(new()
                {
                    To = order.Email,
                    Subject = "Order Created",
                    Body = $"Your order with Id = '{order.Id}' has been created."
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    "Order '{OrderId}' failed due to an error with the email service: {Message}",
                    order.Id,
                    ex.Message);
            }
        }
    }

    internal sealed class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(c => c.Input.UserName).NotEmpty().NotNull();
            RuleFor(c => c.Input.Email).NotEmpty().NotNull().EmailAddress();
            RuleFor(c => c.Input.TotalPrice).NotEmpty().GreaterThan(0);
        }
    }
}
