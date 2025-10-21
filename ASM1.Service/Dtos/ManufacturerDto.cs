namespace ASM1.Service.Dtos
{
    public class ManufacturerDto
    {
        public int ManufacturerId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Country { get; set; }
        public string? Address { get; set; }
    }
}