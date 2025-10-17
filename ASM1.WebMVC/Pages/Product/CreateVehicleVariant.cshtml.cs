// Chuyển đổi từ: ProductController.CreateVehicleVariant (GET + POST)
using ASM1.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ASM1.WebMVC.Pages.Product
{
    public class CreateVehicleVariantModel : BasePageModel
    {
        private readonly IVehicleService _vehicleService;

        public CreateVehicleVariantModel(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

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

        public async Task<IActionResult> OnGetAsync(int? modelId)
        {
            await LoadVehicleModelsAsync();
            if (modelId.HasValue)
            {
                VehicleModelId = modelId.Value;
            }
            ProductYear = DateTime.Now.Year;
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
                    VariantId = 0,
                    VehicleModelId = VehicleModelId,
                    Version = Version,
                    Color = Color,
                    ProductYear = ProductYear,
                    Price = Price,
                    Quantity = Quantity
                };

                await _vehicleService.CreateVehicleVariantAsync(variant);
                TempData["SuccessMessage"] = "Vehicle variant created successfully!";
                return RedirectToPage("/Product/VehicleVariants", new { modelId = VehicleModelId });
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
            VehicleModels = models.Select(m => new SelectListItem
            {
                Value = m.VehicleModelId.ToString(),
                Text = $"{m.Manufacturer?.Name} {m.Name}"
            }).ToList();
        }
    }
}
