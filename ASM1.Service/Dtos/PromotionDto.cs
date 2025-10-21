namespace ASM1.Service.Dtos
{
    public class PromotionDto
    {
        public int PromotionId { get; set; }
        public int OrderId { get; set; }
        public decimal DiscountAmount { get; set; }
        public string? PromotionCode { get; set; }
        public DateOnly? ValidUntil { get; set; }
    }
}