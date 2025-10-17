using System.Security.Claims;
using ASM1.Service.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace ASM1.WebMVC.Pages.Auth
{
    public class LoginModel : BasePageModel
    {
        private readonly ILogger<LoginModel> _logger;
        private readonly IAuthService _authService;

        public LoginModel(ILogger<LoginModel> logger, IAuthService authService)
        {
            _logger = logger;
            _authService = authService;
        }

        // Properties để bind form data
        [BindProperty]
        public string Email { get; set; } = string.Empty;

        [BindProperty]
        public string Password { get; set; } = string.Empty;

        public string? ErrorMessage { get; set; }

        // GET handler - tương đương [HttpGet("Login")]
        public IActionResult OnGet()
        {
            // Kiểm tra nếu đã đăng nhập thì redirect về trang chủ
            var userId = HttpContext.Session.GetString("UserId");
            if (!string.IsNullOrEmpty(userId))
            {
                return RedirectToPage("/Home/Index");
            }

            return Page();
        }

        // POST handler - tương đương [HttpPost("Login")]
        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _authService.Login(Email, Password);

            if (user == null)
            {
                ErrorMessage = "Sai email hoặc mật khẩu";
                return Page();
            }

            // Tạo claims cho cookie authentication
            var roleForClaim = char.ToUpper(user.Role[0]) + user.Role.Substring(1).ToLower();
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Role, roleForClaim),
                new Claim(ClaimTypes.Email, user.Email),
            };

            var claimsIdentity = new ClaimsIdentity(claims, "CarSalesCookies");
            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            // Đăng nhập với cookie authentication
            await HttpContext.SignInAsync("CarSalesCookies", claimsPrincipal);

            // Lưu thông tin user vào session (backup)
            HttpContext.Session.SetString("UserId", user.UserId.ToString());
            HttpContext.Session.SetString("UserRole", user.Role.ToString());
            HttpContext.Session.SetString("UserName", user.FullName);
            HttpContext.Session.SetString("UserEmail", user.Email);

            // Lưu DealerId cho SalesController
            if (user.DealerId.HasValue)
            {
                HttpContext.Session.SetInt32("DealerId", user.DealerId.Value);
            }

            // Thêm thông báo thành công
            TempData["SuccessMessage"] =
                $"Đăng nhập thành công! Chào mừng {user.FullName} ({user.Role})";

            // Redirect về trang chủ
            return RedirectToPage("/Home/Index");
        }
    }
}
