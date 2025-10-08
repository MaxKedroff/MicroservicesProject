namespace CatalogService.CoreLib.DTOs
{
    public class CategoryDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? ParentCategoryId { get; set; }
        public int ProductCount { get; set; }
        public int DisplayOrder { get; set; }
    }
}
