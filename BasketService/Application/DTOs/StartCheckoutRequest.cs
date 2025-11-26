using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class StartCheckoutRequest
    {
        public Guid UserId { get; init; }
        public List<BasketItem> Items { get; init; }
    }
}
