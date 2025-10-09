using CatalogService.CoreLib.DTOs;
using CatalogService.CoreLib.Entities;
using CatalogService.CoreLib.Interfaces;
using CatalogService.DAL.Data;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.DAL.Repositories
{
    public class ProductRepository : IProductRepository
    {

        private readonly CatalogDbContext _context;

        public ProductRepository(CatalogDbContext context)
        {
            _context = context;
        }

        public async Task<Product> AddProductAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<bool> DeleteProductAsync(Guid id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null) return false;

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<Product> GetProductByIdAsync(Guid id)
        {
            return await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Reviews.OrderByDescending(pr => pr.CreatedAt))
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<Product>> GetProductsAsync(ProductFilter filter)
        {
            var query = _context.Products
                .Include(p => p.Category)
                .AsQueryable();

            if (filter.CategoryId.HasValue)
                query = query.Where(p => p.CategoryId == filter.CategoryId);

            if (!string.IsNullOrEmpty(filter.Brand))
                query = query.Where(p => p.Brand == filter.Brand);

            if (filter.MinPrice.HasValue)
                query = query.Where(p => p.Price >= filter.MinPrice);

            if (filter.MinPrice.HasValue)
                query = query.Where(p => p.Price >= filter.MaxPrice);

            query = filter.SortBy.ToLower() switch
            {
                "price" => filter.SortOrder == "desc" ? query.OrderByDescending(p => p.Price) : query.OrderBy(p => p.Price),
                "rating" => filter.SortOrder == "desc" ? query.OrderByDescending(p => p.Rating) : query.OrderBy(p => p.Rating),
                "new" => filter.SortOrder == "desc" ? query.OrderByDescending(p => p.CreatedAt) : query.OrderBy(p => p.CreatedAt),
                _ => filter.SortOrder == "desc" ? query.OrderByDescending(p => p.Name) : query.OrderBy(p => p.Name),
            };

            return await query.ToListAsync();
        }

        public async Task<Product> UpdateProductAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
            return product;
        }
    }
}
