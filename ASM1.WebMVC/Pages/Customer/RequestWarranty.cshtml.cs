using ASM1.Service.Dtos;
using ASM1.Service.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace ASM1.WebMVC.Pages.Customer
{
    [Authorize(Roles = "Customer")]
    public class RequestWarrantyModel : BasePageModel
    {
        private readonly IWarrantyService _warrantyService;
        private readonly ISalesService _salesService;
        private readonly ICustomerRelationshipService _customerService;

        public RequestWarrantyModel(IWarrantyService warrantyService, ISalesService salesService, ICustomerRelationshipService customerService)
        {
            _warrantyService = warrantyService;
            _salesService = salesService;
            _customerService = customerService;
        }

        [BindProperty]
        public WarrantyDto Warranty { get; set; } = new WarrantyDto();

        public List<OrderDto> EligibleOrders { get; set; } = new List<OrderDto>();

        public async Task<IActionResult> OnGetAsync()
        {
            // Get customer email from claims
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(email))
            {
                return RedirectToPage("/Auth/Login");
            }

            // Get customer by email
            var customer = await _customerService.GetCustomerByEmailAsync(email);
            if (customer == null)
            {
                return RedirectToPage("/Auth/Login");
            }

            int customerId = customer.CustomerId;
            Console.WriteLine($"=== DEBUG: Customer email={email}, customerId={customerId}");

            // Get all orders for this customer
            var allOrders = await _salesService.GetOrdersByCustomerAsync(customerId);
            
            Console.WriteLine($"=== DEBUG: Total orders for customer {customerId}: {allOrders.Count()}");
            
            // Filter only eligible orders (within warranty period and completed)
            EligibleOrders = new List<OrderDto>();
            foreach (var order in allOrders)
            {
                Console.WriteLine($"Order {order.OrderId}: Status={order.Status}, OrderDate={order.OrderDate}");
                
                if (order.Status == "Completed" && order.OrderDate.HasValue)
                {
                    var warrantyExpiryDate = order.OrderDate.Value.AddYears(3);
                    var today = DateOnly.FromDateTime(DateTime.Now);
                    Console.WriteLine($"  -> Completed! Expiry={warrantyExpiryDate}, Today={today}, Eligible={today <= warrantyExpiryDate}");
                    
                    if (today <= warrantyExpiryDate)
                    {
                        EligibleOrders.Add(order);
                    }
                }
            }
            
            Console.WriteLine($"=== DEBUG: Eligible orders count: {EligibleOrders.Count}");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Get customer email from claims
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(email))
            {
                return RedirectToPage("/Auth/Login");
            }

            // Get customer by email
            var customer = await _customerService.GetCustomerByEmailAsync(email);
            if (customer == null)
            {
                return RedirectToPage("/Auth/Login");
            }

            int customerId = customer.CustomerId;

            if (!ModelState.IsValid)
            {
                // Reload eligible orders
                await OnGetAsync();
                return Page();
            }

            // Get order details to set CustomerId and DealerId
            var order = await _salesService.GetOrderAsync(Warranty.OrderId);
            if (order == null)
            {
                ModelState.AddModelError("", "Không tìm thấy đơn hàng.");
                await OnGetAsync();
                return Page();
            }

            // Verify the order belongs to this customer
            if (order.CustomerId != customerId)
            {
                ModelState.AddModelError("", "Bạn không có quyền tạo yêu cầu bảo hành cho đơn hàng này.");
                await OnGetAsync();
                return Page();
            }

            // Check if warranty is still valid
            var canRequest = await _warrantyService.CanRequestWarrantyAsync(Warranty.OrderId);
            if (!canRequest)
            {
                ModelState.AddModelError("", "Đơn hàng này đã hết thời hạn bảo hành hoặc không hợp lệ.");
                await OnGetAsync();
                return Page();
            }

            // Set additional properties
            Warranty.CustomerId = customerId;
            Warranty.DealerId = order.DealerId;

            try
            {
                await _warrantyService.CreateWarrantyRequestAsync(Warranty);
                TempData["SuccessMessage"] = "Yêu cầu bảo hành đã được gửi thành công!";
                return RedirectToPage("./MyWarranties");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Lỗi khi gửi yêu cầu bảo hành: {ex.Message}");
                await OnGetAsync();
                return Page();
            }
        }
    }
}
