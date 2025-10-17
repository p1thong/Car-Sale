using ASM1.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ASM1.WebMVC.Pages.Sales
{
    public class OrderDetailModel : BasePageModel
    {
        private readonly ISalesService _salesService;

        public OrderDetailModel(ISalesService salesService)
        {
            _salesService = salesService;
        }

        public Order? Order { get; set; }
        public SalesContract? SalesContract { get; set; }
        public IEnumerable<Payment> Payments { get; set; } = new List<Payment>();
        public decimal TotalPaid { get; set; }
        public decimal RemainingBalance { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                Order = await _salesService.GetOrderAsync(id);
                
                if (Order == null)
                {
                    TempData["Error"] = "Đơn hàng không tồn tại.";
                    return RedirectToPage("./Orders");
                }

                SalesContract = await _salesService.GetSalesContractByOrderAsync(id);
                Payments = await _salesService.GetPaymentsByOrderAsync(id);
                TotalPaid = Payments?.Sum(p => p.Amount ?? 0) ?? 0;
                RemainingBalance = await _salesService.GetRemainingBalanceAsync(id);

                return Page();
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Lỗi khi tải chi tiết đơn hàng: {ex.Message}";
                return RedirectToPage("./Orders");
            }
        }
    }
}
