// Chuyển đổi từ: DealerController.Dashboard (placeholder)
using Microsoft.AspNetCore.Mvc;

namespace ASM1.WebMVC.Pages.Dealer
{
    public class DashboardModel : BasePageModel
    {
        public IActionResult OnGet()
        {
            var dealerId = GetCurrentDealerId();
            if (dealerId == null)
            {
                TempData["ErrorMessage"] = "Dealer access required.";
                return RedirectToPage("/Auth/Login");
            }

            return Page();
        }
    }
}
