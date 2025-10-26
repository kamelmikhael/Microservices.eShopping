using Shopping.Aggregator.Extensions;
using Shopping.Aggregator.Models;

namespace Shopping.Aggregator.Services;

public class CatalogService(HttpClient httpClient) : ICatalogService
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<CatalogModel> GetCatalogAsync(string id, CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync($"{CatalogServiceConstants.Default}/{id}", cancellationToken);

        return await response.ReadContentAs<CatalogModel>();
    }

    public async Task<IEnumerable<CatalogModel>> GetCatalogsAsync(CancellationToken cancellationToken = default)
    {
        var response = await _httpClient.GetAsync(CatalogServiceConstants.Default, cancellationToken);

        return await response.ReadContentAs<List<CatalogModel>>();
    }

    public async Task<IEnumerable<CatalogModel>> GetCatalogsByCategoryAsync(string category, CancellationToken cancellationToken = default)
    {

        var response = await _httpClient.GetAsync($"{CatalogServiceConstants.GetProductByCategory}/{category}", cancellationToken);

        return await response.ReadContentAs<List<CatalogModel>>();
    }

    private static class CatalogServiceConstants
    {
        public const string Default = "/api/v1/Catalog";
        public const string GetProductByCategory = $"{Default}/GetProductByCategory";
    }
}