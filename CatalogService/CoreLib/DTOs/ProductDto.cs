namespace CatalogService.CoreLib.DTOs
{
    public class ProductDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
        public string Brand { get; set; }
        public decimal? Rating { get; set; }
        public int? ReviewCount { get; set; }
        public string imageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
