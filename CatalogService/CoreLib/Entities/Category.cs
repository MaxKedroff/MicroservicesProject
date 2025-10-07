namespace CatalogService.CoreLib.Entities
{
    /// <summary>
    /// Category - категория товара(у товара их может быть несколько), также у одной категории могут быть подкатегории
    /// и наоборот, категория может являться подкатегорией другой категории
    /// </summary>
    public class Category
    {
        public Guid Id { get; set; }

        public required string Name { get; set; }

        public required string Description { get; set; }

        public int? ParentCategoryId { get; set; }

        public bool IsActive { get; set; }

        public int ProductCount { get; set; }

        public Category? ParentCategory { get; set; }
        public List<Category>? SubCategories { get; set; } = new();

        public List<Product>? Products { get; set; } = new();
    }
}
