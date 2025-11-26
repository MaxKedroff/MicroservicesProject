using CatalogService.CoreLib.Entities;
using CatalogService.DAL.Data;
using CatalogService.Saga.Contracts;
using MassTransit;
using Microsoft.EntityFrameworkCore;



namespace CatalogService.Saga.Activities
{
    public class ReserveInventoryActivity : IExecuteActivity<IReserveInventoryArguments>
    {
        private readonly CatalogDbContext _context;

        public ReserveInventoryActivity(CatalogDbContext context)
        {
            _context = context;
        }

        public async Task<ExecutionResult> Execute(ExecuteContext<IReserveInventoryArguments> context)
        {
            var arguments = context.Arguments;

            try
            {
                foreach (var item in arguments.Items)
                {
                    var product = await _context.Products
                        .FirstOrDefaultAsync(p => p.Id == item.ProductId);

                    if (product == null)
                    {
                        throw new InvalidOperationException($"Product {item.ProductId} not found");
                    }

                    if (product.StockQuantity < item.Quantity)
                    {
                        throw new InvalidOperationException($"Insufficient stock for product {product.Name}. Available: {product.StockQuantity}, Requested: {item.Quantity}");
                    }
                }

                var reservation = new CatalogReservation
                {
                    Id = Guid.NewGuid(),
                    BasketId = arguments.BasketId,
                    Items = arguments.Items.Select(i => new ReservationItem
                    {
                        ProductId = i.ProductId,
                        Quantity = i.Quantity,
                    }).ToList(),
                    ReservedAt = DateTime.UtcNow,
                    Status = "Reserved"
                };

                _context.CatalogReservations.Add(reservation);

                foreach (var item in arguments.Items)
                {
                    var product = await _context.Products
                        .FirstOrDefaultAsync(p => p.Id == item.ProductId);
                    product.StockQuantity -= item.Quantity;
                }
                await _context.SaveChangesAsync();
                return context.CompletedWithVariables(new { ReservationId = reservation.Id });
            }catch (Exception ex)
            {
                throw new InvalidOperationException($"Inventory reservation failed: {ex.Message}");
            }
        }
    }
}
