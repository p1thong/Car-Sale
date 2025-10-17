using ASM1.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ASM1.WebMVC.Pages.DealerOrder
{
    public class OrderDetailModel : BasePageModel
    {
        private readonly ISalesService _salesService;
        private readonly IDealerService _dealerService;

        public OrderDetailModel(ISalesService salesService, IDealerService dealerService)
        {
            _salesService = salesService;
            _dealerService = dealerService;
        }

        public Order? Order { get; set; }
        public IEnumerable<Payment> Payments { get; set; } = new List<Payment>();
        public decimal TotalPaid { get; set; }
        public decimal OrderTotal { get; set; }
        public decimal RemainingBalance { get; set; }

        private async Task<int?> GetCurrentDealerIdFromEmailAsync()
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(email))
                return null;

            var dealer = await _dealerService.GetDealerByEmailAsync(email);
            return dealer?.DealerId;
        }

        public async Task<IActionResult> OnGetAsync(int orderId)
        {
            try
            {
                var email = User.FindFirst(ClaimTypes.Email)?.Value;
                if (string.IsNullOrEmpty(email))
                {
                    TempData["Error"] = "Vui lòng đăng nhập lại.";
                    return RedirectToPage("/Auth/Login");
                }

                var dealer = await _dealerService.GetDealerByEmailAsync(email);
                if (dealer == null)
                {
                    TempData["Error"] = "Không tìm thấy thông tin Dealer.";
                    return RedirectToPage("/Auth/Login");
                }

                Order = await _salesService.GetOrderAsync(orderId);

                if (Order == null || Order.DealerId != dealer.DealerId)
                {
                    TempData["Error"] = "Không tìm thấy đơn hàng hoặc bạn không có quyền xem.";
                    return RedirectToPage("./AllOrders");
                }

                // Lấy thông tin thanh toán
                Payments = await _salesService.GetPaymentsByOrderAsync(orderId);
                TotalPaid = Payments?.Sum(p => p.Amount ?? 0) ?? 0;
                OrderTotal = Order.Variant?.Price ?? 0;
                RemainingBalance = OrderTotal - TotalPaid;

                return Page();
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Lỗi khi tải chi tiết đơn hàng: {ex.Message}";
                return RedirectToPage("./AllOrders");
            }
        }

        public async Task<IActionResult> OnPostConfirmAsync(int orderId, string dealerNotes = "")
        {
            try
            {
                var dealerId = await GetCurrentDealerIdFromEmailAsync();
                if (dealerId == null)
                {
                    TempData["Error"] = "Vui lòng đăng nhập lại.";
                    return RedirectToPage("/Auth/Login");
                }

                var order = await _salesService.GetOrderAsync(orderId);

                if (order == null || order.DealerId != dealerId)
                {
                    TempData["Error"] = "Không tìm thấy đơn hàng hoặc bạn không có quyền xử lý.";
                    return RedirectToPage("./PendingOrders");
                }

                if (order.Status != "Pending")
                {
                    TempData["Error"] = "Đơn hàng không ở trạng thái chờ xác nhận.";
                    return RedirectToPage("./OrderDetail", new { orderId });
                }

                await _salesService.ConfirmOrderAsync(orderId, dealerNotes);

                TempData["Success"] = "Đã xác nhận đơn hàng thành công!";
                return RedirectToPage("./OrderDetail", new { orderId });
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Lỗi khi xác nhận đơn hàng: {ex.Message}";
                return RedirectToPage("./OrderDetail", new { orderId });
            }
        }

        public async Task<IActionResult> OnPostRejectAsync(int orderId, string rejectionReason)
        {
            try
            {
                var dealerId = await GetCurrentDealerIdFromEmailAsync();
                if (dealerId == null)
                {
                    TempData["Error"] = "Vui lòng đăng nhập lại.";
                    return RedirectToPage("/Auth/Login");
                }

                var order = await _salesService.GetOrderAsync(orderId);

                if (order == null || order.DealerId != dealerId)
                {
                    TempData["Error"] = "Không tìm thấy đơn hàng hoặc bạn không có quyền xử lý.";
                    return RedirectToPage("./PendingOrders");
                }

                if (order.Status != "Pending")
                {
                    TempData["Error"] = "Đơn hàng không ở trạng thái chờ xác nhận.";
                    return RedirectToPage("./OrderDetail", new { orderId });
                }

                await _salesService.RejectOrderAsync(orderId, rejectionReason);

                TempData["Success"] = "Đã từ chối đơn hàng.";
                return RedirectToPage("./PendingOrders");
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Lỗi khi từ chối đơn hàng: {ex.Message}";
                return RedirectToPage("./OrderDetail", new { orderId });
            }
        }

        public async Task<IActionResult> OnPostDeliverAsync(int orderId)
        {
            try
            {
                var dealerId = await GetCurrentDealerIdFromEmailAsync();
                if (dealerId == null)
                {
                    TempData["Error"] = "Vui lòng đăng nhập lại.";
                    return RedirectToPage("/Auth/Login");
                }

                var order = await _salesService.GetOrderAsync(orderId);

                if (order == null || order.DealerId != dealerId)
                {
                    TempData["Error"] = "Không tìm thấy đơn hàng hoặc bạn không có quyền xử lý.";
                    return RedirectToPage("./AllOrders");
                }

                // Kiểm tra đã thanh toán đủ chưa
                var payments = await _salesService.GetPaymentsByOrderAsync(orderId);
                var totalPaid = payments?.Sum(p => p.Amount ?? 0) ?? 0;
                var orderTotal = order.Variant?.Price ?? 0;

                if (totalPaid < orderTotal)
                {
                    TempData["Error"] = "Khách hàng chưa thanh toán đủ, không thể giao xe.";
                    return RedirectToPage("./OrderDetail", new { orderId });
                }

                await _salesService.CompleteOrderAsync(orderId);

                TempData["Success"] = "Đã giao xe thành công!";
                return RedirectToPage("./OrderDetail", new { orderId });
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Lỗi khi giao xe: {ex.Message}";
                return RedirectToPage("./OrderDetail", new { orderId });
            }
        }
    }
}
