// Chuyển đổi từ: HomeController.VehicleDetail action
// File gốc: Controllers/HomeController.cs
// Mô tả: Hiển thị chi tiết thông tin của một xe cụ thể

using ASM1.Repository.Models;
using ASM1.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ASM1.WebMVC.Pages.Home
{
    public class VehicleDetailModel : BasePageModel
    {
        private readonly ILogger<VehicleDetailModel> _logger;
        private readonly IVehicleService _vehicleService;

        public VehicleDetailModel(ILogger<VehicleDetailModel> logger, IVehicleService vehicleService)
        {
            _logger = logger;
            _vehicleService = vehicleService;
        }

        // Property để bind data từ code-behind sang view
        public VehicleVariant Vehicle { get; set; } = null!;

        // GET handler với route parameter id
        // Tương đương: public async Task<IActionResult> VehicleDetail(int id)
        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                Vehicle = await _vehicleService.GetVehicleVariantByIdAsync(id);
                if (Vehicle == null)
                {
                    TempData["ErrorMessage"] = "Không tìm thấy thông tin xe.";
                    return RedirectToPage("/Home/Index");
                }
                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading vehicle detail for ID: {VehicleId}", id);
                TempData["ErrorMessage"] =
                    "Có lỗi xảy ra khi tải thông tin xe. Vui lòng thử lại sau.";
                return RedirectToPage("/Home/Index");
            }
        }
    }
}
