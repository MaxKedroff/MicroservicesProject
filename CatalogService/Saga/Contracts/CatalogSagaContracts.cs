namespace CatalogService.Saga.Contracts
{
    public interface IReserveInventoryArguments
    {
        Guid BasketId { get; }
        List<InventoryItemRequest> Items { get; }
    }

    public interface ICancelInventoryReservationArguments
    {
        Guid BasketId { get; }
        Guid ReservationId { get; }
    }

    public record InventoryItemRequest
    {
        public Guid ProductId { get; init; }
        public int Quantity { get; init; }
    }
}
