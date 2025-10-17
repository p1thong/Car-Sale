using System.ComponentModel.DataAnnotations;
using ASM1.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ASM1.WebMVC.Pages.Product
{
    public class EditVehicleVariantModel : BasePageModel
    {
        private readonly IVehicleService _vehicleService;

        public EditVehicleVariantModel(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        [BindProperty]
        public int VariantId { get; set; }

        [BindProperty]
        [Required]
        public int VehicleModelId { get; set; }

        [BindProperty]
        [Required]
        public string Version { get; set; } = string.Empty;

        [BindProperty]
        [Required]
        public string Color { get; set; } = string.Empty;

        [BindProperty]
        [Required]
        [Range(1900, 2100)]
        public int ProductYear { get; set; }

        [BindProperty]
        [Required]
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        [BindProperty]
        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }

        public List<SelectListItem> VehicleModels { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var variant = await _vehicleService.GetVehicleVariantByIdAsync(id);
            if (variant == null)
            {
                TempData["ErrorMessage"] = "Variant not found.";
                return RedirectToPage("/Product/VehicleVariants");
            }

            VariantId = variant.VariantId;
            VehicleModelId = variant.VehicleModelId;
            Version = variant.Version ?? string.Empty;
            Color = variant.Color ?? string.Empty;
            ProductYear = variant.ProductYear ?? DateTime.Now.Year;
            Price = variant.Price ?? 0;
            Quantity = variant.Quantity;

            await LoadVehicleModelsAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadVehicleModelsAsync();
                return Page();
            }

            try
            {
                var variant = new VehicleVariant
                {
                    VariantId = VariantId,
                    VehicleModelId = VehicleModelId,
                    Version = Version,
                    Color = Color,
                    ProductYear = ProductYear,
                    Price = Price,
                    Quantity = Quantity,
                };

                await _vehicleService.UpdateVehicleVariantAsync(variant);
                TempData["SuccessMessage"] = "Vehicle variant updated successfully!";
                return RedirectToPage("/Product/VehicleVariantDetails", new { id = VariantId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error: {ex.Message}");
                await LoadVehicleModelsAsync();
                return Page();
            }
        }

        private async Task LoadVehicleModelsAsync()
        {
            var models = await _vehicleService.GetAllVehicleModelsAsync();
            VehicleModels = models
                .Select(m => new SelectListItem
                {
                    Value = m.VehicleModelId.ToString(),
                    Text = $"{m.Manufacturer?.Name} {m.Name}",
                })
                .ToList();
        }
    }
}
