// Chuyển đổi từ: AdminController.ManageUsers
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ASM1.WebMVC.Pages.Admin
{
    public class ManageUsersModel : BasePageModel
    {
        public async Task<IActionResult> OnGetAsync()
        {
            if (!IsInRole("Admin"))
            {
                TempData["ErrorMessage"] = "Access denied.";
                return RedirectToPage("/Auth/Login");
            }

            // TODO: Load users when IAuthService has user management methods
            return Page();
        }
    }
}
