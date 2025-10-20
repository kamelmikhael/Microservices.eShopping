using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Exceptions;
using Ordering.Domain.Orders;

namespace Ordering.Application.Orders;

public class DeleteOrder
{
    public record Command(int Id) : IRequest;

    internal sealed class Handler(
        IWriteRepository<Order> orderRepository
        , ILogger<Handler> logger) : IRequestHandler<Command>
    {
        private readonly IWriteRepository<Order> _orderRepository = orderRepository;
        private readonly ILogger<Handler> _logger = logger;

        public async Task<Unit> Handle(Command command, CancellationToken cancellationToken)
        {
            var orderToDelete = await _orderRepository.GetByIdAsync(command.Id) 
                ?? throw new NotFoundException(nameof(Order), command.Id);

            _orderRepository.Delete(orderToDelete);

            await _orderRepository.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Order {Id} was deleted successfly", orderToDelete.Id);

            return Unit.Value;
        }
    }
}
