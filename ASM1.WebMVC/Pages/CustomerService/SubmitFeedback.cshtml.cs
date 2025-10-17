using ASM1.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ASM1.WebMVC.Pages.CustomerService
{
    public class SubmitFeedbackModel : BasePageModel
    {
        private readonly ICustomerRelationshipService _customerService;

        public SubmitFeedbackModel(ICustomerRelationshipService customerService)
        {
            _customerService = customerService;
        }

        public TestDrive? TestDrive { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Vui lòng nhập nội dung đánh giá")]
        public new string Content { get; set; } = string.Empty;

        [BindProperty]
        [Required(ErrorMessage = "Vui lòng chọn số sao")]
        [Range(1, 5, ErrorMessage = "Đánh giá phải từ 1 đến 5 sao")]
        public int Rating { get; set; }

        public async Task<IActionResult> OnGetAsync(int testDriveId)
        {
            try
            {
                // Get test drive details
                TestDrive = await _customerService.GetTestDriveByIdAsync(testDriveId);
                if (TestDrive == null)
                {
                    TempData["Error"] = "Không tìm thấy thông tin lịch lái thử.";
                    return RedirectToPage("./MyTestDrives");
                }

                // Check if test drive is completed
                if (TestDrive.Status != "Completed")
                {
                    TempData["Error"] = "Chỉ có thể gửi phản hồi cho lịch lái thử đã hoàn thành.";
                    return RedirectToPage("./MyTestDrives");
                }

                // Get current customer by email
                var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
                if (string.IsNullOrEmpty(email))
                {
                    TempData["Error"] = "Không tìm thấy thông tin email. Vui lòng đăng nhập lại.";
                    return RedirectToPage("/Auth/Login");
                }

                var currentCustomer = await _customerService.GetCustomerByEmailAsync(email);
                if (currentCustomer == null)
                {
                    TempData["Error"] = "Không tìm thấy thông tin khách hàng.";
                    return RedirectToPage("./MyTestDrives");
                }

                // Verify this test drive belongs to current customer
                if (TestDrive.CustomerId != currentCustomer.CustomerId)
                {
                    TempData["Error"] = "Bạn chỉ có thể gửi phản hồi cho lịch lái thử của chính mình.";
                    return RedirectToPage("./MyTestDrives");
                }

                return Page();
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Lỗi khi tải trang phản hồi: {ex.Message}";
                return RedirectToPage("./MyTestDrives");
            }
        }

        public async Task<IActionResult> OnPostAsync(int testDriveId)
        {
            // Get current customer by email
            var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(email))
            {
                TempData["Error"] = "Vui lòng đăng nhập.";
                return RedirectToPage("/Auth/Login");
            }

            var currentCustomer = await _customerService.GetCustomerByEmailAsync(email);
            if (currentCustomer == null)
            {
                TempData["Error"] = "Không tìm thấy thông tin khách hàng.";
                return RedirectToPage("/Auth/Login");
            }

            if (!ModelState.IsValid)
            {
                // Reload test drive info
                TestDrive = await _customerService.GetTestDriveByIdAsync(testDriveId);
                return Page();
            }

            try
            {
                // Verify test drive
                var testDrive = await _customerService.GetTestDriveByIdAsync(testDriveId);
                if (testDrive == null || testDrive.Status != "Completed")
                {
                    TempData["Error"] = "Không thể gửi phản hồi cho lịch lái thử này.";
                    return RedirectToPage("./MyTestDrives");
                }

                // Verify customer - use Customer.CustomerId instead of UserId
                if (testDrive.CustomerId != currentCustomer.CustomerId)
                {
                    TempData["Error"] = "Bạn chỉ có thể gửi phản hồi cho lịch lái thử của chính mình.";
                    return RedirectToPage("./MyTestDrives");
                }

                // Create feedback with Customer.CustomerId
                await _customerService.CreateFeedbackAsync(currentCustomer.CustomerId, Content, Rating);

                TempData["Success"] = "Gửi đánh giá thành công! Cảm ơn bạn đã đánh giá.";
                return RedirectToPage("./MyFeedbacks");
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Lỗi khi gửi đánh giá: {ex.Message}";
                TestDrive = await _customerService.GetTestDriveByIdAsync(testDriveId);
                return Page();
            }
        }
    }
}
