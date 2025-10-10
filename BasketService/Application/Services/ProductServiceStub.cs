using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    /// <summary>
    /// поскольку в моем сервисе корзина будет получать инфу о товаре из CatalogService
    /// собрал для понимания эту заглушку, соответственно на следующем ДЗ она будет удалена
    /// и будут собираться реальные данные из CatalogService - пока так
    /// </summary>
    class ProductServiceStub
    {

        public Task<ProductInfo> GetProductInfoAsync(Guid productId)
        {
            var products = new Dictionary<Guid, ProductInfo>
            {
                {
                    Guid.Parse("a1b2c3d4-1234-5678-9101-abcdef123456"),
                    new ProductInfo
                    {
                        Id = Guid.Parse("a1b2c3d4-1234-5678-9101-abcdef123456"),
                        Name = "iPhone 15",
                        Price = 999.99m,
                        ImageUrl = "https://example.com/iphone15.jpg",
                        IsActive = true
                    }
                },
                {
                    Guid.Parse("b2c3d4e5-2345-6789-1011-bcdef1234567"),
                    new ProductInfo
                    {
                        Id = Guid.Parse("b2c3d4e5-2345-6789-1011-bcdef1234567"),
                        Name = "MacBook Air",
                        Price = 1299.99m,
                        ImageUrl = "https://example.com/macbook-air.jpg",
                        IsActive = true
                    }
                },
                {
                    Guid.Parse("c3d4e5f6-3456-7890-1112-cdef12345678"),
                    new ProductInfo
                    {
                        Id = Guid.Parse("c3d4e5f6-3456-7890-1112-cdef12345678"),
                        Name = "AirPods Pro",
                        Price = 249.99m,
                        ImageUrl = "https://example.com/airpods-pro.jpg",
                        IsActive = true
                    }
                }
            };

            if (products.TryGetValue(productId, out var product))
            {
                return Task.FromResult(product);
            }

            return Task.FromResult(new ProductInfo
            {
                Id = productId,
                Name = $"Product {productId}",
                Price = 99.99m,
                ImageUrl = "https://example.com/default-product.jpg",
                IsActive = true
            });
        }
    }

    public class ProductInfo
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public bool IsActive { get; set; }
    }
}
