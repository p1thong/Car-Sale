namespace ASM1.Service.Dtos
{
    public class DealerContractDto
    {
        public int DealerContractId { get; set; }
        public int DealerId { get; set; }
        public int ManufacturerId { get; set; }
        public decimal? TargetSales { get; set; }
        public decimal? CreditLimit { get; set; }
        public DateOnly? SignedDate { get; set; }
    }
}