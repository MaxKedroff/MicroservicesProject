using CatalogService.CoreLib.Entities;

namespace CatalogService.CoreLib.DTOs
{
    public class ProductDetailsDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
        public int? CategoryId { get; set; }
        public string Brand { get; set; }
        public decimal? Rating { get; set; }
        public int? ReviewCount { get; set; }
        public string url { get; set; } = "";
        public List<ProductReview> Reviews { get; set; } = new();
    }
}
