using ASM1.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ASM1.WebMVC.Pages.Admin
{
    public class ManageDealersModel : BasePageModel
    {
        private readonly IDealerService _dealerService;

        public ManageDealersModel(IDealerService dealerService)
        {
            _dealerService = dealerService;
        }

        public List<ASM1.Repository.Models.Dealer> Dealers { get; set; } = new();

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
