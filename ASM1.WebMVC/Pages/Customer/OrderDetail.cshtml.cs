using ASM1.Service.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ASM1.WebMVC.Pages.Customer
{
    [Authorize(Roles = "Customer")]
    public class OrderDetailModel : BasePageModel
    {
        private readonly ISalesService _salesService;
        private readonly ICustomerRelationshipService _customerService;

        public OrderDetailModel(ISalesService salesService, ICustomerRelationshipService customerService)
        {
            _salesService = salesService;
            _customerService = customerService;
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
    }
}
