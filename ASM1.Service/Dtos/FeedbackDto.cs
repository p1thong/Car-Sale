namespace ASM1.Service.Dtos
{
    public class FeedbackDto
    {
        public int FeedbackId { get; set; }
        public int CustomerId { get; set; }
        public string? Content { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? FeedbackDate { get; set; }
        public int? Rating { get; set; }
        public int? VehicleModelId { get; set; }

        // Flattened properties
        public string? CustomerName { get; set; }
        public string? CustomerEmail { get; set; }
        public string? ModelName { get; set; }
    }
}
