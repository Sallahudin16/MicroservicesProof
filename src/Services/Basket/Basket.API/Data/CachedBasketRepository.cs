using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Basket.API.Data
{
    public class CachedBasketRepository : IBasketRepository
    {
        private readonly IBasketRepository _repository;
        private readonly IDistributedCache _cache;
        public CachedBasketRepository(IBasketRepository repository, IDistributedCache cache)
        {
            _repository = repository;
            _cache = cache;
        }

        public async Task<ShoppingCart> GetBasket(string userName, CancellationToken cancellationToken = default)
        {
            string? cachedBasket = await _cache.GetStringAsync(userName, cancellationToken);
            if(!string.IsNullOrWhiteSpace(cachedBasket))
            {
                return JsonSerializer.Deserialize<ShoppingCart>(cachedBasket) 
                    ?? throw new BasketNotFoundException(userName);
            }

            ShoppingCart basket = await _repository.GetBasket(userName, cancellationToken) 
                ?? throw new BasketNotFoundException(userName);

            await _cache.SetStringAsync(userName, JsonSerializer.Serialize(basket), token: cancellationToken);
            return basket;
        }

        public async Task<ShoppingCart> StoreBasket(ShoppingCart basket, CancellationToken cancellationToken = default)
        {
            await _repository.StoreBasket(basket, cancellationToken);
            await _cache.SetStringAsync(basket.UserName, JsonSerializer.Serialize(basket), token: cancellationToken);
            return basket;
        }

        public async Task<bool> DeleteBasket(string userName, CancellationToken cancellationToken = default)
        {
            bool deleted = await _repository.DeleteBasket(userName, cancellationToken);
            await _cache.RemoveAsync(userName, cancellationToken);
            return deleted;
        }
    }
}
