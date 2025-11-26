namespace CatalogService.CoreLib.Entities
{
    /// <summary>
    /// товар который будет показан на витрине сайта,  может иметь категорию, и отзывы на него
    /// </summary>
    public class Product
    {

        public Guid Id { get; set; }

        public required string Name { get; set; }

        public required string Description { get; set; }

        public required decimal Price { get; set; }

        public int? CategoryId { get; set; }

        public string? Brand { get; set; }

        public decimal? Rating { get; set; }

        public int? ReviewCount { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }

        public string? ImageUrl { get; set; }

        public int StockQuantity { get; set; }

        public Category? Category { get; set; }
        public List<ProductReview> Reviews { get; set; } = new();


    }
}
