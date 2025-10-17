// Chuyển đổi từ: AdminController.SystemReports
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ASM1.WebMVC.Pages.Admin
{
    public class SystemReportsModel : BasePageModel
    {
        public async Task<IActionResult> OnGetAsync()
        {
            if (!IsInRole("Admin"))
            {
                TempData["ErrorMessage"] = "Access denied.";
                return RedirectToPage("/Auth/Login");
            }

            return Page();
        }
    }
}
