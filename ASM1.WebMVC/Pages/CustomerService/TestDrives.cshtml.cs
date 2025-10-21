using ASM1.Service.Dtos;
using ASM1.Service.Services.Interfaces;
using ASM1.WebMVC.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace ASM1.WebMVC.Pages.CustomerService
{
    public class TestDrivesModel : BasePageModel
    {
        private readonly ICustomerRelationshipService _customerService;
        private readonly IHubContext<HubServer> _hubContext;

        public TestDrivesModel(
            ICustomerRelationshipService customerService,
            IHubContext<HubServer> hubContext
        )
        {
            _customerService = customerService;
            _hubContext = hubContext;
        }

        public IEnumerable<TestDriveDto> TestDrives { get; set; } = new List<TestDriveDto>();
        public DateOnly? SelectedDate { get; set; }

        public async Task<IActionResult> OnGetAsync(DateTime? date = null)
        {
            try
            {
                var userRole = HttpContext.Session.GetString("UserRole");

                // Nếu là Customer, chuyển hướng đến MyTestDrives
                if (string.Equals(userRole, "Customer", StringComparison.OrdinalIgnoreCase))
                {
                    return RedirectToPage("./MyTestDrives");
                }

                // Nếu có tham số date, hiển thị theo ngày đó (chỉ dành cho Dealer Staff)
                if (date.HasValue)
                {
                    SelectedDate = DateOnly.FromDateTime(date.Value);
                    TestDrives = await _customerService.GetTestDriveScheduleAsync(
                        SelectedDate.Value
                    );
                }
                else
                {
                    // Hiển thị tất cả test drives gần đây
                    TestDrives = await _customerService.GetAllTestDrivesAsync();
                    SelectedDate = DateOnly.FromDateTime(DateTime.Today);
                }

                return Page();
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Lỗi khi tải danh sách lịch lái thử: {ex.Message}";
                return Page();
            }
        }

        public async Task<IActionResult> OnPostConfirmAsync(int testDriveId)
        {
            try
            {
                await _hubContext.Clients.All.SendAsync("UpdateTestDriveStatusForCustomer");
                await _hubContext.Clients.All.SendAsync("UpdateTestDriveStatusForDealer");
                await _customerService.ConfirmTestDriveAsync(testDriveId);
                TempData["Success"] = "Đã xác nhận lịch lái thử!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Lỗi khi xác nhận: {ex.Message}";
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostCompleteAsync(int testDriveId)
        {
            try
            {
                await _hubContext.Clients.All.SendAsync("UpdateTestDriveStatusForCustomer");
                await _hubContext.Clients.All.SendAsync("UpdateTestDriveStatusForDealer");
                await _customerService.CompleteTestDriveAsync(testDriveId);
                TempData["Success"] = "Đã hoàn thành lịch lái thử!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Lỗi khi hoàn thành: {ex.Message}";
            }

            return RedirectToPage();
        }
    }
}
