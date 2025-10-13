using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IBasketRepository
    {

        Task<Basket> GetBasketAsync(Guid userId);


        Task<Basket> UpdateBasketAsync(Basket basket);


        
        Task<bool> ClearBasketAsync(Guid userId);
    }
}
