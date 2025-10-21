using ASM1.Service.Dtos;

namespace ASM1.Service.Services.Interfaces
{
    public interface ICustomerRelationshipService
    {
        // Customer Management
        Task<IEnumerable<CustomerDto>> GetCustomersByDealerAsync(int dealerId);
        Task<CustomerDto?> GetCustomerByIdAsync(int customerId);
        Task<CustomerDto?> GetCustomerByEmailAsync(string email);
        Task<CustomerDto> CreateCustomerAsync(CustomerDto customer);
        Task<CustomerDto> UpdateCustomerAsync(CustomerDto customer);

        // Test Drive Management
        Task<TestDriveDto> ScheduleTestDriveAsync(
            int customerId,
            int variantId,
            DateOnly scheduledDate,
            TimeOnly? scheduledTime = null
        );
        Task<TestDriveDto> ConfirmTestDriveAsync(int testDriveId);
        Task<TestDriveDto> CompleteTestDriveAsync(int testDriveId);
        Task<TestDriveDto?> GetTestDriveByIdAsync(int testDriveId);
        Task<IEnumerable<TestDriveDto>> GetTestDriveScheduleAsync(DateOnly date);
        Task<IEnumerable<TestDriveDto>> GetAllTestDrivesAsync();
        Task<IEnumerable<TestDriveDto>> GetCustomerTestDrivesAsync(int customerId);
        Task<IEnumerable<TestDriveDto>> GetTestDrivesByDealerAsync(int dealerId);
        Task<TestDriveDto> CreateTestDriveAsync(TestDriveDto testDrive);
        Task UpdateTestDriveStatusAsync(int testDriveId, string status);

        // Feedback Management
        Task<FeedbackDto> CreateFeedbackAsync(FeedbackDto feedback);
        Task<IEnumerable<FeedbackDto>> GetCustomerFeedbacksAsync(int customerId);
        Task<IEnumerable<FeedbackDto>> GetAllFeedbacksAsync();
        Task<IEnumerable<FeedbackDto>> GetFeedbacksByDealerAsync(int dealerId);

        // Customer Profile & History
        Task<CustomerProfileDto> GetCustomerProfileAsync(int customerId);
        Task<IEnumerable<OrderDto>> GetCustomerOrderHistoryAsync(int customerId);
        Task<IEnumerable<PaymentDto>> GetCustomerPaymentHistoryAsync(int customerId);
        Task<decimal> GetCustomerOutstandingBalanceAsync(int customerId);

        // Customer Care & Promotions
        Task<IEnumerable<PromotionDto>> GetCustomerEligiblePromotionsAsync(int customerId);
        Task<CustomerCareReportDto> GenerateCustomerCareReportAsync(
            int dealerId,
            DateTime fromDate,
            DateTime toDate
        );
    }

    public class CustomerProfileDto
    {
        public required CustomerDto Customer { get; set; }
        public required IEnumerable<OrderDto> Orders { get; set; }
        public required IEnumerable<TestDriveDto> TestDrives { get; set; }
        public required IEnumerable<FeedbackDto> Feedbacks { get; set; }
        public decimal TotalPurchaseAmount { get; set; }
        public decimal OutstandingBalance { get; set; }
    }

    public class CustomerCareReportDto
    {
        public int TotalCustomers { get; set; }
        public int TotalTestDrives { get; set; }
        public int TotalFeedbacks { get; set; }
        public double AverageRating { get; set; }
        public decimal TotalSales { get; set; }
        public required IEnumerable<CustomerDto> NewCustomers { get; set; }
        public required IEnumerable<FeedbackDto> RecentFeedbacks { get; set; }
    }
}
