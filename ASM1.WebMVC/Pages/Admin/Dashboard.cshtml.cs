// Chuyển đổi từ: AdminController.Dashboard
using ASM1.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ASM1.WebMVC.Pages.Admin
{
    public class DashboardModel : BasePageModel
    {
        private readonly ILogger<DashboardModel> _logger;
        private readonly IVehicleService _vehicleService;
        private readonly IDealerService _dealerService;

        public DashboardModel(
            ILogger<DashboardModel> logger,
            IVehicleService vehicleService,
            IDealerService dealerService)
        {
            _logger = logger;
            _vehicleService = vehicleService;
            _dealerService = dealerService;
        }

        public int TotalManufacturers { get; set; }
        public int TotalDealers { get; set; }
        public int TotalVehicleModels { get; set; }
        public int TotalUsers { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            // Check admin role
            if (!IsInRole("Admin"))
            {
                TempData["ErrorMessage"] = "Access denied. Admin role required.";
                return RedirectToPage("/Auth/Login");
            }

            try
            {
                var manufacturers = await _vehicleService.GetAllManufacturersAsync();
                var dealers = await _dealerService.GetAllDealersAsync();
                var vehicleModels = await _vehicleService.GetAllVehicleModelsAsync();

                TotalManufacturers = manufacturers.Count();
                TotalDealers = dealers.Count();
                TotalVehicleModels = vehicleModels.Count();
                TotalUsers = 0; // TODO: Add user service

                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading admin dashboard data");
                TempData["ErrorMessage"] = "Error loading dashboard statistics.";
                return Page();
            }
        }
    }
}
