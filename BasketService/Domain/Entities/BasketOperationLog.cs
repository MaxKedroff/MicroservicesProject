using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    /// <summary>
    /// сущность лога операции над корзиной. Хотим хранить каждое действие пользователя для возможной аналитики.
    /// </summary>
    public class BasketOperationLog
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Operation { get; set; }

        public Guid? ProductId { get; set; }

        public int? Amount { get; set; }
        public DateTime Timestamp { get; set; }

        public string details { get; set; }
    }
}
