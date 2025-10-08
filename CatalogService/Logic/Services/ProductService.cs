using CatalogService.CoreLib.DTOs;
using CatalogService.CoreLib.DTOs.Requests;
using CatalogService.CoreLib.Entities;
using CatalogService.CoreLib.Interfaces;
using CatalogService.Logic.Interfaces;

namespace CatalogService.Logic.Services
{
    

    public class ProductService : IProductService
    {

        private readonly IProductRepository _productRepository;
        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<ProductDetailsDto> CreateProductAsync(CreateProductRequest request)
        {
            var product = new Product
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                CategoryId = request.CategoryId,
                Brand = request.Brand,
                Rating = 0,
                ReviewCount = 0,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                ImageUrl = request.ImageUrl
            };

            var newProduct = await _productRepository.AddProductAsync(product);
            return MapToProductDetailsDto(newProduct);

        }

        public async Task<bool> DeleteProductAsync(Guid id)
        {
            return await _productRepository.DeleteProductAsync(id);
        }

        public async Task<ProductDetailsDto> GetProductAsync(Guid id)
        {

            return MapToProductDetailsDto(await _productRepository.GetProductByIdAsync(id));
        }

        public async Task<List<ProductDto>> GetProductsAsync(ProductFilter filter)
        {
            var products = await _productRepository.GetProductsAsync(filter);
            return products.Select(MapToProductDto).ToList();
        }

        public async Task<ProductDetailsDto> UpdateProductAsync(Guid id, UpdateProductRequest request)
        {
            var product = await _productRepository.GetProductByIdAsync(id);
            if (product == null) return null;

            product.Name = request.Name;
            product.Description = request.Description;
            product.Price = request.Price;
            product.CategoryId = request.CategoryId;
            product.Brand = request.Brand;
            product.IsActive = request.IsActive;
            product.ImageUrl = request.url;

            var updated = await _productRepository.UpdateProductAsync(product);
            return MapToProductDetailsDto(updated);

        }

        private ProductDto MapToProductDto(Product product)
        {
            return new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Category = product.Category?.Name,
                Brand = product.Brand,
                Rating = product.Rating,
                ReviewCount = product.ReviewCount,
                imageUrl = product.ImageUrl,
                CreatedAt = product.CreatedAt
            };
        }

        private ProductDetailsDto MapToProductDetailsDto(Product product)
        {
            return new ProductDetailsDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Category = product.Category?.Name,
                CategoryId = product.CategoryId,
                Brand = product.Brand,
                Rating = product.Rating,
                ReviewCount = product.ReviewCount,
                url = product.ImageUrl,
                Reviews = product.Reviews.Select(r => new ProductReview
                {
                    Id = r.Id,
                    UserName = r.UserName,
                    Rating = r.Rating,
                    Comment = r.Comment,
                    CreatedAt = r.CreatedAt,
                    Product = product
                }).ToList()
            };
        }
    }
}
