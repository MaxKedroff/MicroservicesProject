using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CatalogService.CoreLib.Entities
{
    public class CatalogReservation
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid BasketId { get; set; } 

        [Required]
        public DateTime ReservedAt { get; set; }

        public DateTime? CancelledAt { get; set; }

        [Required]
        [MaxLength(20)]
        public string Status { get; set; } = "Reserved"; 

        public virtual List<ReservationItem> Items { get; set; } = new();
    }

    public class ReservationItem
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid ProductId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public Guid ReservationId { get; set; }

        [ForeignKey("ReservationId")]
        public virtual CatalogReservation Reservation { get; set; }
    }
}
