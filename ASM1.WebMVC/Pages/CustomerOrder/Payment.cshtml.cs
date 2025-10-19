using System.ComponentModel.DataAnnotations;
using ASM1.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ASM1.WebMVC.Pages.CustomerOrder
{
    public class PaymentModel : BasePageModel
    {
        private readonly ISalesService _salesService;
        private readonly ICustomerRelationshipService _customerService;
        private readonly ILogger<PaymentModel> _logger;

        public PaymentModel(ISalesService salesService, ICustomerRelationshipService customerService, ILogger<PaymentModel> logger)
        {
            _salesService = salesService;
            _customerService = customerService;
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

        public async Task<IActionResult> OnGetAsync(int? orderId)
        {
            var customerId = GetCurrentCustomerId();
            // If session/claims did not resolve to a customer id, try resolving by email directly via the customer service
            if (customerId == null && User?.Identity?.IsAuthenticated == true)
            {
                var emailClaim = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
                if (!string.IsNullOrEmpty(emailClaim))
                {
                    try
                    {
                        var cust = await _customerService.GetCustomerByEmailAsync(emailClaim);
                        if (cust != null)
                        {
                            customerId = cust.CustomerId;
                            try { HttpContext.Session.SetString("UserId", customerId.ToString()); } catch { }
                            _logger.LogInformation("Resolved customerId from email fallback: {customerId}", customerId);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Failed to resolve customer by email in Payment.OnGetAsync fallback");
                    }
                }
            }
            _logger.LogInformation("Payment.OnGetAsync called. routeOrderId={orderId}, sessionUserId={sessionUserId}, isAuthenticated={isAuth}", orderId, HttpContext.Session.GetString("UserId"), User?.Identity?.IsAuthenticated);
            if (customerId == null)
            {
                TempData["ErrorMessage"] = "Vui lòng đăng nhập.";
                _logger.LogWarning("Payment.OnGetAsync redirect to Login because customerId is null");
                return RedirectToPage("/Auth/Login");
            }

            if (!orderId.HasValue)
            {
                TempData["ErrorMessage"] = "Thiếu thông tin đơn hàng.";
                return RedirectToPage("/Customer/MyOrders");
            }

            OrderId = orderId.Value;
            OrderData = await _salesService.GetOrderAsync(OrderId);
            _logger.LogInformation("Fetched Order. OrderId={OrderId}, OrderCustomerId={OrderCustomerId}, OrderStatus={OrderStatus}", OrderData?.OrderId, OrderData?.CustomerId, OrderData?.Status);

            if (OrderData == null || OrderData.CustomerId != customerId)
            {
                TempData["ErrorMessage"] = "Không tìm thấy đơn hàng hoặc bạn không có quyền xem.";
                _logger.LogWarning("Payment.OnGetAsync redirect to MyOrders. OrderData==null: {isNull}, OrderCustomerId={orderCustomerId}, currentCustomerId={currentCustomerId}", OrderData == null, OrderData?.CustomerId, customerId);
                return RedirectToPage("/Customer/MyOrders");
            }

            if (OrderData.Status != "Confirmed" && OrderData.Status != "Pending")
            {
                TempData["ErrorMessage"] = "Đơn hàng chưa được xác nhận, không thể thanh toán.";
                _logger.LogWarning("Payment.OnGetAsync redirect to OrderDetail because status is {status}", OrderData.Status);
                return RedirectToPage("/Customer/OrderDetail", new { id = OrderId });
            }

            Payments = (await _salesService.GetPaymentsByOrderAsync(OrderId)).ToList();
            TotalPaid = Payments.Sum(p => p.Amount ?? 0);
            OrderTotal = OrderData.Variant?.Price ?? 0;
            RemainingBalance = OrderTotal - TotalPaid;
            IsFullyPaid = RemainingBalance <= 0;
            Amount = RemainingBalance > 0 ? RemainingBalance : 0;

            _logger.LogInformation("Payment calculation: OrderTotal={OrderTotal}, TotalPaid={TotalPaid}, RemainingBalance={RemainingBalance}, IsFullyPaid={IsFullyPaid}", OrderTotal, TotalPaid, RemainingBalance, IsFullyPaid);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            _logger.LogInformation("Payment.OnPostAsync called. OrderId={OrderId}, Amount={Amount}, PaymentMethod={PaymentMethod}, IsAuthenticated={IsAuth}", OrderId, Amount, PaymentMethod, User?.Identity?.IsAuthenticated);
            // Log modelstate errors if any
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
                    TempData["ErrorMessage"] =
                        "Không tìm thấy đơn hàng hoặc bạn không có quyền thanh toán.";
                    return RedirectToPage("/Customer/MyOrders");
                }

                // Calculate remaining balance and use it as payment amount
                var existingPayments = await _salesService.GetPaymentsByOrderAsync(OrderId);
                var totalPaid = existingPayments.Sum(p => p.Amount ?? 0);
                var orderTotal = order.Variant?.Price ?? 0;
                var remainingBalance = orderTotal - totalPaid;

                if (remainingBalance <= 0)
                {
                    TempData["ErrorMessage"] = "Đơn hàng đã được thanh toán đủ.";
                    return RedirectToPage("/Customer/MyOrders");
                }

                // Use remaining balance as payment amount (ignore user input)
                var paymentAmount = remainingBalance;

                await _salesService.ProcessPaymentAsync(OrderId, paymentAmount, PaymentMethod);

                TempData["SuccessMessage"] = $"Thanh toán ${paymentAmount:N2} thành công!";

                // Check if fully paid
                var payments = await _salesService.GetPaymentsByOrderAsync(OrderId);
                var totalPaidAfter = payments.Sum(p => p.Amount ?? 0);

                if (totalPaidAfter >= orderTotal)
                {
                    // Update order status to Completed when fully paid
                    await _salesService.UpdateOrderStatusAsync(OrderId, "Completed");
                    TempData["SuccessMessage"] =
                        "Thanh toán hoàn tất! Đơn hàng của bạn đã được thanh toán đầy đủ.";
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
