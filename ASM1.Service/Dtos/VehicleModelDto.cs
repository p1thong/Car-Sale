namespace ASM1.Service.Dtos
{
    public class VehicleModelDto
    {
        public int VehicleModelId { get; set; }
        public int ManufacturerId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Category { get; set; }
        public string? ImageUrl { get; set; }
    }
}