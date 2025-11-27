using Domain.Entities;
using MassTransit;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Sagas;

namespace Infrastructure.Data
{
    class BasketCheckoutStateMap : SagaClassMap<BasketCheckoutState>
    {
        protected override void Configure(EntityTypeBuilder<BasketCheckoutState> entity, ModelBuilder model)
        {
            entity.Property(x => x.CurrentState).HasMaxLength(64);
            entity.Property(x => x.RowVersion).IsRowVersion();
        }
    }
}
