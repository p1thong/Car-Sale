using ASM1.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ASM1.WebMVC.Pages.CustomerOrder
{
    public class OrderStatusModel : BasePageModel
    {
        private readonly ISalesService _salesService;

        public OrderStatusModel(ISalesService salesService)
        {
            _salesService = salesService;
        }

        public Order? OrderData { get; set; }

        public async Task<IActionResult> OnGetAsync(int orderId)
        {
            var customerId = GetCurrentCustomerId();
            if (customerId == null)
            {
                TempData["ErrorMessage"] = "Vui lòng đăng nhập.";
                return RedirectToPage("/Auth/Login");
            }

            OrderData = await _salesService.GetOrderAsync(orderId);

            if (OrderData == null || OrderData.CustomerId != customerId)
            {
                TempData["ErrorMessage"] = "Không tìm thấy đơn hàng hoặc bạn không có quyền xem.";
                return RedirectToPage("/Home/Index");
            }

            return Page();
        }
    }
}
