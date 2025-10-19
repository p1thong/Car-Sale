// BasePageModel - Tương tự BaseController
// Mô tả: Base class cho tất cả PageModels, xử lý authentication và session logic
// Chuyển đổi từ: Controllers/BaseController.cs

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ASM1.Service.Services.Interfaces;
using Microsoft.Extensions.Logging;

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
                // Try to log claim information for debugging (best-effort)
                try
                {
                    var _logger = HttpContext.RequestServices.GetService(typeof(ILogger<BasePageModel>)) as ILogger<BasePageModel>;
                    if (_logger != null)
                    {
                        var nameId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                        var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
                        var cId = User.FindFirst("CustomerId")?.Value;
                        _logger.LogInformation("OnPageHandlerExecuting: Authenticated user claims: NameIdentifier={nameId}, CustomerIdClaim={cId}, Email={email}, SessionUserId={session}", nameId, cId, email, HttpContext.Session.GetString("UserId"));
                    }
                }
                catch { }
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
            // 1) Try session first (fast-path)
            try
            {
                var userIdString = HttpContext.Session.GetString("UserId");
                // If session contains an obviously invalid id like "0", remove it so fallbacks can run
                if (userIdString == "0")
                {
                    try { HttpContext.Session.Remove("UserId"); } catch { }
                    userIdString = null;
                }
                if (int.TryParse(userIdString, out int userId))
                {
                    // Treat 0 or negative as invalid/missing
                    if (userId > 0)
                    {
                        return userId;
                    }
                }
            }
            catch
            {
                // Session cookie may be invalid/unprotectable (dev/local). We'll fallback to claims below.
            }

            // 2) Fallback to authenticated user's claims (NameIdentifier or a custom "CustomerId" claim)
            if (User?.Identity?.IsAuthenticated == true)
            {
                // Prefer a custom CustomerId claim if present
                var claimCustomerId = User.FindFirst("CustomerId")?.Value;
                if (int.TryParse(claimCustomerId, out var cid))
                {
                    if (cid > 0)
                    {
                        // store in session for subsequent requests if possible
                        try { HttpContext.Session.SetString("UserId", cid.ToString()); } catch { }
                        return cid;
                    }
                }

                // Fallback to NameIdentifier claim
                var nameId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (int.TryParse(nameId, out var nid))
                {
                    if (nid > 0)
                    {
                        try { HttpContext.Session.SetString("UserId", nid.ToString()); } catch { }
                        return nid;
                    }
                }

                // Final fallback: if we have an email claim, try to resolve CustomerId via ICustomerRelationshipService
                var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
                if (!string.IsNullOrEmpty(email))
                {
                    try
                    {
                        var svc = HttpContext.RequestServices.GetService(typeof(ICustomerRelationshipService)) as ICustomerRelationshipService;
                        if (svc != null)
                        {
                            var cust = svc.GetCustomerByEmailAsync(email).GetAwaiter().GetResult();
                            if (cust != null && cust.CustomerId > 0)
                            {
                                try { HttpContext.Session.SetString("UserId", cust.CustomerId.ToString()); } catch { }
                                return cust.CustomerId;
                            }
                        }
                    }
                    catch
                    {
                        // swallow - this is best-effort fallback
                    }
                }
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
