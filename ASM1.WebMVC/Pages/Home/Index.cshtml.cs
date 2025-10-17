using ASM1.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ASM1.WebMVC.Pages.Home
{
    public class IndexModel : BasePageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IVehicleService _vehicleService;

        public IndexModel(ILogger<IndexModel> logger, IVehicleService vehicleService)
        {
            _logger = logger;
            _vehicleService = vehicleService;
        }

        // Property để bind data từ code-behind sang view
        public IEnumerable<VehicleVariant> Vehicles { get; set; } = new List<VehicleVariant>();

        // GET handler - tương đương với action Index() trong controller
        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                // Lấy danh sách tất cả vehicle variants với thông tin liên quan
                Vehicles = await _vehicleService.GetAllVehicleVariantsAsync();
                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading vehicles for homepage");
                TempData["ErrorMessage"] =
                    "Có lỗi xảy ra khi tải danh sách xe. Vui lòng thử lại sau.";
                Vehicles = new List<VehicleVariant>();
                return Page();
            }
        }
    }
}
