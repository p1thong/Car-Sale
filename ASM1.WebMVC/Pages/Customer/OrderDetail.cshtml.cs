using ASM1.Service.Services.Interfaces;
using ASM1.WebMVC.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace ASM1.WebMVC.Pages.Customer
{
    [Authorize(Roles = "Customer")]
    public class OrderDetailModel : BasePageModel
    {
        private readonly ISalesService _salesService;
        private readonly ICustomerRelationshipService _customerService;
        private readonly IHubContext<HubServer> _hubContext;

        public OrderDetailModel(ISalesService salesService, ICustomerRelationshipService customerService, IHubContext<HubServer> hubContext)
        {
            _salesService = salesService;
            _customerService = customerService;
            _hubContext = hubContext;
        }

        public Order? Order { get; set; }
        public IEnumerable<Payment> Payments { get; set; } = new List<Payment>();
        public decimal TotalPaid { get; set; }
        public decimal OrderTotal { get; set; }
        public decimal RemainingBalance { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                var email = User.FindFirst(ClaimTypes.Email)?.Value;
                if (string.IsNullOrEmpty(email))
                {
                    TempData["Error"] = "Vui lòng đăng nhập lại.";
                    return RedirectToPage("/Auth/Login");
                }

                var customer = await _customerService.GetCustomerByEmailAsync(email);
                if (customer == null)
                {
                    TempData["Error"] = "Không tìm thấy thông tin khách hàng.";
                    return RedirectToPage("/Auth/Login");
                }

                Order = await _salesService.GetOrderAsync(id);

                if (Order == null || Order.CustomerId != customer.CustomerId)
                {
                    TempData["Error"] = "Không tìm thấy đơn hàng hoặc bạn không có quyền xem.";
                    return RedirectToPage("./MyOrders");
                }

                // Lấy thông tin thanh toán
                Payments = await _salesService.GetPaymentsByOrderAsync(id);
                TotalPaid = Payments?.Sum(p => p.Amount ?? 0) ?? 0;
                OrderTotal = Order.Variant?.Price ?? 0;
                RemainingBalance = Math.Max(0, OrderTotal - TotalPaid);

                return Page();
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Lỗi khi tải chi tiết đơn hàng: {ex.Message}";
                return RedirectToPage("./MyOrders");
            }
        }

        public async Task<IActionResult> OnPostConfirmReceivedAsync(int orderId)
        {
            try
            {
                var email = User.FindFirst(ClaimTypes.Email)?.Value;
                if (string.IsNullOrEmpty(email))
                {
                    TempData["Error"] = "Vui lòng đăng nhập lại.";
                    return RedirectToPage("/Auth/Login");
                }

                var customer = await _customerService.GetCustomerByEmailAsync(email);
                if (customer == null)
                {
                    TempData["Error"] = "Không tìm thấy thông tin khách hàng.";
                    return RedirectToPage("/Auth/Login");
                }

                var order = await _salesService.GetOrderAsync(orderId);
                if (order == null || order.CustomerId != customer.CustomerId)
                {
                    TempData["Error"] = "Không tìm thấy đơn hàng hoặc bạn không có quyền xử lý.";
                    return RedirectToPage("./MyOrders");
                }

                // Kiểm tra có payment nào đã được giao chưa
                var payments = await _salesService.GetPaymentsByOrderAsync(orderId);
                var hasDelivered = payments.Any(p => p.Status == "Delivered");

                if (!hasDelivered)
                {
                    TempData["Error"] = "Xe chưa được giao, không thể xác nhận nhận xe.";
                    return RedirectToPage("./OrderDetail", new { id = orderId });
                }

                // Cập nhật tất cả payment status thành "Received"
                foreach (var payment in payments.Where(p => p.Status == "Delivered"))
                {
                    await _salesService.UpdatePaymentStatusAsync(payment.PaymentId, "Received");
                }

                await _hubContext.Clients.All.SendAsync("CustomerConfirmReceived");

                TempData["Success"] = "Bạn đã xác nhận nhận xe thành công! Cảm ơn bạn đã sử dụng dịch vụ của chúng tôi.";
                return RedirectToPage("./OrderDetail", new { id = orderId });
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Lỗi khi xác nhận nhận xe: {ex.Message}";
                return RedirectToPage("./OrderDetail", new { id = orderId });
            }
        }
    }
}
