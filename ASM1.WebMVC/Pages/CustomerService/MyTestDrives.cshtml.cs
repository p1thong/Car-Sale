using ASM1.Service.Dtos;
using ASM1.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ASM1.WebMVC.Pages.CustomerService
{
    public class MyTestDrivesModel : BasePageModel
    {
        private readonly ICustomerRelationshipService _customerService;

        public MyTestDrivesModel(ICustomerRelationshipService customerService)
        {
            _customerService = customerService;
        }

        public List<TestDriveDto> TestDrives { get; set; } = new();
        public string? CustomerName { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            // Lấy email từ claims
            var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(email))
            {
                TempData["ErrorMessage"] = "Không tìm thấy thông tin đăng nhập. Vui lòng đăng nhập lại.";
                return RedirectToPage("/Auth/Login");
            }

            try
            {
                // Tìm Customer bằng email
                var customer = await _customerService.GetCustomerByEmailAsync(email);
                if (customer == null)
                {
                    TempData["ErrorMessage"] = $"Không tìm thấy thông tin khách hàng với email {email}. Vui lòng liên hệ quản trị viên.";
                    return Page();
                }

                CustomerName = customer.FullName;

                // Lấy test drives của customer
                TestDrives = (await _customerService.GetCustomerTestDrivesAsync(customer.CustomerId)).ToList();
                
                if (!TestDrives.Any())
                {
                    TempData["InfoMessage"] = "Bạn chưa có lịch lái thử nào.";
                }

                return Page();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Lỗi khi tải lịch lái thử: {ex.Message}";
                return Page();
            }
        }
    }
}
