using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace ASM1.WebMVC.Pages.Auth
{
    public class LogoutModel : BasePageModel
    {
        // GET handler - tương đương [HttpGet("Logout")]
        // Razor Pages không cần action riêng, OnGet tự động xử lý
        public async Task<IActionResult> OnGet()
        {
            var userName = HttpContext.Session.GetString("UserName") ?? User.Identity?.Name;

            // Đăng xuất cookie authentication
            await HttpContext.SignOutAsync("CarSalesCookies");

            // Xóa session
            HttpContext.Session.Clear();

            TempData["InfoMessage"] = $"Đã đăng xuất thành công! Hẹn gặp lại {userName}.";
            return RedirectToPage("/Auth/Login");
        }
    }
}
