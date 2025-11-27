using Domain.Entities;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Sagas
{
    public class BasketCheckoutState : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }
        public string CurrentState { get; set; }

        public Guid BasketId { get; set; }
        public Guid UserId { get; set; }
        public List<BasketItem> Items { get; set; }
        public decimal TotalAmount { get; set; }
        public Guid? ReservationId { get; set; }
        public Guid? OrderId { get; set; }
        public string FailureReason { get; set; }
        public DateTime CheckoutStartedAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public byte[] RowVersion { get; set; }
    }
}
