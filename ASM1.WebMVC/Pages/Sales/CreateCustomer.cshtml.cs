using ASM1.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ASM1.WebMVC.Pages.Sales
{
    public class CreateCustomerModel : BasePageModel
    {
        private readonly ISalesService _salesService;

        public CreateCustomerModel(ISalesService salesService)
        {
            _salesService = salesService;
        }

        [BindProperty]
        public ASM1.Repository.Models.Customer Customer { get; set; } =
            new ASM1.Repository.Models.Customer();

        public IActionResult OnGet()
        {
            var dealerId = GetCurrentDealerId();
            if (dealerId == 0)
            {
                TempData["Error"] = "Vui lòng đăng nhập với tài khoản Dealer.";
                return RedirectToPage("/Auth/Login");
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var dealerId = GetCurrentDealerId();
            if (dealerId == 0)
            {
                TempData["Error"] = "Vui lòng đăng nhập với tài khoản Dealer.";
                return RedirectToPage("/Auth/Login");
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                Customer.DealerId = dealerId ?? 0;
                await _salesService.CreateOrUpdateCustomerAsync(Customer);
                TempData["Success"] = "Tạo khách hàng thành công!";
                return RedirectToPage("./Customers");
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Lỗi khi tạo khách hàng: {ex.Message}";
                return Page();
            }
        }
    }
}
