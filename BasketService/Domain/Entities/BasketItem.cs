using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    /// <summary>
    /// Элемент корзины(Товар). Данные по нему будут собираться с CatalogService
    /// </summary>
    public class BasketItem
    {
        public Guid ProductId { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public int Amount { get; set; }

        public string ImageUrl { get; set; }
    }
}
