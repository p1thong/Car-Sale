using ASM1.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ASM1.WebMVC.Pages.Product
{
    public class EditVehicleModelModel : BasePageModel
    {
        private readonly IVehicleService _vehicleService;

        public EditVehicleModelModel(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        [BindProperty]
        public int VehicleModelId { get; set; }

        [BindProperty]
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [BindProperty]
        [Required]
        public int ManufacturerId { get; set; }

        [BindProperty]
        [Required]
        [StringLength(50)]
        public string Category { get; set; } = string.Empty;

        [BindProperty]
        [Url]
        public string? ImageUrl { get; set; }

        public List<SelectListItem> Manufacturers { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var model = await _vehicleService.GetVehicleModelByIdAsync(id);
            if (model == null)
            {
                TempData["ErrorMessage"] = "Vehicle model not found.";
                return RedirectToPage("/Product/VehicleModels");
            }

            VehicleModelId = model.VehicleModelId;
            Name = model.Name;
            ManufacturerId = model.ManufacturerId;
            Category = model.Category;
            ImageUrl = model.ImageUrl;

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
                var model = new VehicleModel
                {
                    VehicleModelId = VehicleModelId,
                    Name = Name,
                    ManufacturerId = ManufacturerId,
                    Category = Category,
                    ImageUrl = ImageUrl
                };

                await _vehicleService.UpdateVehicleModelAsync(model);
                TempData["SuccessMessage"] = "Vehicle model updated successfully!";
                return RedirectToPage("/Product/VehicleModelDetail", new { id = VehicleModelId });
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
            Manufacturers = manufacturers.Select(m => new SelectListItem
            {
                Value = m.ManufacturerId.ToString(),
                Text = m.Name
            }).ToList();
        }
    }
}
