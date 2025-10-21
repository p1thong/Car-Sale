using ASM1.Service.Dtos;
using ASM1.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASM1.WebMVC.Pages.Sales
{
    public class CustomersModel : BasePageModel
    {
        private readonly ISalesService _salesService;

        public CustomersModel(ISalesService salesService)
        {
            _salesService = salesService;
        }

        public List<CustomerDto> Customers { get; set; } = new();

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
                Customers = (
                    await _salesService.GetCustomersByDealerAsync(dealerId.Value)
                ).ToList();

                if (!Customers.Any())
                {
                    TempData["InfoMessage"] = $"Chưa có khách hàng nào. Hãy thêm khách hàng mới!";
                }

                return Page();
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Lỗi khi tải danh sách khách hàng: {ex.Message}";
                return Page();
            }
        }
    }
}
