using Domain.Entities;
using Domain.Interfaces;
using StackExchange.Redis;
using System;
using System.Text.Json;

using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class BasketRepository : IBasketRepository
    {

        private readonly IDatabase _database;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public BasketRepository(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
            _jsonSerializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }

        public async Task<bool> ClearBasketAsync(Guid userId)
        {
            return await _database.KeyDeleteAsync(userId.ToString());
        }

        public async Task<Basket> GetBasketAsync(Guid userId)
        {
            var data = await _database.StringGetAsync(userId.ToString());
            return data.IsNullOrEmpty ? null : JsonSerializer.Deserialize<Basket>(data, _jsonSerializerOptions);
        }

        public async Task<Basket> UpdateBasketAsync(Basket basket)
        {
            basket.UpdatedAt = DateTime.UtcNow;
            var serializedBasket = JsonSerializer.Serialize(basket, _jsonSerializerOptions);

            await _database.StringSetAsync(basket.UserId.ToString(), serializedBasket, TimeSpan.FromDays(60));
            return basket;
        }
    }
}
