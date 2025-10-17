using ASM1.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ASM1.WebMVC.Pages.CustomerService
{
    public class CreateFeedbackModel : BasePageModel
    {
        private readonly ICustomerRelationshipService _customerService;
        private readonly ISalesService _salesService;

        public CreateFeedbackModel(ICustomerRelationshipService customerService, ISalesService salesService)
        {
            _customerService = customerService;
            _salesService = salesService;
        }

        [BindProperty]
        public int CustomerId { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Vui lòng nhập nội dung phản hồi")]
        public new string Content { get; set; } = string.Empty;

        [BindProperty]
        [Required(ErrorMessage = "Vui lòng chọn đánh giá")]
        [Range(1, 5, ErrorMessage = "Đánh giá phải từ 1 đến 5 sao")]
        public int Rating { get; set; }

        [BindProperty]
        public int? TestDriveId { get; set; }

        public string? CustomerName { get; set; }
        public string? TestDriveInfo { get; set; }
        public IEnumerable<ASM1.Repository.Models.Customer>? Customers { get; set; }

        public async Task<IActionResult> OnGetAsync(int? customerId = null, int? testDriveId = null)
        {
            try
            {
                if (customerId.HasValue && customerId.Value > 0)
                {
                    CustomerId = customerId.Value;
                    var customer = await _customerService.GetCustomerByIdAsync(customerId.Value);
                    if (customer != null)
                    {
                        CustomerName = customer.FullName;
                    }
                }
                else
                {
                    // Load customers for dropdown
                    var dealerId = GetCurrentDealerId();
                    if (dealerId > 0)
                    {
                        Customers = await _salesService.GetCustomersByDealerAsync(dealerId ?? 0);
                    }
                }

                if (testDriveId.HasValue)
                {
                    TestDriveId = testDriveId.Value;
                    try
                    {
                        var testDrive = await _customerService.GetTestDriveByIdAsync(testDriveId.Value);
                        if (testDrive != null)
                        {
                            TestDriveInfo = $"Lái thử {testDrive.Variant?.VehicleModel?.Name} vào {testDrive.ScheduledDate?.ToString("dd/MM/yyyy")}";
                        }
                    }
                    catch { }
                }

                return Page();
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Lỗi khi tải form: {ex.Message}";
                return RedirectToPage("./Feedbacks");
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                // Reload customers if needed
                if (CustomerId == 0)
                {
                    var dealerId = GetCurrentDealerId();
                    if (dealerId > 0)
                    {
                        Customers = await _salesService.GetCustomersByDealerAsync(dealerId ?? 0);
                    }
                }
                return Page();
            }

            try
            {
                await _customerService.CreateFeedbackAsync(CustomerId, Content, Rating);
                TempData["Success"] = "Tạo phản hồi thành công!";
                return RedirectToPage("./Feedbacks");
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Lỗi khi tạo phản hồi: {ex.Message}";
                return Page();
            }
        }
    }
}
