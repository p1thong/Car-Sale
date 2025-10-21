using ASM1.Service.Dtos;
using ASM1.Service.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ASM1.WebMVC.Pages.Product
{
    public class VehicleModelsModel : BasePageModel
    {
        private readonly IVehicleService _vehicleService;

        public VehicleModelsModel(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        public IEnumerable<VehicleModelDto> Models { get; set; } = new List<VehicleModelDto>();

        public async Task OnGetAsync()
        {
            Models = await _vehicleService.GetAllVehicleModelsAsync();
        }
    }
}
