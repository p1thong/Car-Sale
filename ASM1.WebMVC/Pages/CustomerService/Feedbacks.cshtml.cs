using ASM1.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ASM1.WebMVC.Pages.CustomerService
{
    public class FeedbacksModel : BasePageModel
    {
        private readonly ICustomerRelationshipService _customerService;

        public FeedbacksModel(ICustomerRelationshipService customerService)
        {
            _customerService = customerService;
        }

        public IEnumerable<Feedback> Feedbacks { get; set; } = new List<Feedback>();

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                // Lấy dealerId giống logic trong Controller cũ
                var currentDealerId = GetCurrentDealerId() ?? 1; // Fallback về 1 như trong Controller
                
                Feedbacks = await _customerService.GetFeedbacksByDealerAsync(currentDealerId);
                
                // Debug logging
                Console.WriteLine($"Feedbacks loaded: {Feedbacks?.Count() ?? 0} feedbacks for dealer {currentDealerId}");
                
                return Page();
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Lỗi khi tải danh sách phản hồi: {ex.Message}";
                Console.WriteLine($"Error loading feedbacks: {ex.Message}");
                Feedbacks = new List<Feedback>(); // Khởi tạo empty list để tránh null reference
                return Page();
            }
        }
    }
}
