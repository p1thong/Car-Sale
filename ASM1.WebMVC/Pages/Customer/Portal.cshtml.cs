using ASM1.Service.Dtos;
using ASM1.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ASM1.WebMVC.Pages.Customer
{
    public class PortalModel : BasePageModel
    {
        private readonly ISalesService _salesService;

        public PortalModel(ISalesService salesService)
        {
            _salesService = salesService;
        }

        public CustomerDto? CustomerData { get; set; }
        public int OrdersCount { get; set; }
        public int PendingOrdersCount { get; set; }
        public int ConfirmedOrdersCount { get; set; }
        public int CompletedOrdersCount { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var customerId = GetCurrentCustomerId();
            if (customerId == null)
            {
                TempData["ErrorMessage"] = "Please login to access customer portal.";
                return RedirectToPage("/Auth/Login");
            }

            try
            {
                CustomerData = await _salesService.GetCustomerAsync(customerId.Value);
                if (CustomerData == null)
                {
                    TempData["ErrorMessage"] = "Customer not found.";
                    return RedirectToPage("/Auth/Login");
                }

                // Get customer stats
                var orders = await _salesService.GetOrdersByCustomerAsync(customerId.Value);

                OrdersCount = orders.Count();
                PendingOrdersCount = orders.Count(o => o.Status == "Pending");
                ConfirmedOrdersCount = orders.Count(o => o.Status == "Confirmed");
                CompletedOrdersCount = orders.Count(o => o.Status == "Completed");

                return Page();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error loading customer portal: {ex.Message}";
                return RedirectToPage("/Home/Index");
            }
        }
    }
}
