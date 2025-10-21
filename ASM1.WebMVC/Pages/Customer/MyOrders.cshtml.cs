using ASM1.Service.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASM1.WebMVC.Pages.Customer
{
    // Inherit từ BasePageModel để có sẵn helper methods
    [Authorize(Roles = "Customer")]
    public class MyOrdersModel : BasePageModel
    {
        private readonly ISalesService _salesService;
        private readonly ICustomerRelationshipService _customerService;

        public MyOrdersModel(ISalesService salesService, ICustomerRelationshipService customerService)
        {
            _salesService = salesService;
            _customerService = customerService;
        }

        // Property để bind data từ code-behind sang view
        public IEnumerable<Order> Orders { get; set; } = new List<Order>();

        // GET handler - tương đương public async Task<IActionResult> MyOrders()
        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                // Lấy email từ claims thay vì dùng GetCurrentCustomerId()
                var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
                if (string.IsNullOrEmpty(email))
                {
                    TempData["Error"] = "Vui lòng đăng nhập để xem đơn hàng.";
                    return RedirectToPage("/Auth/Login");
                }

                // Tìm Customer bằng email (pattern đã được áp dụng ở các pages khác)
                var customer = await _customerService.GetCustomerByEmailAsync(email);
                if (customer == null)
                {
                    TempData["Error"] = $"Không tìm thấy thông tin khách hàng với email {email}.";
                    return RedirectToPage("/Auth/Login");
                }

                // Lấy orders bằng customer.CustomerId (không phải UserId)
                Orders = await _salesService.GetOrdersByCustomerAsync(customer.CustomerId);
                return Page();
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Lỗi khi tải danh sách đơn hàng: {ex.Message}";
                Orders = Enumerable.Empty<Order>();
                return Page();
            }
        }

        public async Task<IActionResult> OnPostCancelOrderAsync(int orderId)
        {
            try
            {
                // Verify the customer owns this order
                var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
                if (string.IsNullOrEmpty(email))
                {
                    TempData["Error"] = "Vui lòng đăng nhập để thực hiện thao tác.";
                    return RedirectToPage("/Auth/Login");
                }

                var customer = await _customerService.GetCustomerByEmailAsync(email);
                if (customer == null)
                {
                    TempData["Error"] = "Không tìm thấy thông tin khách hàng.";
                    return RedirectToPage();
                }

                // Get order to verify ownership
                var order = await _salesService.GetOrderAsync(orderId);
                if (order == null || order.CustomerId != customer.CustomerId)
                {
                    TempData["Error"] = "Không tìm thấy đơn hàng hoặc bạn không có quyền hủy đơn hàng này.";
                    return RedirectToPage();
                }

                // Cancel the order
                var success = await _salesService.CancelOrderAsync(orderId);
                if (success)
                {
                    TempData["SuccessMessage"] = "Đã hủy đơn hàng thành công. Số lượng xe đã được hoàn lại kho.";
                }
                else
                {
                    TempData["Error"] = "Không thể hủy đơn hàng. Vui lòng thử lại.";
                }

                return RedirectToPage();
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Lỗi khi hủy đơn hàng: {ex.Message}";
                return RedirectToPage();
            }
        }
    }
}
