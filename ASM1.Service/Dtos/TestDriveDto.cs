namespace ASM1.Service.Dtos
{
    public class TestDriveDto
    {
        public int TestDriveId { get; set; }
        public int CustomerId { get; set; }
        public int VariantId { get; set; }
        public DateOnly? ScheduledDate { get; set; }
        public TimeOnly? ScheduledTime { get; set; }
        public string? Status { get; set; }
    }
}