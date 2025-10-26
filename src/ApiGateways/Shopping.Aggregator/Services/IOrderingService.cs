using Shopping.Aggregator.Models;

namespace Shopping.Aggregator.Services;

public interface IOrderingService
{
    Task<IEnumerable<OrderModel>> GetOrdersByUserName(string userName, CancellationToken cancellationToken = default);
}
