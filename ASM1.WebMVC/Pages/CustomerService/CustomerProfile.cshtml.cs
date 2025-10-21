using ASM1.Service.Dtos;
using ASM1.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ASM1.WebMVC.Pages.CustomerService
{
    public class CustomerProfileModel : BasePageModel
    {
        private readonly ICustomerRelationshipService _customerService;
        private readonly ISalesService _salesService;

        public CustomerProfileModel(ICustomerRelationshipService customerService, ISalesService salesService)
        {
            _customerService = customerService;
            _salesService = salesService;
        }

        public CustomerDto? Customer { get; set; }
        public IEnumerable<OrderDto> Orders { get; set; } = new List<OrderDto>();
        public IEnumerable<TestDriveDto> TestDrives { get; set; } = new List<TestDriveDto>();
        public int OrderCount { get; set; }
        public int TestDriveCount { get; set; }
        public int FeedbackCount { get; set; }
        public decimal TotalSpent { get; set; }

        public async Task<IActionResult> OnGetAsync(int customerId)
        {
            try
            {
                Customer = await _customerService.GetCustomerByIdAsync(customerId);

                if (Customer == null)
                {
                    TempData["Error"] = "Không tìm thấy thông tin khách hàng.";
                    return RedirectToPage("./TestDrives");
                }

                // Get orders
                Orders = await _salesService.GetOrdersByCustomerAsync(customerId);
                OrderCount = Orders.Count();
                TotalSpent = Orders.Where(o => o.Status == "Completed")
                                  .Sum(o => o.Price ?? 0);

                // Get test drives
                TestDrives = await _customerService.GetCustomerTestDrivesAsync(customerId);
                TestDriveCount = TestDrives.Count();

                // Note: GetFeedbacksByCustomerAsync might not exist in service
                // Using dealer feedbacks as alternative
                try
                {
                    var dealerId = Customer.DealerId;
                    if (dealerId > 0)
                    {
                        var allFeedbacks = await _customerService.GetFeedbacksByDealerAsync(dealerId);
                        FeedbackCount = allFeedbacks.Count(f => f.CustomerId == customerId);
                    }
                    else
                    {
                        FeedbackCount = 0;
                    }
                }
                catch
                {
                    FeedbackCount = 0;
                }

                return Page();
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Lỗi khi tải hồ sơ khách hàng: {ex.Message}";
                return RedirectToPage("./TestDrives");
            }
        }
    }
}
