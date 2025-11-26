using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public interface IBasketCheckoutStarted
    {
        Guid BasketId { get; }
        Guid UserId { get; }
        List<BasketItem> Items { get; }
        DateTime CheckoutStartedAt { get; }
    }

    public interface IInventoryReserved
    {
        Guid BasketId { get; }
        Guid ReservationId { get; }
    }

    public interface IInventoryReservationFailed
    {
        Guid BasketId { get; }
        string Reason { get; }
    }
    public interface IOrderCreated
    {
        Guid BasketId { get; }
        Guid OrderId { get; }
    }
    public interface IOrderCreationFailed
    {
        Guid BasketId { get; }
        string Reason { get; }
    }
    public interface IBasketCheckoutCompleted
    {
        Guid BasketId { get; }
        Guid OrderId { get; }
    }

    public interface IBasketCheckoutFailed
    {
        Guid BasketId { get; }
        string Reason { get; }
    }
    public interface IReserveInventoryArguments
    {
        Guid BasketId { get; }
        List<BasketItem> Items { get; }
    }
    public interface ICreateOrderArguments
    {
        Guid BasketId { get; }
        Guid UserId { get; }
        List<BasketItem> Items { get; }
        decimal TotalAmount { get; }
    }
    public interface ICancelInventoryReservationArguments
    {
        Guid BasketId { get; }
        Guid ReservationId { get; }
    }
}
