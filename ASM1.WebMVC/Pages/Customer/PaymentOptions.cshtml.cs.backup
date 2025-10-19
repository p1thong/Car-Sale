using System.ComponentModel.DataAnnotations;
using ASM1.Service.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ASM1.WebMVC.Pages.Customer
{
    [Authorize(Roles = "Customer")]
    public class PaymentOptionsModel : BasePageModel
    {
        private readonly ISalesService _salesService;
        private readonly ICustomerRelationshipService _customerService;

        public PaymentOptionsModel(ISalesService salesService, ICustomerRelationshipService customerService)
        {
            _salesService = salesService;
            _customerService = customerService;
        }

        // Properties để display data
        public Order Order { get; set; } = null!;
        public IEnumerable<Payment> Payments { get; set; } = new List<Payment>();
        public decimal RemainingBalance { get; set; }

        // Properties để bind form data với validation
        [BindProperty]
        public int OrderId { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Payment amount is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Payment amount must be greater than 0")]
        public decimal PaymentAmount { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Payment method is required")]
        public string PaymentMethod { get; set; } = string.Empty;

        // GET handler - tương đương public async Task<IActionResult> PaymentOptions(int orderId)
        public async Task<IActionResult> OnGetAsync(int orderId)
        {
            try
            {
                // Lấy email từ claims thay vì dùng GetCurrentCustomerId()
                var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
                if (string.IsNullOrEmpty(email))
                {
                    TempData["Error"] = "Vui lòng đăng nhập để thanh toán.";
                    return RedirectToPage("/Auth/Login");
                }

                // Tìm Customer bằng email (pattern đã được áp dụng ở các pages khác)
                var customer = await _customerService.GetCustomerByEmailAsync(email);
                if (customer == null)
                {
                    TempData["Error"] = $"Không tìm thấy thông tin khách hàng với email {email}.";
                    return RedirectToPage("/Auth/Login");
                }

                Order = await _salesService.GetOrderAsync(orderId);
                if (Order == null || Order.CustomerId != customer.CustomerId)
                {
                    TempData["Error"] = "Không tìm thấy đơn hàng hoặc bạn không có quyền truy cập.";
                    return RedirectToPage("/Customer/MyOrders");
                }

                // Get existing payments for this order
                Payments = await _salesService.GetPaymentsByOrderAsync(orderId);
                
                // Tính RemainingBalance trực tiếp từ Order.Variant.Price
                var totalPaid = Payments?.Sum(p => p.Amount ?? 0) ?? 0;
                var orderTotal = Order.Variant?.Price ?? 0;
                RemainingBalance = Math.Max(0, orderTotal - totalPaid);

                // Set OrderId for form binding
                OrderId = orderId;

                return Page();
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Lỗi khi tải trang thanh toán: {ex.Message}";
                return RedirectToPage("/Customer/MyOrders");
            }
        }

        // POST handler - tương đương [HttpPost] ProcessPayment
        public async Task<IActionResult> OnPostAsync()
        {
            // DEBUG: Log khi OnPostAsync được gọi
            Console.WriteLine("=== OnPostAsync called ===");
            Console.WriteLine($"OrderId: {OrderId}");
            Console.WriteLine($"PaymentAmount: {PaymentAmount}");
            Console.WriteLine($"PaymentMethod: {PaymentMethod}");
            Console.WriteLine($"ModelState.IsValid: {ModelState.IsValid}");
            
            try
            {
                // Lấy email từ claims thay vì dùng GetCurrentCustomerId()
                var email = User.FindFirst(System.Security.Claims.ClaimTypes.Email)?.Value;
                Console.WriteLine($"User email: {email}");
                
                if (string.IsNullOrEmpty(email))
                {
                    Console.WriteLine("ERROR: Email is null or empty");
                    TempData["Error"] = "Vui lòng đăng nhập để thanh toán.";
                    return RedirectToPage("/Auth/Login");
                }

                // Validation tự động thông qua Data Annotations
                if (!ModelState.IsValid)
                {
                    // Reload page data nếu validation fail
                    await LoadPageDataAsync(OrderId);
                    return Page();
                }

                // Tìm Customer bằng email
                var customer = await _customerService.GetCustomerByEmailAsync(email);
                if (customer == null)
                {
                    TempData["Error"] = $"Không tìm thấy thông tin khách hàng với email {email}.";
                    return RedirectToPage("/Auth/Login");
                }

                var order = await _salesService.GetOrderAsync(OrderId);
                if (order == null || order.CustomerId != customer.CustomerId)
                {
                    TempData["Error"] = "Không tìm thấy đơn hàng hoặc bạn không có quyền truy cập.";
                    return RedirectToPage("/Customer/MyOrders");
                }

                // Get remaining balance before payment
                var remainingBalanceBefore = await _salesService.GetRemainingBalanceAsync(OrderId);

                // Validation: Payment amount không được vượt quá remaining balance
                if (PaymentAmount > remainingBalanceBefore)
                {
                    ModelState.AddModelError(
                        nameof(PaymentAmount),
                        $"Số tiền thanh toán ({PaymentAmount:C0}) không thể vượt quá số dư còn lại ({remainingBalanceBefore:C0})."
                    );
                    await LoadPageDataAsync(OrderId);
                    return Page();
                }

                // Validation: Payment amount phải > 0 (double check)
                if (PaymentAmount <= 0)
                {
                    ModelState.AddModelError(
                        nameof(PaymentAmount),
                        "Số tiền thanh toán phải lớn hơn 0."
                    );
                    await LoadPageDataAsync(OrderId);
                    return Page();
                }

                // Process payment using the service method
                await _salesService.ProcessPaymentAsync(OrderId, PaymentAmount, PaymentMethod);

                // Get remaining balance after payment
                var remainingBalanceAfter = await _salesService.GetRemainingBalanceAsync(OrderId);

                // Check if order is fully paid
                if (remainingBalanceAfter <= 0)
                {
                    await _salesService.UpdateOrderStatusAsync(OrderId, "Paid");
                }

                // Return JSON response cho AJAX (giống controller cũ)
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return new JsonResult(new { success = true, message = "Thanh toán thành công!" });
                }

                TempData["SuccessMessage"] = "Thanh toán thành công!";
                return RedirectToPage("/Customer/PaymentOptions", new { orderId = OrderId });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR in OnPostAsync: {ex.Message}");
                
                // Return JSON error cho AJAX
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return new JsonResult(new { success = false, message = ex.Message });
                }
                
                ModelState.AddModelError(string.Empty, ex.Message);
                await LoadPageDataAsync(OrderId);
                return Page();
            }
        }

        // Helper method để load page data (reusable cho GET và POST error)
        private async Task LoadPageDataAsync(int orderId)
        {
            Order = await _salesService.GetOrderAsync(orderId);
            Payments = await _salesService.GetPaymentsByOrderAsync(orderId);
            RemainingBalance = await _salesService.GetRemainingBalanceAsync(orderId);
        }
    }
}
