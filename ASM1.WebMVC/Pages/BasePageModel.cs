// BasePageModel - Tương tự BaseController
// Mô tả: Base class cho tất cả PageModels, xử lý authentication và session logic
// Chuyển đổi từ: Controllers/BaseController.cs

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ASM1.WebMVC.Pages
{
    public class BasePageModel : PageModel
    {
        // Override OnPageHandlerExecuting để kiểm tra authentication trước khi handler chạy
        // Tương đương với OnActionExecuting trong BaseController
        public override void OnPageHandlerExecuting(PageHandlerExecutingContext context)
        {
            // Kiểm tra cookie authentication trước
            if (User.Identity?.IsAuthenticated == true)
            {
                // Lấy thông tin từ claims
                var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                var userRole = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;
                var userName = User.FindFirst(System.Security.Claims.ClaimTypes.Name)?.Value;

                // Đồng bộ với session nếu session rỗng
                if (
                    string.IsNullOrEmpty(HttpContext.Session.GetString("UserId"))
                    && !string.IsNullOrEmpty(userId)
                )
                {
                    HttpContext.Session.SetString("UserId", userId);
                    HttpContext.Session.SetString("UserRole", userRole ?? "Guest");
                    HttpContext.Session.SetString("UserName", userName ?? "Unknown");
                }

                // Set ViewData (tương tự ViewBag trong BaseController)
                ViewData["IsLoggedIn"] = true;
                ViewData["UserId"] = userId;
                ViewData["UserRole"] = userRole;
                ViewData["UserName"] = userName;
            }
            else
            {
                // Kiểm tra session (fallback)
                var userId = HttpContext.Session.GetString("UserId");
                var userRole = HttpContext.Session.GetString("UserRole");
                var userName = HttpContext.Session.GetString("UserName");

                if (!string.IsNullOrEmpty(userId))
                {
                    ViewData["IsLoggedIn"] = true;
                    ViewData["UserId"] = userId;
                    ViewData["UserRole"] = userRole;
                    ViewData["UserName"] = userName;
                }
                else
                {
                    ViewData["IsLoggedIn"] = false;

                    // Optional: Redirect to login if trying to access protected pages
                    // Có thể uncomment nếu muốn bắt buộc đăng nhập cho tất cả pages
                    // if (!IsPublicPage())
                    // {
                    //     context.Result = RedirectToPage("/Auth/Login");
                    // }
                }
            }

            base.OnPageHandlerExecuting(context);
        }

        // Helper method để lấy current customer ID
        protected int? GetCurrentCustomerId()
        {
            var userIdString = HttpContext.Session.GetString("UserId");
            if (int.TryParse(userIdString, out int userId))
            {
                return userId;
            }
            return null;
        }

        // Helper method để lấy current dealer ID
        protected int? GetCurrentDealerId()
        {
            return HttpContext.Session.GetInt32("DealerId");
        }

        // Helper method để kiểm tra user role
        protected bool IsInRole(string role)
        {
            var userRole = HttpContext.Session.GetString("UserRole");
            return userRole?.Equals(role, StringComparison.OrdinalIgnoreCase) == true;
        }

        // Helper method để kiểm tra xem page có public không
        // protected virtual bool IsPublicPage()
        // {
        //     // Override trong derived class nếu cần
        //     return false;
        // }
    }
}
