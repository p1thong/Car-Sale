using ASM1.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ASM1.WebMVC.Pages.CustomerOrder
{
    public class CreateOrderModel : BasePageModel
    {
        private readonly ISalesService _salesService;
        private readonly IVehicleService _vehicleService;
        private readonly ICustomerRelationshipService _customerService;

        public CreateOrderModel(ISalesService salesService, IVehicleService vehicleService, ICustomerRelationshipService customerService)
        {
            _salesService = salesService;
            _vehicleService = vehicleService;
            _customerService = customerService;
        }

        public VehicleVariant? Variant { get; set; }
        public ASM1.Repository.Models.Customer? CustomerData { get; set; }
        public int MaxQuantity { get; set; }

        [BindProperty]
        public int VariantId { get; set; }

        [BindProperty]
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; } = 1;

        [BindProperty]
        public string? Notes { get; set; }

        public async Task<IActionResult> OnGetAsync(int variantId)
        {
            VariantId = variantId;
            
            try
            {
                // Get vehicle info
                Variant = await _vehicleService.GetVehicleVariantByIdAsync(variantId);
                if (Variant == null)
                {
                    TempData["ErrorMessage"] = "Không tìm thấy xe được chọn.";
                    return RedirectToPage("/Home/Index");
                }

                // Lấy email từ claims thay vì dùng GetCurrentCustomerId()
                var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
                if (string.IsNullOrEmpty(email))
                {
                    TempData["ErrorMessage"] = "Vui lòng đăng nhập để đặt hàng.";
                    return RedirectToPage("/Auth/Login");
                }

                // Tìm Customer bằng email (pattern đã được áp dụng ở các pages khác)
                CustomerData = await _customerService.GetCustomerByEmailAsync(email);
                if (CustomerData == null)
                {
                    TempData["ErrorMessage"] = $"Không tìm thấy thông tin khách hàng với email {email}.";
                    return RedirectToPage("/Auth/Login");
                }

                MaxQuantity = Variant.Quantity;
                return Page();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Lỗi khi tải trang đặt hàng: {ex.Message}";
                return RedirectToPage("/Home/Index");
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await OnGetAsync(VariantId);
                return Page();
            }

            try
            {
                // Lấy email từ claims thay vì dùng GetCurrentCustomerId()
                var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
                if (string.IsNullOrEmpty(email))
                {
                    TempData["ErrorMessage"] = "Vui lòng đăng nhập để đặt hàng.";
                    return RedirectToPage("/Auth/Login");
                }

                // Tìm Customer bằng email
                var customer = await _customerService.GetCustomerByEmailAsync(email);
                if (customer == null)
                {
                    TempData["ErrorMessage"] = $"Không tìm thấy thông tin khách hàng với email {email}.";
                    return RedirectToPage("/Auth/Login");
                }

                // Validate quantity
                var variant = await _vehicleService.GetVehicleVariantByIdAsync(VariantId);
                if (variant == null)
                {
                    TempData["ErrorMessage"] = "Không tìm thấy xe được chọn.";
                    return RedirectToPage();
                }

                if (Quantity <= 0 || Quantity > variant.Quantity)
                {
                    ModelState.AddModelError(nameof(Quantity), $"Số lượng không hợp lệ. Chỉ có {variant.Quantity} xe có sẵn.");
                    await OnGetAsync(VariantId);
                    return Page();
                }

                // Create order với customer.CustomerId (ĐÚNG, không phải UserId)
                var order = new Order
                {
                    CustomerId = customer.CustomerId, // ✅ Sử dụng CustomerId từ Customer record
                    DealerId = customer.DealerId,
                    VariantId = VariantId,
                    Status = "Pending",
                    OrderDate = DateOnly.FromDateTime(DateTime.Now)
                };

                var createdOrder = await _salesService.CreateOrderAsync(order);

                TempData["SuccessMessage"] = $"Đặt hàng thành công! Mã đơn hàng: #{createdOrder.OrderId}. Đơn hàng đang chờ dealer xác nhận.";
                return RedirectToPage("/Customer/MyOrders");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Lỗi khi đặt hàng: {ex.Message}");
                await OnGetAsync(VariantId);
                return Page();
            }
        }
    }
}
