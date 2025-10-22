using ASM1.Service.Dtos;
using ASM1.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ASM1.WebMVC.Pages.Customer
{
    public class MyWarrantiesModel : BasePageModel
    {
        private readonly IWarrantyService _warrantyService;
        private readonly ICustomerRelationshipService _customerService;

        public MyWarrantiesModel(IWarrantyService warrantyService, ICustomerRelationshipService customerService)
        {
            _warrantyService = warrantyService;
            _customerService = customerService;
        }

        public List<WarrantyDto> Warranties { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(email))
            {
                TempData["Error"] = "Vui lòng đăng nhập.";
                return RedirectToPage("/Auth/Login");
            }

            var customer = await _customerService.GetCustomerByEmailAsync(email);
            if (customer == null)
            {
                TempData["Error"] = "Không tìm thấy thông tin khách hàng.";
                return RedirectToPage("/Home/Index");
            }

            Warranties = (await _warrantyService.GetWarrantiesByCustomerIdAsync(customer.CustomerId)).ToList();
            return Page();
        }

        public async Task<IActionResult> OnPostConfirmReceiptAsync(int warrantyId)
        {
            try
            {
                await _warrantyService.CustomerConfirmReceiptAsync(warrantyId);
                TempData["Success"] = "Đã xác nhận nhận xe thành công!";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Lỗi: {ex.Message}";
            }

            return RedirectToPage();
        }
    }
}
