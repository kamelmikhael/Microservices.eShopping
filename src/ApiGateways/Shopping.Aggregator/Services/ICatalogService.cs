using Shopping.Aggregator.Models;

namespace Shopping.Aggregator.Services;

public interface ICatalogService
{
    Task<IEnumerable<CatalogModel>> GetCatalogsAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<CatalogModel>> GetCatalogsByCategoryAsync(string category, CancellationToken cancellationToken = default);
    Task<CatalogModel> GetCatalogAsync(string id, CancellationToken cancellationToken = default);
}
