using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Exceptions;
using Ordering.Domain.Orders;

namespace Ordering.Application.Orders;

public class UpdateOrder
{
    public sealed record Request
    {
        public int Id { get; set; }
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

    public sealed record Command(Request Request) : IRequest;

    internal sealed class Handler(
        IWriteRepository<Order> orderRepository
        , ILogger<Handler> logger) : IRequestHandler<Command>
    {
        private readonly IWriteRepository<Order> _orderRepository = orderRepository;
        private readonly ILogger<Handler> _logger = logger;

        public async Task<Unit> Handle(Command command, CancellationToken cancellationToken)
        {
            var orderToUpdate = await _orderRepository.GetByIdAsync(command.Request.Id) 
                ?? throw new NotFoundException(nameof(Order), command.Request.Id);

            UpdateOrder(command.Request, orderToUpdate);

            await _orderRepository.UpdateAsync(orderToUpdate);

            await _orderRepository.SaveChangesAsync(cancellationToken);

            _logger.LogInformation($"Order {command.Request.Id} was updated successfly");

            return Unit.Value;
        }

        private static void UpdateOrder(Request request, Order orderToUpdate)
        {
            orderToUpdate.UserName = request.UserName;
            orderToUpdate.TotalPrice = request.TotalPrice;
            orderToUpdate.FirstName = request.FirstName;
            orderToUpdate.LastName = request.LastName;
            orderToUpdate.Email = request.Email;
            orderToUpdate.Address = request.Address;
            orderToUpdate.Country = request.Country;
            orderToUpdate.State = request.State;
            orderToUpdate.ZipCode = request.ZipCode;
            orderToUpdate.CardName = request.CardName;
            orderToUpdate.CardNumber = request.CardNumber;
            orderToUpdate.Expiration = request.Expiration;
            orderToUpdate.Cvv = request.Cvv;
            orderToUpdate.PaymentMethod = request.PaymentMethod;
        }
    }

    internal sealed class Validator : AbstractValidator<Command>
    {
        public Validator()
        {
            RuleFor(c => c.Request.UserName).NotEmpty().NotNull();
            RuleFor(c => c.Request.Email).NotEmpty().NotNull().EmailAddress();
            RuleFor(c => c.Request.TotalPrice).NotEmpty().GreaterThan(0);
        }
    }
}
