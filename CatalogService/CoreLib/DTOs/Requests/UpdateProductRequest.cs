namespace CatalogService.CoreLib.DTOs.Requests
{
    public class UpdateProductRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public string Brand { get; set; }
        public bool IsActive { get; set; }
        public string url { get; set; } = "";
    }
}
