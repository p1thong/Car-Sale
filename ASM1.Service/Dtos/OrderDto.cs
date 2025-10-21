using System.Collections.Generic;

namespace ASM1.Service.Dtos
{
    public class OrderDto
    {
        public int OrderId { get; set; }
        public int DealerId { get; set; }
        public int CustomerId { get; set; }
        public int VariantId { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateOnly? OrderDate { get; set; }
        public int Quantity { get; set; } = 1;
        public decimal? TotalPrice { get; set; }
        public string? ImageUrl { get; set; }
        public string? Notes { get; set; }

        // Flattened properties from related entities
        public string? CustomerName { get; set; }
        public string? CustomerEmail { get; set; }
        public string? CustomerPhone { get; set; }
        public string? DealerName { get; set; }
        public string? ManufacturerName { get; set; }
        public string? ModelName { get; set; }
        public string? VariantVersion { get; set; }
        public string? VariantColor { get; set; }
        public int? ProductYear { get; set; }
        public decimal? Price { get; set; }

        // Payment related properties
        public List<PaymentDto> Payments { get; set; } = new List<PaymentDto>();
        public decimal? TotalPaid { get; set; }
        public decimal? RemainingAmount { get; set; }
        public bool IsFullyPaid { get; set; }
    }
}