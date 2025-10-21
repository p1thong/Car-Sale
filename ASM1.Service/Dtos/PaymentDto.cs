namespace ASM1.Service.Dtos
{
    public class PaymentDto
    {
        public int PaymentId { get; set; }
        public int OrderId { get; set; }
        public decimal Amount { get; set; }
        public string Method { get; set; } = string.Empty;
        public DateTime PaymentDate { get; set; }
        public string? PaymentMethod { get; set; }
        public string? Status { get; set; }
    }
}