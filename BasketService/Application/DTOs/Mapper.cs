
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class Mapper
    {
        public static BasketDto MapToDto(Basket basket)
        {
            return new BasketDto
            {
                UserId = basket.UserId,
                Items = basket.Items.Select(x => new BasketItemDto
                {
                    ProductId = x.ProductId,
                    ProductName = x.Name,
                    Price = x.Price,
                    Amount = x.Amount,
                    ImageUrl = x.ImageUrl
                }).ToList(),
                TotalPrice = basket.TotalPrice,
                ItemsCount = basket.Items.Count
            };
        }
    }
}
