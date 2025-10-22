using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASM1.Repository.Models
{
    [Table("Warranty")]
    public class Warranty
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int WarrantyId { get; set; }

        [Required]
        public int OrderId { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Required]
        public int DealerId { get; set; }

        [Required]
        [StringLength(50)]
        public string WarrantyType { get; set; } = string.Empty; // "ManufacturerDefect" or "PeriodicMaintenance"

        [Required]
        [StringLength(500)]
        public string Reason { get; set; } = string.Empty;

        [Required]
        public DateTime RequestDate { get; set; }

        public DateTime? DealerConfirmedDate { get; set; }

        public DateTime? RepairCompletedDate { get; set; }

        public DateTime? CustomerReceivedDate { get; set; }

        [Required]
        [StringLength(50)]
        public string Status { get; set; } = "Pending"; // Pending, DealerConfirmed, RepairCompleted, CustomerReceived

        [StringLength(1000)]
        public string? Notes { get; set; }

        // Navigation properties
        [ForeignKey("OrderId")]
        public virtual Order? Order { get; set; }

        [ForeignKey("CustomerId")]
        public virtual Customer? Customer { get; set; }

        [ForeignKey("DealerId")]
        public virtual Dealer? Dealer { get; set; }
    }
}
