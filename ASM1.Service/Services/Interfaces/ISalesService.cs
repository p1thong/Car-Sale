using ASM1.Service.Dtos;

namespace ASM1.Service.Services.Interfaces
{
    public interface ISalesService
    {
        // Customer Management
        Task<CustomerDto> CreateOrUpdateCustomerAsync(CustomerDto customer);
        Task<CustomerDto?> GetCustomerAsync(int customerId);
        Task<IEnumerable<CustomerDto>> GetCustomersByDealerAsync(int dealerId);

        // Order Management
        Task<OrderDto> CreateOrderAsync(OrderDto order);
        Task<OrderDto?> GetOrderAsync(int orderId);
        Task<OrderDto> UpdateOrderStatusAsync(int orderId, string status);
        Task<bool> CancelOrderAsync(int orderId);
        Task<IEnumerable<OrderDto>> GetOrdersByDealerAsync(int dealerId);
        Task<IEnumerable<OrderDto>> GetOrdersByCustomerAsync(int customerId);
        Task<IEnumerable<OrderDto>> GetPendingOrdersByDealerAsync(int dealerId);
        Task<OrderDto> ConfirmOrderAsync(int orderId, string dealerNotes = "");
        Task<OrderDto> RejectOrderAsync(int orderId, string rejectionReason);

        // Sales Contract Management
        Task<SalesContractDto> CreateSalesContractAsync(int orderId, decimal totalAmount, string terms);
        Task<SalesContractDto?> GetSalesContractByOrderAsync(int orderId);

        // Payment Management
        Task<PaymentDto> ProcessPaymentAsync(int orderId, decimal amount, string paymentMethod);
        Task<IEnumerable<PaymentDto>> GetPaymentsByOrderAsync(int orderId);
        Task<decimal> GetRemainingBalanceAsync(int orderId);
        Task<PaymentDto> UpdatePaymentStatusAsync(int paymentId, string status);

        // Vehicle Delivery
        Task<OrderDto> CompleteOrderAsync(int orderId);
        Task<bool> IsOrderReadyForDeliveryAsync(int orderId);
    }
}