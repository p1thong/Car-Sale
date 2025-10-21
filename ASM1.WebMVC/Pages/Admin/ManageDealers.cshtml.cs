using ASM1.Service.Dtos;
using ASM1.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASM1.WebMVC.Pages.Admin
{
    public class ManageDealersModel : BasePageModel
    {
        private readonly IDealerService _dealerService;

        public ManageDealersModel(IDealerService dealerService)
        {
            _dealerService = dealerService;
        }

        public List<DealerDto> Dealers { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            if (!IsInRole("Admin"))
            {
                TempData["ErrorMessage"] = "Access denied.";
                return RedirectToPage("/Auth/Login");
            }

            Dealers = (await _dealerService.GetAllDealersAsync()).ToList();
            return Page();
        }
    }
}
