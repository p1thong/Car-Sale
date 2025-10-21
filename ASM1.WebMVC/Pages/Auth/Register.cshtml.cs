using ASM1.Service.Dtos;
using ASM1.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ASM1.WebMVC.Pages.Auth
{
    public class RegisterModel : BasePageModel
    {
        private readonly ILogger<RegisterModel> _logger;
        private readonly IAuthService _authService;
        private readonly IDealerService _dealerService;

        public RegisterModel(
            ILogger<RegisterModel> logger,
            IAuthService authService,
            IDealerService dealerService
        )
        {
            _logger = logger;
            _authService = authService;
            _dealerService = dealerService;
        }

        // Properties để bind form data với validation attributes
        [BindProperty]
        [Required(ErrorMessage = "Vui lòng nhập họ tên")]
        public string FullName { get; set; } = string.Empty;

        [BindProperty]
        [Required(ErrorMessage = "Vui lòng nhập email")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; } = string.Empty;

        [BindProperty]
        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        public string Phone { get; set; } = string.Empty;

        [BindProperty]
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [MinLength(6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự")]
        public string Password { get; set; } = string.Empty;

        [BindProperty]
        [Required(ErrorMessage = "Vui lòng xác nhận mật khẩu")]
        [Compare(nameof(Password), ErrorMessage = "Mật khẩu xác nhận không khớp")]
        public string ConfirmPassword { get; set; } = string.Empty;

        public string? ErrorMessage { get; set; }
        public string? SuccessMessage { get; set; }

        // GET handler - tương đương [HttpGet("Register")]
        public IActionResult OnGet()
        {
            return Page();
        }

        // POST handler - tương đương [HttpPost("Register")]
        public async Task<IActionResult> OnPostAsync()
        {
            // Validation tự động thông qua Data Annotations
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Kiểm tra email đã tồn tại
            var existingUser = await _authService.GetUserByEmail(Email);
            if (existingUser != null)
            {
                ErrorMessage = "Email đã được sử dụng";
                return Page();
            }

            // Tự động chọn dealer đầu tiên
            var dealers = await _dealerService.GetAllDealersAsync();
            var firstDealer = dealers.FirstOrDefault();
            if (firstDealer == null)
            {
                ErrorMessage = "Hệ thống chưa có đại lý nào. Vui lòng liên hệ admin.";
                return Page();
            }

            // Tạo user mới với vai trò mặc định là customer
            var newUser = new UserDto
            {
                FullName = FullName,
                Email = Email,
                Phone = Phone,
                Password = Password,
                Role = "customer",
            };

            var result = await _authService.Register(newUser, firstDealer.DealerId);
            if (result)
            {
                SuccessMessage = "Đăng ký thành công! Vui lòng đăng nhập.";
                return Page();
            }
            else
            {
                ErrorMessage = "Đăng ký thất bại. Vui lòng thử lại sau.";
                return Page();
            }
        }
    }
}
