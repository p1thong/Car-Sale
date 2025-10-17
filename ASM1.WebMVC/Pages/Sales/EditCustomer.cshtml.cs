using ASM1.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ASM1.WebMVC.Pages.Sales
{
    public class EditCustomerModel : BasePageModel
    {
        private readonly ISalesService _salesService;

        public EditCustomerModel(ISalesService salesService)
        {
            _salesService = salesService;
        }

        [BindProperty]
        public ASM1.Repository.Models.Customer? Customer { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var dealerId = GetCurrentDealerId();
            if (dealerId == 0)
            {
                TempData["Error"] = "Vui lòng đăng nhập với tài khoản Dealer.";
                return RedirectToPage("/Auth/Login");
            }

            try
            {
                Customer = await _salesService.GetCustomerAsync(id);

                if (Customer == null)
                {
                    TempData["Error"] = "Không tìm thấy khách hàng.";
                    return RedirectToPage("./Customers");
                }

                return Page();
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Lỗi khi tải thông tin khách hàng: {ex.Message}";
                return RedirectToPage("./Customers");
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || Customer == null)
            {
                return Page();
            }

            try
            {
                await _salesService.CreateOrUpdateCustomerAsync(Customer);
                TempData["Success"] = "Cập nhật khách hàng thành công!";
                return RedirectToPage("./Customers");
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Lỗi khi cập nhật khách hàng: {ex.Message}";
                return Page();
            }
        }
    }
}
