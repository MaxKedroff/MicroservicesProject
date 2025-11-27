using CatalogService.DAL.Data;
using CatalogService.Saga.Contracts;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Saga.Activities
{
    public class CancelInventoryReservationActivity : ICompensateActivity<IReserveInventoryArguments>
    {
        private readonly CatalogDbContext _context;
        
        public CancelInventoryReservationActivity(CatalogDbContext context)
        {
            _context = context;
        }

        public async Task<CompensationResult> Compensate(CompensateContext<IReserveInventoryArguments> context)
        {
            
            if (context.Log is IDictionary<string, object> log && log.TryGetValue("ReservationId", out object reservationObj))
            {
                if (reservationObj is not Guid reservationId)
                {
                    return context.Compensated(); 
                }
                var reservation = await _context.CatalogReservations
                    .Include(r => r.Items)
                    .FirstOrDefaultAsync(r => r.Id == reservationId);

                if (reservation != null)
                {
                    foreach (var item in reservation.Items)
                    {
                        var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == item.ProductId);
                        if (product != null)
                        {
                            product.StockQuantity += item.Quantity;
                        }
                    }

                    reservation.Status = "Cancelled";
                    reservation.CancelledAt = DateTime.UtcNow;

                    await _context.SaveChangesAsync();
                }
            }
            return context.Compensated();
        }
    }
}
