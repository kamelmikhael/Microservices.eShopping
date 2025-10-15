using Catalog.Api.Data;
using Catalog.Api.Features.Products;
using MongoDB.Driver;

namespace Catalog.Api.Repositories;

public class PorductRepository : IPorductRepository
{
    private readonly ICatalogContext _context;

    public PorductRepository(ICatalogContext context) =>
        _context = context ?? throw new ArgumentNullException(nameof(context));

    public async Task<IEnumerable<Product>> GetProducts()
    {
        return await _context
                        .Products
                        .Find(p => true)
                        .ToListAsync();
    }

    public Task<Product> GetProduct(string id)
    {
        return _context
                    .Products
                    .Find(p => p.Id == id)
                    .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Product>> GetProductByName(string productName)
    {
        FilterDefinition<Product> filter = Builders<Product>
            .Filter
            .Eq(p => p.Name, productName);

        return await _context
                        .Products
                        .Find(filter)
                        .ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetProductByCategory(string categoryName)
    {
        FilterDefinition<Product> filter = Builders<Product>
            .Filter
            .Eq(p => p.Catagory, categoryName);

        return await _context
                        .Products
                        .Find(filter)
                        .ToListAsync();
    }

    public Task CreateProduct(Product product)
    {
        return _context
                    .Products
                    .InsertOneAsync(product);
    }

    public async Task<bool> UpdateProduct(Product product)
    {
        var updateResult = await _context
            .Products
            .ReplaceOneAsync(filter: p => p.Id == product.Id, replacement: product);

        return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
    }

    public async Task<bool> DeleteProduct(string id)
    {
        FilterDefinition<Product> filter = Builders<Product>
            .Filter
            .Eq(p => p.Id, id);

        DeleteResult deleteResult = await _context
            .Products
            .DeleteOneAsync(filter);

        return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
    }
}
