using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Contracts.Persistence;
using Ordering.Domain.Orders;

namespace Ordering.Application.Orders;

public class CkeckoutOrder
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

    internal sealed class Handler(IWriteRepository<Order> orderRepository
        , IEmailService emailService
        , ILogger<Handler> logger) : IRequestHandler<Command, Response>
    {
        private readonly IWriteRepository<Order> _orderRepository = orderRepository;
        private readonly IEmailService _emailService = emailService;
        private readonly ILogger<Handler> _logger = logger;

        public async Task<Response> Handle(Command request, CancellationToken cancellationToken)
        {
            var order = new Order
            {
                UserName = request.Input.UserName,
                TotalPrice = request.Input.TotalPrice,
                FirstName = request.Input.FirstName,
                LastName = request.Input.LastName,
                Email = request.Input.Email,
                Address = request.Input.Address,
                Country = request.Input.Country,
                State = request.Input.State,
                ZipCode = request.Input.ZipCode,
                CardName = request.Input.CardName,
                CardNumber = request.Input.CardNumber,
                Expiration = request.Input.Expiration,
                Cvv = request.Input.Cvv,
                PaymentMethod = request.Input.PaymentMethod,
            };

            await _orderRepository.AddAsync(order);

            await _orderRepository.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Order '{OrderId}' is successfully created.", order.Id);

            await SendEmail(order);

            return new Response(order.Id);
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

    internal sealed class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(c => c.Input.UserName).NotEmpty().NotNull();
            RuleFor(c => c.Input.Email).NotEmpty().NotNull().EmailAddress();
            RuleFor(c => c.Input.TotalPrice).NotEmpty().GreaterThan(0);
        }
    }
}
