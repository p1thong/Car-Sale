using ASM1.Service.Dtos;
using ASM1.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ASM1.WebMVC.Pages.CustomerService
{
    public class TestDriveDetailsModel : BasePageModel
    {
        private readonly ICustomerRelationshipService _customerService;

        public TestDriveDetailsModel(ICustomerRelationshipService customerService)
        {
            _customerService = customerService;
        }

        public TestDriveDto? TestDrive { get; set; }
        public bool CanConfirm { get; set; }
        public bool CanComplete { get; set; }
        public bool CanCancel { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                TestDrive = await _customerService.GetTestDriveByIdAsync(id);

                if (TestDrive == null)
                {
                    TempData["Error"] = "Không tìm thấy thông tin lịch lái thử.";
                    return RedirectToPage("./MyTestDrives");
                }

                // Set permissions based on role and status
                var userRole = ViewData["UserRole"]?.ToString();
                var isDealer = userRole?.Equals("Dealer", StringComparison.OrdinalIgnoreCase) == true;

                // Only Dealer can confirm and complete
                CanConfirm = isDealer && TestDrive.Status == "Scheduled";
                CanComplete = isDealer && TestDrive.Status == "Confirmed";
                
                // Both Customer and Dealer can cancel (if not completed or already cancelled)
                CanCancel = TestDrive.Status != "Completed" && TestDrive.Status != "Cancelled";

                return Page();
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Lỗi khi tải thông tin lịch lái thử: {ex.Message}";
                return RedirectToPage("./MyTestDrives");
            }
        }

        public async Task<IActionResult> OnPostConfirmAsync(int testDriveId)
        {
            try
            {
                // Check if user is Dealer
                var userRole = ViewData["UserRole"]?.ToString();
                if (!userRole?.Equals("Dealer", StringComparison.OrdinalIgnoreCase) == true)
                {
                    TempData["Error"] = "Chỉ có Dealer mới có thể xác nhận lịch lái thử.";
                    return RedirectToPage(new { id = testDriveId });
                }

                await _customerService.ConfirmTestDriveAsync(testDriveId);
                TempData["Success"] = "Đã xác nhận lịch lái thử!";
                return RedirectToPage(new { id = testDriveId });
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Lỗi khi xác nhận: {ex.Message}";
                return RedirectToPage(new { id = testDriveId });
            }
        }

        public async Task<IActionResult> OnPostCompleteAsync(int testDriveId)
        {
            try
            {
                // Check if user is Dealer
                var userRole = ViewData["UserRole"]?.ToString();
                if (!userRole?.Equals("Dealer", StringComparison.OrdinalIgnoreCase) == true)
                {
                    TempData["Error"] = "Chỉ có Dealer mới có thể hoàn thành lịch lái thử.";
                    return RedirectToPage(new { id = testDriveId });
                }

                await _customerService.CompleteTestDriveAsync(testDriveId);
                TempData["Success"] = "Đã hoàn thành lịch lái thử!";
                return RedirectToPage(new { id = testDriveId });
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Lỗi khi hoàn thành: {ex.Message}";
                return RedirectToPage(new { id = testDriveId });
            }
        }

        public async Task<IActionResult> OnPostCancelAsync(int testDriveId)
        {
            try
            {
                await _customerService.UpdateTestDriveStatusAsync(testDriveId, "Cancelled");
                TempData["Success"] = "Đã hủy lịch lái thử!";
                return RedirectToPage("./MyTestDrives");
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Lỗi khi hủy: {ex.Message}";
                return RedirectToPage(new { id = testDriveId });
            }
        }
    }
}
