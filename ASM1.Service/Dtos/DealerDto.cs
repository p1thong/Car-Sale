namespace ASM1.Service.Dtos
{
    public class DealerDto
    {
        public int DealerId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public int? TransactionId { get; set; }
        // Note: Password excluded for security
    }
}