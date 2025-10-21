namespace ASM1.Service.Dtos
{
    public class SalesContractDto
    {
        public int SaleContractId { get; set; }
        public int OrderId { get; set; }
        public DateOnly? SignedDate { get; set; }
        public DateOnly? ContractDate { get; set; }
        public decimal? TotalAmount { get; set; }
        public string? Terms { get; set; }
        public string? Status { get; set; }
    }
}