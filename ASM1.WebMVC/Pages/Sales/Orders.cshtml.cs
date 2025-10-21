using ASM1.Service.Dtos;
using ASM1.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace ASM1.WebMVC.Pages.Sales
{
    public class OrdersModel : BasePageModel
    {
        private readonly ISalesService _salesService;

        public OrdersModel(ISalesService salesService)
        {
            _salesService = salesService;
        }

        public List<OrderDto> Orders { get; set; } = new();

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
