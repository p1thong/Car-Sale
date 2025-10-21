using ASM1.Service.Dtos;
using ASM1.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASM1.WebMVC.Pages.Product
{
    public class VehicleModelDetailModel : BasePageModel
    {
        private readonly IVehicleService _vehicleService;

        public VehicleModelDetailModel(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        public VehicleModelDto? VehicleModelData { get; set; }
        public List<VehicleVariantDto> Variants { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            VehicleModelData = await _vehicleService.GetVehicleModelByIdAsync(id);

            if (VehicleModelData == null)
            {
                TempData["ErrorMessage"] = "Vehicle model not found.";
                return RedirectToPage("/Product/VehicleModels");
            }

            var allVariants = await _vehicleService.GetAllVehicleVariantsAsync();
            Variants = allVariants.Where(v => v.VehicleModelId == id).ToList();
            return Page();
        }
    }
}
