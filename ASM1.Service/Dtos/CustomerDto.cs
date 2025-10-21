namespace ASM1.Service.Dtos
{
    public class CustomerDto
    {
        public int CustomerId { get; set; }
        public int DealerId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public DateOnly? Birthday { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public string? DealerName { get; set; }
    }
}