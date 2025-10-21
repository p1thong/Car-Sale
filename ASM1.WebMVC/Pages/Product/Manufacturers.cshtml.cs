using ASM1.Service.Dtos;
using ASM1.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASM1.WebMVC.Pages.Product
{
    public class ManufacturersModel : BasePageModel
    {
        private readonly IVehicleService _vehicleService;

        public ManufacturersModel(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        public List<ManufacturerDto> Manufacturers { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            Manufacturers = (await _vehicleService.GetAllManufacturersAsync()).ToList();
            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            try
            {
                await _vehicleService.DeleteManufacturerAsync(id);
                TempData["SuccessMessage"] = "Manufacturer deleted successfully!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error: {ex.Message}";
            }
            return RedirectToPage();
        }
    }
}
