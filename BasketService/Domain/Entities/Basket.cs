using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    /// <summary>
    /// корзина конкретного пользователя(она одна у каждого юзера, поэтому, считаем ее ID как UserId Для простоты
    /// </summary>
    public class Basket
    {
        public Guid UserId { get; set; }

        public List<BasketItem> Items { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public decimal TotalPrice => Items.Sum(x => x.Price * x.Amount);

    }
}
