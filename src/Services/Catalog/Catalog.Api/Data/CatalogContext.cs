using Catalog.Api.Features.Products;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Catalog.Api.Data;

public class CatalogContext : ICatalogContext
{
    private readonly IMongoDatabase _database;
    private readonly DatabaseSettings _databaseSettings;

    public CatalogContext(IOptions<DatabaseSettings> settings)
    {
        _databaseSettings = settings.Value;

        var client = new MongoClient(_databaseSettings.ConnectionString);

        _database = client.GetDatabase(_databaseSettings.DatabaseName);
    }

    public IMongoCollection<Product> Products => 
        _database.GetCollection<Product>(_databaseSettings.CollectionName);
}
