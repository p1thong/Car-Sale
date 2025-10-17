using ASM1.Service.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ASM1.WebMVC.Pages.DealerOrder
{
    [Authorize(Roles = "Dealer")]
    public class PendingOrdersModel : BasePageModel
    {
        private readonly ISalesService _salesService;
        private readonly IDealerService _dealerService;

        public PendingOrdersModel(ISalesService salesService, IDealerService dealerService)
        {
            _salesService = salesService;
            _dealerService = dealerService;
        }

        public IEnumerable<Order> Orders { get; set; } = new List<Order>();

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                var email = User.FindFirst(ClaimTypes.Email)?.Value;
                if (string.IsNullOrEmpty(email))
                {
                    TempData["Error"] = "Vui lòng đăng nhập lại.";
                    return RedirectToPage("/Auth/Login");
                }

                var dealer = await _dealerService.GetDealerByEmailAsync(email);
                if (dealer == null)
                {
                    TempData["Error"] = "Không tìm thấy thông tin Dealer.";
                    return RedirectToPage("/Auth/Login");
                }

                Orders = await _salesService.GetPendingOrdersByDealerAsync(dealer.DealerId);
                return Page();
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Lỗi khi tải danh sách đơn hàng: {ex.Message}";
                return Page();
            }
        }
    }
}
