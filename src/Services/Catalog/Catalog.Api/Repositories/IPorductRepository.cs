using Catalog.Api.Features.Products;

namespace Catalog.Api.Repositories;

public interface IPorductRepository
{
    Task<IEnumerable<Product>> GetProducts();
    Task<Product> GetProduct(string id);
    Task<IEnumerable<Product>> GetProductByName(string productName);
    Task<IEnumerable<Product>> GetProductByCategory(string categoryName);
    Task CreateProduct(Product product);
    Task<bool> UpdateProduct(Product product);
    Task<bool> DeleteProduct(string id);
}
