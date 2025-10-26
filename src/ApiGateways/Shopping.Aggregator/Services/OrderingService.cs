using Shopping.Aggregator.Extensions;
using Shopping.Aggregator.Models;

namespace Shopping.Aggregator.Services;

public class OrderingService(HttpClient httpClient) : IOrderingService
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<IEnumerable<OrderModel>> GetOrdersByUserName(string userName, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync($"{OrderingServiceConstants.Default}/{userName}", cancellationToken);

        return await response.ReadContentAs<List<OrderModel>>();
    }

    private static class OrderingServiceConstants
    {
        public const string Default = "/api/v1/Orders";
    }
}
