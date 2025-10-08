using CatalogService.CoreLib.DTOs;
using CatalogService.CoreLib.Entities;

namespace CatalogService.CoreLib.Utils
{
    public class Mapper
    {
        public static CategoryDto MapToCategoryDto(Category category)
        {
            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                ParentCategoryId = category.ParentCategoryId,
                ProductCount = category.ProductCount,
            };
        }

        public static ProductDto MapToProductDto(Product product)
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

        public static ProductDetailsDto MapToProductDetailsDto(Product product)
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
