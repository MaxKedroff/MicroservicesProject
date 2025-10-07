namespace CatalogService.CoreLib.DTOs
{
    /// <summary>
    /// DTO для передачи query параметров в запрос
    /// </summary>
    public class ProductFilter
    {
        public int? CategoryId { get; set; }
        public string? Brand { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public string SortBy { get; set; } = "name";
        public string SortOrder { get; set; } = "asc";
    }
}
