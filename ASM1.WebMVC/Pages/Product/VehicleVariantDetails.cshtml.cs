// Chuyển đổi từ: ProductController.VehicleVariantDetails
using ASM1.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ASM1.WebMVC.Pages.Product
{
    public class VehicleVariantDetailsModel : BasePageModel
    {
        private readonly IVehicleService _vehicleService;

        public VehicleVariantDetailsModel(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        public VehicleVariant? Variant { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Variant = await _vehicleService.GetVehicleVariantByIdAsync(id);

            if (Variant == null)
            {
                TempData["ErrorMessage"] = "Variant not found.";
                return RedirectToPage("/Product/VehicleVariants");
            }

            return Page();
        }
    }
}
