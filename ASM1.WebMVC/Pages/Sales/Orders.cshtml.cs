// Chuyển đổi từ: SalesController.Orders
using ASM1.Repository.Models;
using ASM1.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ASM1.WebMVC.Pages.Sales
{
    public class OrdersModel : BasePageModel
    {
        private readonly ISalesService _salesService;

        public OrdersModel(ISalesService salesService)
        {
            _salesService = salesService;
        }

        public List<Order> Orders { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            var dealerId = GetCurrentDealerId();
            if (dealerId == null)
            {
                TempData["ErrorMessage"] = "Dealer access required.";
                return RedirectToPage("/Auth/Login");
            }

            try
            {
                Orders = (await _salesService.GetOrdersByDealerAsync(dealerId.Value)).ToList();
                return Page();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error: {ex.Message}";
                return Page();
            }
        }
    }
}
