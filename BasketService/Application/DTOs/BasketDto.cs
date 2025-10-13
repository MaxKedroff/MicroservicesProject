using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class BasketDto
    {
        public Guid UserId { get; set; }
        public List<BasketItemDto> Items { get; set; }
        public decimal TotalPrice { get; set; }
        public int ItemsCount { get; set; }
    }
    public class BasketItemDto
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Amount { get; set; }
        public string ImageUrl { get; set; }
        public decimal TotalPrice => Price * Amount;
    }

    public class AddItemRequest
    {
        public Guid ProductId { get; set; }
        public int Amount { get; set; }
    }

    public class UpdateItemRequest
    {
        public int Amount { get; set; }
    }

    public class OperationLogDto
    {
        public string Operation { get; set; }
        public Guid? ProductId { get; set; }
        public int? Amount { get; set; }
        public DateTime Timestamp { get; set; }
        public string Details { get; set; }
    }
}
