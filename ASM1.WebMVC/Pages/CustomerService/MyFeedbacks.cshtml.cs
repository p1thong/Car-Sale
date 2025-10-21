using ASM1.Service.Dtos;
using ASM1.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ASM1.WebMVC.Pages.CustomerService
{
    public class MyFeedbacksModel : BasePageModel
    {
        private readonly ICustomerRelationshipService _customerService;

        public MyFeedbacksModel(ICustomerRelationshipService customerService)
        {
            _customerService = customerService;
        }

        public List<FeedbackDto> Feedbacks { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            Console.WriteLine("===== MyFeedbacks.OnGetAsync CALLED =====");
            
            // Get current customer by email
            var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
            Console.WriteLine($"MyFeedbacks - Email: {email}");
            
            if (string.IsNullOrEmpty(email))
            {
                TempData["ErrorMessage"] = "Vui lòng đăng nhập.";
                return RedirectToPage("/Auth/Login");
            }

            try
            {
                var customer = await _customerService.GetCustomerByEmailAsync(email);
                Console.WriteLine($"MyFeedbacks - Customer ID: {customer?.CustomerId}");
                
                if (customer == null)
                {
                    TempData["ErrorMessage"] = "Không tìm thấy thông tin khách hàng.";
                    return RedirectToPage("/Home/Index");
                }

                Feedbacks = (
                    await _customerService.GetCustomerFeedbacksAsync(customer.CustomerId)
                ).ToList();
                
                Console.WriteLine($"MyFeedbacks loaded: {Feedbacks.Count} feedbacks for customer {customer.CustomerId}");
                return Page();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"MyFeedbacks ERROR: {ex.Message}");
                TempData["ErrorMessage"] = $"Lỗi: {ex.Message}";
                return Page();
            }
        }
    }
}
