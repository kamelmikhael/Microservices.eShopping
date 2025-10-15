using Catalog.Api.Features.Products;
using MongoDB.Driver;

namespace Catalog.Api.Data;

public interface ICatalogContext
{
    IMongoCollection<Product> Products { get; }
}
