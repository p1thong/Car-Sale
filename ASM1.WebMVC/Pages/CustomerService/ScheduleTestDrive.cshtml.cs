// Chuyển đổi từ: CustomerServiceController.ScheduleTestDrive (GET + POST) - Simplified
using System.ComponentModel.DataAnnotations;
using ASM1.Service.Services.Interfaces;
using ASM1.WebMVC.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;

namespace ASM1.WebMVC.Pages.CustomerService
{
    public class ScheduleTestDriveModel : BasePageModel
    {
        private readonly ICustomerRelationshipService _customerService;
        private readonly IVehicleService _vehicleService;
        private readonly IHubContext<HubServer> _hubContext;

        public ScheduleTestDriveModel(
            ICustomerRelationshipService customerService,
            IVehicleService vehicleService,
            IHubContext<HubServer> hubContext
        )
        {
            _customerService = customerService;
            _vehicleService = vehicleService;
            _hubContext = hubContext;
        }

        public List<SelectListItem> VehicleVariants { get; set; } = new();

        [BindProperty]
        [Required]
        public int VariantId { get; set; }

        [BindProperty]
        [Required]
        public DateOnly ScheduledDate { get; set; } =
            DateOnly.FromDateTime(DateTime.Now.AddDays(1));

        [BindProperty]
        [Required]
        public string ScheduledTimeString { get; set; } = "09:00";

        public TimeOnly ScheduledTime => TimeOnly.Parse(ScheduledTimeString);

        public async Task<IActionResult> OnGetAsync(int? variantId)
        {
            // Kiểm tra authentication
            if (User.Identity?.IsAuthenticated != true)
            {
                TempData["ErrorMessage"] = "Vui lòng đăng nhập để đặt lịch lái thử.";
                return RedirectToPage("/Auth/Login");
            }

            // Lấy email từ claims
            var userEmail = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(userEmail))
            {
                TempData["ErrorMessage"] =
                    "Không tìm thấy thông tin email. Vui lòng đăng nhập lại.";
                return RedirectToPage("/Auth/Login");
            }

            // Tìm customer bằng email
            var customer = await _customerService.GetCustomerByEmailAsync(userEmail);
            if (customer == null)
            {
                TempData["ErrorMessage"] = "Bạn cần là khách hàng để đặt lịch lái thử.";
                return RedirectToPage("/Home/Index");
            }

            if (variantId.HasValue)
            {
                VariantId = variantId.Value;
            }

            await LoadVariantsAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Kiểm tra authentication
            if (User.Identity?.IsAuthenticated != true)
            {
                TempData["ErrorMessage"] = "Vui lòng đăng nhập.";
                return RedirectToPage("/Auth/Login");
            }

            // Lấy email từ claims
            var userEmail = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(userEmail))
            {
                TempData["ErrorMessage"] =
                    "Không tìm thấy thông tin email. Vui lòng đăng nhập lại.";
                return RedirectToPage("/Auth/Login");
            }

            // Tìm customer bằng email
            var customer = await _customerService.GetCustomerByEmailAsync(userEmail);
            if (customer == null)
            {
                TempData["ErrorMessage"] = "Bạn cần là khách hàng để đặt lịch lái thử.";
                return RedirectToPage("/Home/Index");
            }

            if (!ModelState.IsValid)
            {
                await LoadVariantsAsync();
                return Page();
            }

            try
            {
                // Validation 1: Check if variant is booked at this time by another customer
                var existingTestDrives = await _customerService.GetTestDriveScheduleAsync(
                    ScheduledDate
                );

                var isVariantBookedByOther = existingTestDrives.Any(td =>
                    td.VariantId == VariantId
                    && td.ScheduledTime == ScheduledTime
                    && td.CustomerId != customer.CustomerId
                    && (td.Status == "Scheduled" || td.Status == "Confirmed")
                );

                if (isVariantBookedByOther)
                {
                    ModelState.AddModelError(
                        string.Empty,
                        "Xe này đã được khách hàng khác đặt vào khung giờ bạn chọn. Vui lòng chọn giờ khác."
                    );
                    await LoadVariantsAsync();
                    return Page();
                }

                // Validation 2: Check if customer has another test drive at the same time
                var hasOtherVariantSameTime = existingTestDrives.Any(td =>
                    td.CustomerId == customer.CustomerId
                    && td.VariantId != VariantId
                    && td.ScheduledTime == ScheduledTime
                    && (td.Status == "Scheduled" || td.Status == "Confirmed")
                );

                if (hasOtherVariantSameTime)
                {
                    ModelState.AddModelError(
                        string.Empty,
                        "Bạn đã có lịch lái thử một xe khác trong cùng khung giờ. Vui lòng chọn giờ khác."
                    );
                    await LoadVariantsAsync();
                    return Page();
                }

                // Schedule test drive với customerId từ Customer record
                await _customerService.ScheduleTestDriveAsync(
                    customer.CustomerId,
                    VariantId,
                    ScheduledDate,
                    ScheduledTime
                );
                await _hubContext.Clients.All.SendAsync("ScheduleTestDrives");
                TempData["SuccessMessage"] = "Đặt lịch lái thử thành công!";
                return RedirectToPage("/CustomerService/MyTestDrives");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Lỗi: {ex.Message}");
                await LoadVariantsAsync();
                return Page();
            }
        }

        private async Task LoadVariantsAsync()
        {
            var variants = await _vehicleService.GetAvailableVariantsAsync();
            VehicleVariants = variants
                .Select(v => new SelectListItem
                {
                    Value = v.VariantId.ToString(),
                    Text =
                        $"{v.VehicleModel?.Manufacturer?.Name} {v.VehicleModel?.Name} - {v.Version} ({v.Color})",
                })
                .ToList();
        }
    }
}
