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
    }
}