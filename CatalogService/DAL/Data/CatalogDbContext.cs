using CatalogService.CoreLib.Entities;
using Microsoft.EntityFrameworkCore;


namespace CatalogService.DAL.Data
{
    public class CatalogDbContext : DbContext
    {
        public CatalogDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<CatalogReservation> CatalogReservations { get; set; }
        public DbSet<ReservationItem> ReservationItems { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<ProductReview> ProductReviews { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId);
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(c => c.Id);
                entity.HasOne(c => c.ParentCategory)
                .WithMany(c => c.SubCategories)
                .HasForeignKey(c => c.ParentCategoryId)
                .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<ProductReview>(entity =>
            {
                entity.HasKey(pr => pr.Id);
                entity.HasOne(pr => pr.Product)
                .WithMany(p => p.Reviews)
                .HasForeignKey(pr => pr.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<CatalogReservation>(entity =>
            {
                entity.HasKey(r => r.Id);
                entity.Property(r => r.Status).HasMaxLength(20);
                entity.HasIndex(r => r.BasketId); 

                entity.HasMany(r => r.Items)
                      .WithOne(i => i.Reservation)
                      .HasForeignKey(i => i.ReservationId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<ReservationItem>(entity =>
            {
                entity.HasKey(i => i.Id);
                entity.HasIndex(i => i.ProductId); 
            });


            base.OnModelCreating(modelBuilder);
        }


    }
}
