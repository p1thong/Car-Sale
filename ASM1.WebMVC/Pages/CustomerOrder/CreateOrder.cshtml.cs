using System.ComponentModel.DataAnnotations;
using ASM1.Service.Dtos;
using ASM1.Service.Services.Interfaces;
using ASM1.WebMVC.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace ASM1.WebMVC.Pages.CustomerOrder
{
    public class CreateOrderModel : BasePageModel
    {
        private readonly ISalesService _salesService;
        private readonly IVehicleService _vehicleService;
        private readonly ICustomerRelationshipService _customerService;
        private readonly IHubContext<HubServer> _hubContext;

        public CreateOrderModel(
            ISalesService salesService,
            IVehicleService vehicleService,
            ICustomerRelationshipService customerService,
            IHubContext<HubServer> hubContext
        )
        {
            _salesService = salesService;
            _vehicleService = vehicleService;
            _customerService = customerService;
            _hubContext = hubContext;
        }

        [BindProperty]
        public OrderDto Order { get; set; } = new();
        public VehicleVariantDto? VariantData { get; set; }
        public CustomerDto? CustomerData { get; set; }
        public int MaxQuantity { get; set; }

        [BindProperty]
        public int VariantId { get; set; }

        public async Task<IActionResult> OnGetAsync(int variantId)
        {
            VariantId = variantId;

            try
            {
                // Get vehicle info
                VariantData = await _vehicleService.GetVehicleVariantByIdAsync(variantId);
                if (VariantData == null)
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
                    TempData["ErrorMessage"] =
                        $"Không tìm thấy thông tin khách hàng với email {email}.";
                    return RedirectToPage("/Auth/Login");
                }

                MaxQuantity = VariantData.Quantity;
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
                await OnGetAsync(Order.VariantId);
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
                    TempData["ErrorMessage"] =
                        $"Không tìm thấy thông tin khách hàng với email {email}.";
                    return RedirectToPage("/Auth/Login");
                }

                // Validate quantity
                var variant = await _vehicleService.GetVehicleVariantByIdAsync(Order.VariantId);
                if (variant == null)
                {
                    TempData["ErrorMessage"] = "Không tìm thấy xe được chọn.";
                    return RedirectToPage();
                }

                if (Order.Quantity <= 0 || Order.Quantity > variant.Quantity)
                {
                    ModelState.AddModelError(
                        nameof(Order.Quantity),
                        $"Số lượng không hợp lệ. Chỉ có {variant.Quantity} xe có sẵn."
                    );
                    await OnGetAsync(Order.VariantId);
                    return Page();
                }

                // Create order với customer.CustomerId (ĐÚNG, không phải UserId)
                var order = new OrderDto
                {
                    CustomerId = customer.CustomerId, // ✅ Sử dụng CustomerId từ Customer record
                    DealerId = customer.DealerId,
                    VariantId = Order.VariantId,
                    Quantity = Order.Quantity, // ✅ Thêm quantity
                    Status = "Pending",
                    OrderDate = DateOnly.FromDateTime(DateTime.Now),
                    Notes = Order.Notes
                };

                var createdOrder = await _salesService.CreateOrderAsync(order);

                // Gửi SignalR notification cho đúng user role
                await _hubContext.Clients.All.SendAsync("ShowOrdersForDealer");

                TempData["SuccessMessage"] =
                    $"Đặt hàng thành công! Mã đơn hàng: #{createdOrder.OrderId}. Đơn hàng đang chờ dealer xác nhận.";
                return RedirectToPage("/Customer/MyOrders");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $"Lỗi khi đặt hàng: {ex.Message}");
                await OnGetAsync(Order.VariantId);
                return Page();
            }
        }
    }
}
