namespace ASM1.Service.Dtos
{
    public class WarrantyDto
    {
        public int WarrantyId { get; set; }
        public int OrderId { get; set; }
        public int CustomerId { get; set; }
        public int DealerId { get; set; }
        
        public string WarrantyType { get; set; } = string.Empty; // "ManufacturerDefect" or "PeriodicMaintenance"
        public string Reason { get; set; } = string.Empty;
        
        public DateTime RequestDate { get; set; }
        public DateTime? DealerConfirmedDate { get; set; }
        public DateTime? RepairCompletedDate { get; set; }
        public DateTime? CustomerReceivedDate { get; set; }
        
        public string Status { get; set; } = "Pending";
        public string? Notes { get; set; }
        
        // Flattened properties for display
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public string DealerName { get; set; } = string.Empty;
        public string VehicleModelName { get; set; } = string.Empty;
        public string VariantVersion { get; set; } = string.Empty;
        public DateTime? OrderDate { get; set; }
        public DateTime? WarrantyExpiryDate { get; set; } // Calculated from OrderDate
    }
}
