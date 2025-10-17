using ASM1.Service.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ASM1.WebMVC.Pages.CustomerService
{
    public class CancelTestDriveModel : BasePageModel
    {
        private readonly ICustomerRelationshipService _customerService;

        public CancelTestDriveModel(ICustomerRelationshipService customerService)
        {
            _customerService = customerService;
        }

        public async Task<IActionResult> OnPostAsync(int testDriveId)
        {
            try
            {
                await _customerService.UpdateTestDriveStatusAsync(testDriveId, "Cancelled");
                return new JsonResult(
                    new { success = true, message = "Lịch lái thử đã được hủy thành công!" }
                );
            }
            catch (Exception ex)
            {
                return new JsonResult(new { success = false, message = ex.Message });
            }
        }
    }
}
