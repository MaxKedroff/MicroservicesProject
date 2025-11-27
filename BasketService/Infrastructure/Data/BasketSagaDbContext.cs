using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class BasketSagaDbContext : SagaDbContext
    {
        public BasketSagaDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override IEnumerable<ISagaClassMap> Configurations
        {
            get
            {
                yield return new BasketCheckoutStateMap();
            }
        }
    }
}
