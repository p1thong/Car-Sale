using ASM1.Service.Dtos;
using ASM1.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

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
        public VehicleVariantDto Vehicle { get; set; } = null!;

        // GET handler với route parameter id
        // Tương đương: public async Task<IActionResult> VehicleDetail(int id)
        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                var vehicle = await _vehicleService.GetVehicleVariantByIdAsync(id);
                if (vehicle == null)
                {
                    TempData["ErrorMessage"] = "Không tìm thấy thông tin xe.";
                    return RedirectToPage("/Home/Index");
                }
                Vehicle = vehicle;
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
