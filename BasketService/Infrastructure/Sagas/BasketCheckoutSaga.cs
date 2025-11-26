using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Contracts;
namespace Infrastructure.Sagas
{
    public class BasketCheckoutSaga : MassTransitStateMachine<BasketCheckoutState>
    {
        public State ReservingInventory { get; private set; }
        public State CreatingOrder { get; private set; }
        public State CancellingInventory { get; private set; }
        public State CheckoutCompleted { get; private set; }
        public State CheckoutFailed { get; private set; }

        public Event<IBasketCheckoutStarted> BasketCheckoutStarted { get; private set; }
        public Event<IInventoryReserved> InventoryReserved { get; private set; }
        public Event<IInventoryReservationFailed> InventoryReservationFailed { get; private set; }
        public Event<IOrderCreated> OrderCreated { get; private set; }
        public Event<IOrderCreationFailed> OrderCreationFailed { get; private set; }

        public BasketCheckoutSaga()
        {
            InstanceState(x => x.CurrentState);
            Event(() => BasketCheckoutStarted, x => x.CorrelateById(context => context.Message.BasketId));
            Event(() => InventoryReserved, x => x.CorrelateById(context => context.Message.BasketId));
            Event(() => InventoryReservationFailed, x => x.CorrelateById(context => context.Message.BasketId));
            Event(() => OrderCreated, x => x.CorrelateById(context => context.Message.BasketId));
            Event(() => OrderCreationFailed, x => x.CorrelateById(context => context.Message.BasketId));

            Initially(
                When(BasketCheckoutStarted)
                .Then(context =>
                {
                    context.Saga.BasketId = context.Message.BasketId;
                    context.Saga.UserId = context.Message.UserId;
                    context.Saga.Items = context.Message.Items.ToList();
                    context.Saga.CheckoutStartedAt = context.Message.CheckoutStartedAt;
                    context.Saga.TotalAmount = context.Message.Items.Sum(x => x.Price * x.Amount);
                })
                .PublishAsync(context => context.Init<IReserveInventoryArguments>(new
                {
                    context.Saga.BasketId,
                    context.Saga.Items
                }))
                .TransitionTo(ReservingInventory)
            );
            During(ReservingInventory,
                When(InventoryReserved)
                .Then(context => context.Saga.ReservationId = context.Message.ReservationId)
                .PublishAsync(context => context.Init<ICreateOrderArguments>(new
                {
                    context.Saga.BasketId,
                    context.Saga.UserId,
                    context.Saga.Items,
                    context.Saga.TotalAmount
                }))
                .TransitionTo(CreatingOrder),
                When(InventoryReservationFailed)
                    .Then(context => context.Saga.FailureReason = context.Message.Reason)
                    .PublishAsync(context => context.Init<IBasketCheckoutFailed>(new
                    {
                        context.Saga.BasketId,
                        context.Message.Reason
                    }))
                    .TransitionTo(CheckoutFailed)
            );
            During(CreatingOrder,
                When(OrderCreated)
                    .Then(context => context.Saga.OrderId = context.Message.OrderId)
                    .PublishAsync(context => context.Init<IBasketCheckoutCompleted>(new
                    {
                        context.Saga.BasketId,
                        context.Saga.OrderId.Value
                    }))
                    .TransitionTo(CheckoutCompleted)
                    .Finalize(),
                When(OrderCreationFailed)
                    .Then(context => context.Saga.FailureReason = context.Message.Reason)
                    .PublishAsync(context => context.Init<ICancelInventoryReservationArguments>(new
                    {
                        context.Saga.BasketId,
                        context.Message.Reason
                    }))
                    .TransitionTo(CheckoutFailed)
            );

            During(CancellingInventory,
                When(InventoryReservationFailed)
                .PublishAsync(context => context.Init<IBasketCheckoutFailed>(new
                {
                    context.Saga.BasketId,
                    context.Saga.FailureReason
                }))
                .TransitionTo(CheckoutFailed)
            );

            SetCompletedWhenFinalized();
        }
    }

    public interface IGetBasketCheckoutStatus
    {
        Guid BasketId { get; }
    }
}
