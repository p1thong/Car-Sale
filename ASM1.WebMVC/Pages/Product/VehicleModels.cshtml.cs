using ASM1.Service.Services.Interfaces;

namespace ASM1.WebMVC.Pages.Product
{
    public class VehicleModelsModel : BasePageModel
    {
        private readonly IVehicleService _vehicleService;

        public VehicleModelsModel(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        public IEnumerable<VehicleModel> Models { get; set; } = new List<VehicleModel>();

        public async Task OnGetAsync()
        {
            Models = await _vehicleService.GetAllVehicleModelsAsync();
        }
    }
}
