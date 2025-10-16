using Basket.Api.Entities;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Basket.Api.Repositories;

public class BaskRepository(IDistributedCache cache) : IBaskRepository
{
    private readonly IDistributedCache _cache = cache;

    public async Task<ShoppingCart?> GetBasket(string userName)
    {
        var basket = await _cache.GetStringAsync(userName);

        return string.IsNullOrEmpty(basket) 
            ? null 
            : JsonSerializer.Deserialize<ShoppingCart>(basket);
    }

    public async Task<ShoppingCart?> UpdateBasket(ShoppingCart basket)
    {
        await _cache.SetStringAsync(basket.UserName, JsonSerializer.Serialize(basket));
        return await GetBasket(basket.UserName);
    }

    public async Task DeleteBasket(string userName)
    {
        await _cache.RemoveAsync(userName);
    }
}