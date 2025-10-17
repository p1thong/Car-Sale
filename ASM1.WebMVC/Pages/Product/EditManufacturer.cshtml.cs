// Chuyển đổi từ: ProductController.EditManufacturer (GET + POST)
using ASM1.Repository.Models;
using ASM1.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace ASM1.WebMVC.Pages.Product
{
    public class EditManufacturerModel : BasePageModel
    {
        private readonly IVehicleService _vehicleService;

        public EditManufacturerModel(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        [BindProperty]
        public int ManufacturerId { get; set; }

        [BindProperty]
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [BindProperty]
        [Required]
        [StringLength(100)]
        public string Country { get; set; } = string.Empty;

        [BindProperty]
        [StringLength(200)]
        public string? Address { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var manufacturer = await _vehicleService.GetManufacturerByIdAsync(id);
            if (manufacturer == null)
            {
                TempData["ErrorMessage"] = "Manufacturer not found.";
                return RedirectToPage("/Product/Manufacturers");
            }

            ManufacturerId = manufacturer.ManufacturerId;
            Name = manufacturer.Name;
            Country = manufacturer.Country;
            Address = manufacturer.Address;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var manufacturer = new Manufacturer
                {
                    ManufacturerId = ManufacturerId,
                    Name = Name,
                    Country = Country,
                    Address = Address ?? string.Empty
                };

                await _vehicleService.UpdateManufacturerAsync(manufacturer);
                TempData["SuccessMessage"] = "Manufacturer updated successfully!";
                return RedirectToPage("/Product/Manufacturers");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Error: {ex.Message}");
                return Page();
            }
        }
    }
}
