using Shopping.Aggregator.Extensions;
using Shopping.Aggregator.Models;

namespace Shopping.Aggregator.Services;

public class BasketService(HttpClient httpClient) : IBasketService
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<BasketModel> GetBasket(string userName, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync($"{BasketServiceConstants.Default}/{userName}", cancellationToken);

        return await response.ReadContentAs<BasketModel>();
    }

    private static class BasketServiceConstants
    {
        public const string Default = "/api/v1/Basket";
    }
}
