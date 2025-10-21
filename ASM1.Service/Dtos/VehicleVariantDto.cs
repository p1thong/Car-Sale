namespace ASM1.Service.Dtos
{
    public class VehicleVariantDto
    {
        public int VariantId { get; set; }
        public int VehicleModelId { get; set; }
        public string Version { get; set; } = string.Empty;
        public string? Color { get; set; }
        public int? ProductYear { get; set; }
        public decimal? Price { get; set; }
        public int Quantity { get; set; }
    }
}