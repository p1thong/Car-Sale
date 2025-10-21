namespace ASM1.Service.Dtos
{
    public class TestDriveDto
    {
        public int TestDriveId { get; set; }
        public int CustomerId { get; set; }
        public int VariantId { get; set; }
        public DateOnly? ScheduledDate { get; set; }
        public TimeOnly? ScheduledTime { get; set; }
        public string? Status { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerEmail { get; set; }
        public string? CustomerPhone { get; set; }
        public string? ModelName { get; set; }
        public string? VariantVersion { get; set; }
        public string? VariantColor { get; set; }
        public string? ImageUrl { get; set; }
        public int? ProductYear { get; set; }
        public string? ManufacturerName { get; set; }
        public decimal? Price { get; set; }
        public int? VehicleModelId { get; set; }

        // Enriched properties
        public string CustomerNameDisplay { get; set; } = string.Empty;
        public string ModelNameDisplay { get; set; } = string.Empty;
        public string VariantNameDisplay { get; set; } = string.Empty;
    }
}