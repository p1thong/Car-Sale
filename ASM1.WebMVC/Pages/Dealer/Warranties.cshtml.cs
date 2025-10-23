using ASM1.Service.Dtos;
using ASM1.Service.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace ASM1.WebMVC.Pages.Dealer
{
    [Authorize(Roles = "Dealer")]
    public class WarrantiesModel : BasePageModel
    {
        private readonly IWarrantyService _warrantyService;
        private readonly IDealerService _dealerService;

        public WarrantiesModel(IWarrantyService warrantyService, IDealerService dealerService)
        {
            _warrantyService = warrantyService;
            _dealerService = dealerService;
        }

        public List<WarrantyDto> Warranties { get; set; } = new List<WarrantyDto>();
        
        [BindProperty]
        public string Notes { get; set; } = string.Empty;

        public string StatusFilter { get; set; } = "All";

        public async Task<IActionResult> OnGetAsync(string? statusFilter)
        {
            // Try to get dealerId from session first
            var dealerId = GetCurrentDealerId();
            
            // If not in session, get from email
            if (dealerId == null)
            {
                var email = User.FindFirst(ClaimTypes.Email)?.Value;
                if (string.IsNullOrEmpty(email))
                {
                    TempData["ErrorMessage"] = "Không tìm thấy thông tin Dealer. Vui lòng đăng nhập lại.";
                    return RedirectToPage("/Auth/Login");
                }

                var dealer = await _dealerService.GetDealerByEmailAsync(email);
                if (dealer == null)
                {
                    TempData["ErrorMessage"] = "Không tìm thấy thông tin Dealer.";
                    return RedirectToPage("/Home/Index");
                }
                
                dealerId = dealer.DealerId;
                // Save to session for next time
                HttpContext.Session.SetInt32("DealerId", dealerId.Value);
            }

            // Get all warranties for this dealer
            var allWarranties = await _warrantyService.GetWarrantiesByDealerIdAsync(dealerId.Value);
            Warranties = allWarranties.ToList();

            // Apply status filter
            StatusFilter = statusFilter ?? "All";
            if (StatusFilter != "All")
            {
                Warranties = Warranties.Where(w => w.Status == StatusFilter).ToList();
            }

            // Sort by most recent first
            Warranties = Warranties.OrderByDescending(w => w.RequestDate).ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostConfirmAsync(int warrantyId, string notes)
        {
            try
            {
                await _warrantyService.DealerConfirmWarrantyAsync(warrantyId, notes);
                TempData["SuccessMessage"] = "Đã xác nhận yêu cầu bảo hành thành công!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Lỗi khi xác nhận: {ex.Message}";
            }

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostCompleteRepairAsync(int warrantyId, string notes)
        {
            try
            {
                await _warrantyService.CompleteRepairAsync(warrantyId, notes);
                TempData["SuccessMessage"] = "Đã hoàn thành sửa chữa!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Lỗi khi cập nhật: {ex.Message}";
            }

            return RedirectToPage();
        }
    }
}
