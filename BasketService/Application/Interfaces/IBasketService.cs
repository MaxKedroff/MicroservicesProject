using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IBasketService
    {
        Task<Basket> GetBasketAsync(Guid userId);
        Task<Basket> AddItemAsync(Guid userId, BasketItem item);
        Task<Basket> UpdateItemAsync(Guid userId, Guid productId, int amount);
        Task<bool> RemoveItemAsync(Guid userId, Guid productId);
        Task<bool> ClearBasketAsync(Guid userId);
    }
}
