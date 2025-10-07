namespace CatalogService.CoreLib.Entities
{
    /// <summary>
    /// таблица отзывов на товар
    /// (?, возможно имеет смысл вынести в отдельный микросервис рейтинговую систему, тут надо подумать)
    /// </summary>
    public class ProductReview
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public Guid UserId { get; set; }

        public required string UserName { get; set; }

        public int Rating { get; set; }

        public string? Comment { get; set; }

        public DateTime CreatedAt { get; set; }

        public required Product Product { get; set; }

    }
}
