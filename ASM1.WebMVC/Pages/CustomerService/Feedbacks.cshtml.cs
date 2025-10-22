using ASM1.Service.Dtos;
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

        public IEnumerable<FeedbackDto> Feedbacks { get; set; } = new List<FeedbackDto>();

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                // Check user role
                var userRole = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;
                Console.WriteLine($"Feedbacks page - User role: {userRole}");

                if (userRole?.Equals("Customer", StringComparison.OrdinalIgnoreCase) == true)
                {
                    // Customer: Show only their own feedbacks
                    var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
                    if (string.IsNullOrEmpty(email))
                    {
                        TempData["Error"] = "Vui lòng đăng nhập.";
                        return RedirectToPage("/Auth/Login");
                    }

                    var customer = await _customerService.GetCustomerByEmailAsync(email);
                    if (customer == null)
                    {
                        TempData["Error"] = "Không tìm thấy thông tin khách hàng.";
                        return RedirectToPage("/Home/Index");
                    }

                    Feedbacks = await _customerService.GetCustomerFeedbacksAsync(
                        customer.CustomerId
                    );
                    Console.WriteLine(
                        $"Feedbacks loaded: {Feedbacks?.Count() ?? 0} feedbacks for customer {customer.CustomerId}"
                    );
                }
                else
                {
                    // Dealer: Show all feedbacks for their dealership
                    var currentDealerId = GetCurrentDealerId() ?? 1;
                    Feedbacks = await _customerService.GetFeedbacksByDealerAsync(currentDealerId);
                    Console.WriteLine(
                        $"Feedbacks loaded: {Feedbacks?.Count() ?? 0} feedbacks for dealer {currentDealerId}"
                    );
                }

                return Page();
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Lỗi khi tải danh sách phản hồi: {ex.Message}";
                Console.WriteLine($"Error loading feedbacks: {ex.Message}");
                Feedbacks = new List<FeedbackDto>();
                return Page();
            }
        }
    }
}
