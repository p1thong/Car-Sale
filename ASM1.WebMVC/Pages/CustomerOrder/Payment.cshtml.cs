using ASM1.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ASM1.WebMVC.Pages.CustomerOrder
{
    public class PaymentModel : BasePageModel
    {
        private readonly ISalesService _salesService;
        private readonly ILogger<PaymentModel> _logger;

        public PaymentModel(ISalesService salesService, ILogger<PaymentModel> logger)
        {
            _salesService = salesService;
            _logger = logger;
        }

        public Order? OrderData { get; set; }
        public List<Payment> Payments { get; set; } = new();
        public decimal TotalPaid { get; set; }
        public decimal OrderTotal { get; set; }
        public decimal RemainingBalance { get; set; }
        public bool IsFullyPaid { get; set; }

        [BindProperty]
        public int OrderId { get; set; }

        [BindProperty]
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than 0")]
        public decimal Amount { get; set; }

        [BindProperty]
        [Required]
        public string PaymentMethod { get; set; } = "Cash";

        public async Task<IActionResult> OnGetAsync(int orderId)
        {
            var customerId = GetCurrentCustomerId();
            if (customerId == null)
            {
                TempData["ErrorMessage"] = "Vui lòng đăng nhập.";
                return RedirectToPage("/Auth/Login");
            }

            OrderId = orderId;
            OrderData = await _salesService.GetOrderAsync(orderId);

            if (OrderData == null || OrderData.CustomerId != customerId)
            {
                TempData["ErrorMessage"] = "Không tìm thấy đơn hàng hoặc bạn không có quyền xem.";
                return RedirectToPage("/Customer/MyOrders");
            }

            if (OrderData.Status != "Confirmed")
            {
                TempData["ErrorMessage"] = "Đơn hàng chưa được xác nhận, không thể thanh toán.";
                return RedirectToPage("/CustomerOrder/OrderStatus", new { orderId });
            }

            Payments = (await _salesService.GetPaymentsByOrderAsync(orderId)).ToList();
            TotalPaid = Payments.Sum(p => p.Amount ?? 0);
            OrderTotal = OrderData.Variant?.Price ?? 0;
            RemainingBalance = OrderTotal - TotalPaid;
            IsFullyPaid = RemainingBalance <= 0;
            Amount = RemainingBalance > 0 ? RemainingBalance : 0;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await OnGetAsync(OrderId);
                return Page();
            }

            var customerId = GetCurrentCustomerId();
            if (customerId == null)
            {
                TempData["ErrorMessage"] = "Vui lòng đăng nhập.";
                return RedirectToPage("/Auth/Login");
            }

            try
            {
                var order = await _salesService.GetOrderAsync(OrderId);

                if (order == null || order.CustomerId != customerId)
                {
                    TempData["ErrorMessage"] = "Không tìm thấy đơn hàng hoặc bạn không có quyền thanh toán.";
                    return RedirectToPage("/Customer/MyOrders");
                }

                if (Amount <= 0)
                {
                    ModelState.AddModelError(nameof(Amount), "Số tiền thanh toán phải lớn hơn 0.");
                    await OnGetAsync(OrderId);
                    return Page();
                }

                await _salesService.ProcessPaymentAsync(OrderId, Amount, PaymentMethod);

                TempData["SuccessMessage"] = $"Thanh toán ${Amount:N2} thành công!";

                // Check if fully paid
                var payments = await _salesService.GetPaymentsByOrderAsync(OrderId);
                var totalPaid = payments.Sum(p => p.Amount ?? 0);
                var orderTotal = order.Variant?.Price ?? 0;

                if (totalPaid >= orderTotal)
                {
                    TempData["SuccessMessage"] = "Thanh toán hoàn tất! Đơn hàng của bạn đã được thanh toán đầy đủ.";
                    return RedirectToPage("/Customer/MyOrders");
                }

                return RedirectToPage(new { orderId = OrderId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing payment for Order {OrderId}", OrderId);
                ModelState.AddModelError(string.Empty, $"Lỗi khi thanh toán: {ex.Message}");
                await OnGetAsync(OrderId);
                return Page();
            }
        }
    }
}
