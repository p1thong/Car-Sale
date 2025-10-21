using System.ComponentModel.DataAnnotations;
using ASM1.Service.Dtos;
using ASM1.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace ASM1.WebMVC.Pages.Product
{
    public class CreateManufacturerModel : BasePageModel
    {
        private readonly IVehicleService _vehicleService;

        public CreateManufacturerModel(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

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

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var manufacturer = new ManufacturerDto
                {
                    ManufacturerId = 0,
                    Name = Name,
                    Country = Country,
                    Address = Address ?? string.Empty,
                };

                await _vehicleService.CreateManufacturerAsync(manufacturer);
                TempData["SuccessMessage"] = "Manufacturer created successfully!";
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
