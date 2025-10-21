using ASM1.Service.Dtos;
using ASM1.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASM1.WebMVC.Pages.Product
{
    public class VehicleVariantsModel : BasePageModel
    {
        private readonly IVehicleService _vehicleService;

        public VehicleVariantsModel(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        public List<VehicleVariantDto> Variants { get; set; } = new();
        public List<SelectListItem> VehicleModels { get; set; } = new();
        public int? SelectedModelId { get; set; }

        public async Task<IActionResult> OnGetAsync(int? modelId)
        {
            SelectedModelId = modelId;

            // Load all models for filter dropdown
            var allModels = await _vehicleService.GetAllVehicleModelsAsync();
            VehicleModels = allModels
                .Select(m => new SelectListItem
                {
                    Value = m.VehicleModelId.ToString(),
                    Text = $"{m.ManufacturerName} {m.Name}",
                    Selected = m.VehicleModelId == modelId,
                })
                .ToList();

            // Load variants
            var allVariants = await _vehicleService.GetAllVehicleVariantsAsync();

            if (modelId.HasValue)
            {
                Variants = allVariants.Where(v => v.VehicleModelId == modelId.Value).ToList();
            }
            else
            {
                Variants = allVariants.ToList();
            }

            return Page();
        }
    }
}
