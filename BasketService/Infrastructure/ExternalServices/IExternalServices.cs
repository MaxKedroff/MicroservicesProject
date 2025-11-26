using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.ExternalServices
{
    public interface ICatalogService
    {
        Task<ProductInfo> GetProductInfoAsync(Guid productId);
    }

    public class ProductInfo
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = "";
        public decimal Price { get; set; }
        public string ImageUrl { get; set; } = "";
        public bool IsActive { get; set; }
    }
}
