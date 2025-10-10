using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class BasketLogDbContext : DbContext
    {

        public DbSet<BasketOperationLog> BasketOperationsLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BasketOperationLog>(entity =>
            {
                entity.HasKey(e => e.Id);
            });
            base.OnModelCreating(modelBuilder);
        }
    }
}
