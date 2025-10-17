// Chuyển đổi từ: ProductController.CreateVehicleModel (GET + POST)
using System.ComponentModel.DataAnnotations;
using ASM1.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ASM1.WebMVC.Pages.Product
{
    public class CreateVehicleModelModel : BasePageModel
    {
        private readonly IVehicleService _vehicleService;

        public CreateVehicleModelModel(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        [BindProperty]
        [Required]
        public string Name { get; set; } = string.Empty;

        [BindProperty]
        [Required]
        public int ManufacturerId { get; set; }

        [BindProperty]
        [Required]
        public string Category { get; set; } = string.Empty;

        [BindProperty]
        public string? ImageUrl { get; set; }

        public List<SelectListItem> Manufacturers { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            await LoadManufacturersAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadManufacturersAsync();
                return Page();
            }

            try
            {
                var vehicleModel = new VehicleModel
                {
                    VehicleModelId = 0,
                    Name = Name,
                    ManufacturerId = ManufacturerId,
                    Category = Category,
                    ImageUrl = ImageUrl,
                };

                await _vehicleService.CreateVehicleModelAsync(vehicleModel);
                TempData["SuccessMessage"] = "Vehicle model created successfully!";
                return RedirectToPage("/Product/VehicleModels");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error: {ex.Message}");
                await LoadManufacturersAsync();
                return Page();
            }
        }

        private async Task LoadManufacturersAsync()
        {
            var manufacturers = await _vehicleService.GetAllManufacturersAsync();

            if (!manufacturers.Any())
            {
                var toyota = new Manufacturer
                {
                    ManufacturerId = 0,
                    Name = "Toyota",
                    Country = "Japan",
                };
                var bmw = new Manufacturer
                {
                    ManufacturerId = 0,
                    Name = "BMW",
                    Country = "Germany",
                };
                await _vehicleService.CreateManufacturerAsync(toyota);
                await _vehicleService.CreateManufacturerAsync(bmw);
                manufacturers = await _vehicleService.GetAllManufacturersAsync();
            }

            Manufacturers = manufacturers
                .Select(m => new SelectListItem
                {
                    Value = m.ManufacturerId.ToString(),
                    Text = m.Name,
                })
                .ToList();
        }
    }
}
