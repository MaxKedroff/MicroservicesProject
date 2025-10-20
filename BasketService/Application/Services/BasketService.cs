using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.ExternalServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class BasketService : IBasketService
    {

        private readonly IBasketRepository _basketRepository;
        private readonly IBasketLogService _logService;
        private readonly ICatalogService _catalogService;


        public BasketService(IBasketRepository basketRepository, IBasketLogService logService, ICatalogService catalogService)
        {
            _basketRepository = basketRepository;
            _logService = logService;
            _catalogService = catalogService;
        }

        public async Task<Basket> AddItemAsync(Guid userId, BasketItem item)
        {
            var product = await _catalogService.GetProductInfoAsync(item.ProductId);

            item.Name = product.Name;
            item.Price = product.Price;
            item.ImageUrl = product.ImageUrl;

            var basket = await GetBasketAsync(userId);

            var existingItem = basket.Items.FirstOrDefault(x => x.ProductId == item.ProductId);
            if (existingItem != null)
            {
                existingItem.Amount += item.Amount;
                await _logService.LogOperationAsync(userId, "Update", item.ProductId, existingItem.Amount, $"Increased quantity to {existingItem.Amount}");

            }
            else
            {
                basket.Items.Add(item);
                await _logService.LogOperationAsync(userId, "Add", item.ProductId, existingItem.Amount, $"Added {item.Name} to Basket");
            }
            basket.UpdatedAt = DateTime.UtcNow;
            return await _basketRepository.UpdateBasketAsync(basket);
        }

        public async Task<bool> ClearBasketAsync(Guid userId)
        {
            await _logService.LogOperationAsync(userId, "Clear", null, null, "Cleared entire basket");
            return await _basketRepository.ClearBasketAsync(userId);

        }

        public async Task<Basket> GetBasketAsync(Guid userId)
        {
            return await _basketRepository.GetBasketAsync(userId) ?? new Basket { UserId = userId };
        }

        public async Task<bool> RemoveItemAsync(Guid userId, Guid productId)
        {
            var basket = await GetBasketAsync(userId);
            var item = basket.Items.FirstOrDefault(itm => itm.ProductId == productId);
            if (item == null) return false;

            basket.Items.Remove(item);
            basket.UpdatedAt = DateTime.UtcNow;
            await _logService.LogOperationAsync(userId, "Remove", productId, null, $"Removed {item.Name} from basket");
            await _basketRepository.UpdateBasketAsync(basket);
            return true;
        }

        public async Task<Basket> UpdateItemAsync(Guid userId, Guid productId, int amount)
        {
            if (amount <= 0)
            {
                await RemoveItemAsync(userId, productId);
                return await GetBasketAsync(userId);
            }

            var basket = await GetBasketAsync(userId);
            var item = basket.Items.FirstOrDefault(x => x.ProductId == productId);
            if (item == null) return basket;

            item.Amount = amount;
            basket.UpdatedAt = DateTime.UtcNow;

            await _logService.LogOperationAsync(userId, "Update", productId, amount, $"Updated quantity to {amount}");

            return await _basketRepository.UpdateBasketAsync(basket);
        }
    }
}
