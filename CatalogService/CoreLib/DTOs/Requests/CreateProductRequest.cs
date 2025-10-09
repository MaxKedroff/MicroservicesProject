namespace CatalogService.CoreLib.DTOs.Requests
{
    public class CreateProductRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public string Brand { get; set; }
        public string ImageUrl { get; set; } = "";
    }
}
