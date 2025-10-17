// Chuyển đổi từ: CustomerServiceController.Feedbacks (simplified MyFeedbacks)
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

        public List<Feedback> Feedbacks { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            // Get current customer by email
            var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(email))
            {
                TempData["ErrorMessage"] = "Vui lòng đăng nhập.";
                return RedirectToPage("/Auth/Login");
            }

            try
            {
                var customer = await _customerService.GetCustomerByEmailAsync(email);
                if (customer == null)
                {
                    TempData["ErrorMessage"] = "Không tìm thấy thông tin khách hàng.";
                    return RedirectToPage("/Home/Index");
                }

                Feedbacks = (
                    await _customerService.GetCustomerFeedbacksAsync(customer.CustomerId)
                ).ToList();
                return Page();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Lỗi: {ex.Message}";
                return Page();
            }
        }
    }
}
