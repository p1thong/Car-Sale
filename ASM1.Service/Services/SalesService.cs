using ASM1.Repository.Models;
using ASM1.Repository.Repositories.Interfaces;
using ASM1.Service.Services.Interfaces;
using ASM1.Service.Dtos;
using ASM1.Service.Mappers;

namespace ASM1.Service.Services
{
    public class SalesService : ISalesService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IMapper _mapper;

        public SalesService(
            ICustomerRepository customerRepository,
            IOrderRepository orderRepository,
            IPaymentRepository paymentRepository,
            IVehicleRepository vehicleRepository,
            IMapper mapper)
        {
            _customerRepository = customerRepository;
            _orderRepository = orderRepository;
            _paymentRepository = paymentRepository;
            _vehicleRepository = vehicleRepository;
            _mapper = mapper;
        }

        // Customer Management
        public async Task<CustomerDto> CreateOrUpdateCustomerAsync(CustomerDto customerDto)
        {
            var customer = _mapper.Map<CustomerDto, Customer>(customerDto);
            Customer result;
            
            if (customer.CustomerId == 0)
            {
                result = await _customerRepository.CreateCustomerAsync(customer);
            }
            else
            {
                result = await _customerRepository.UpdateCustomerAsync(customer);
            }
            
            return _mapper.Map<Customer, CustomerDto>(result);
        }

        public async Task<CustomerDto?> GetCustomerAsync(int customerId)
        {
            var customer = await _customerRepository.GetCustomerByIdAsync(customerId);
            return customer != null ? _mapper.Map<Customer, CustomerDto>(customer) : null;
        }

        public async Task<IEnumerable<CustomerDto>> GetCustomersByDealerAsync(int dealerId)
        {
            var customers = await _customerRepository.GetCustomersByDealerAsync(dealerId);
            return _mapper.MapList<Customer, CustomerDto>(customers);
        }

        // Order Management
        public async Task<OrderDto> CreateOrderAsync(OrderDto orderDto)
        {
            var order = _mapper.Map<OrderDto, Order>(orderDto);
            var result = await _orderRepository.CreateOrderAsync(order);
            return _mapper.Map<Order, OrderDto>(result);
        }

        public async Task<OrderDto?> GetOrderAsync(int orderId)
        {
            var order = await _orderRepository.GetOrderWithDetailsAsync(orderId);
            if (order == null) return null;

            var orderDto = _mapper.Map<Order, OrderDto>(order);

            // Flatten related data
            orderDto.CustomerName = order.Customer?.FullName;
            orderDto.CustomerEmail = order.Customer?.Email;
            orderDto.CustomerPhone = order.Customer?.Phone;
            orderDto.DealerName = order.Dealer?.FullName;
            orderDto.ModelName = order.Variant?.VehicleModel?.Name;
            orderDto.ManufacturerName = order.Variant?.VehicleModel?.Manufacturer?.Name;
            orderDto.VariantVersion = order.Variant?.Version;
            orderDto.VariantColor = order.Variant?.Color;
            orderDto.ProductYear = order.Variant?.ProductYear;
            orderDto.Price = order.Variant?.Price;
            orderDto.ImageUrl = order.Variant?.VehicleModel?.ImageUrl;

            // Populate payment information
            var payments = await _paymentRepository.GetPaymentsByOrderAsync(orderId);
            orderDto.TotalPaid = payments.Sum(p => p.Amount);
            var totalAmount = order.TotalPrice ?? (order.Variant?.Price * order.Quantity) ?? 0;
            orderDto.RemainingAmount = Math.Max(0, totalAmount - (orderDto.TotalPaid ?? 0));
            orderDto.IsFullyPaid = orderDto.RemainingAmount <= 0;

            return orderDto;
        }

        public async Task<OrderDto> UpdateOrderStatusAsync(int orderId, string status)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order == null)
                throw new ArgumentException("Order not found");

            order.Status = status;
            var result = await _orderRepository.UpdateOrderAsync(order);
            return _mapper.Map<Order, OrderDto>(result);
        }

        public async Task<bool> CancelOrderAsync(int orderId)
        {
            return await _orderRepository.CancelOrderAsync(orderId);
        }

        public async Task<IEnumerable<OrderDto>> GetOrdersByDealerAsync(int dealerId)
        {
            var orders = await _orderRepository.GetOrdersByDealerAsync(dealerId);
            var orderDtos = new List<OrderDto>();

            foreach (var order in orders)
            {
                var orderDto = _mapper.Map<Order, OrderDto>(order);
                orderDto.CustomerName = order.Customer?.FullName;
                orderDto.CustomerEmail = order.Customer?.Email;
                orderDto.CustomerPhone = order.Customer?.Phone;
                orderDto.ModelName = order.Variant?.VehicleModel?.Name;
                orderDto.VariantVersion = order.Variant?.Version;
                orderDto.VariantColor = order.Variant?.Color;
                orderDto.ImageUrl = order.Variant?.VehicleModel?.ImageUrl;
                orderDtos.Add(orderDto);
            }

            return orderDtos;
        }

        public async Task<IEnumerable<OrderDto>> GetOrdersByCustomerAsync(int customerId)
        {
            var orders = await _orderRepository.GetOrdersByCustomerAsync(customerId);
            var orderDtos = new List<OrderDto>();

            foreach (var order in orders)
            {
                var orderDto = _mapper.Map<Order, OrderDto>(order);
                orderDto.ModelName = order.Variant?.VehicleModel?.Name;
                orderDto.ManufacturerName = order.Variant?.VehicleModel?.Manufacturer?.Name;
                orderDto.VariantVersion = order.Variant?.Version;
                orderDto.VariantColor = order.Variant?.Color;
                orderDto.ProductYear = order.Variant?.ProductYear;
                orderDto.Price = order.Variant?.Price;
                orderDto.ImageUrl = order.Variant?.VehicleModel?.ImageUrl;

                // Populate payment information
                var payments = await _paymentRepository.GetPaymentsByOrderAsync(order.OrderId);
                orderDto.Payments = _mapper.MapList<Payment, PaymentDto>(payments).ToList();
                orderDto.TotalPaid = payments.Sum(p => p.Amount);
                var totalAmount = order.TotalPrice ?? (order.Variant?.Price * order.Quantity) ?? 0;
                orderDto.RemainingAmount = Math.Max(0, totalAmount - (orderDto.TotalPaid ?? 0));
                orderDto.IsFullyPaid = orderDto.RemainingAmount <= 0;

                orderDtos.Add(orderDto);
            }

            return orderDtos;
        }

        public async Task<IEnumerable<OrderDto>> GetPendingOrdersByDealerAsync(int dealerId)
        {
            var allOrders = await _orderRepository.GetOrdersByDealerAsync(dealerId);
            var pendingOrders = allOrders.Where(o => o.Status == "Pending");
            return _mapper.MapList<Order, OrderDto>(pendingOrders);
        }

        public async Task<OrderDto> ConfirmOrderAsync(int orderId, string dealerNotes = "")
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order == null)
                throw new ArgumentException("Order not found");

            if (order.Status != "Pending")
                throw new InvalidOperationException("Order is not in pending status");

            order.Status = "Confirmed";
            // Note: You might want to add a DealerNotes field to Order model
            var result = await _orderRepository.UpdateOrderAsync(order);
            return _mapper.Map<Order, OrderDto>(result);
        }

        public async Task<OrderDto> RejectOrderAsync(int orderId, string rejectionReason)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order == null)
                throw new ArgumentException("Order not found");

            if (order.Status != "Pending")
                throw new InvalidOperationException("Order is not in pending status");

            order.Status = "Rejected";
            // Note: You might want to add a RejectionReason field to Order model
            var result = await _orderRepository.UpdateOrderAsync(order);
            return _mapper.Map<Order, OrderDto>(result);
        }

        // Sales Contract Management
        public async Task<SalesContractDto> CreateSalesContractAsync(int orderId, decimal totalAmount, string terms)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order == null)
                throw new ArgumentException("Order not found");

            var contract = new SalesContract
            {
                OrderId = orderId,
                ContractDate = DateOnly.FromDateTime(DateTime.Now),
                TotalAmount = totalAmount,
                Terms = terms,
                Status = "Active"
            };

            // Note: You might need to create SalesContractRepository if not exists
            // For now, we'll assume it's handled through Order navigation property
            order.Status = "Contract Signed";
            await _orderRepository.UpdateOrderAsync(order);

            return _mapper.Map<SalesContract, SalesContractDto>(contract);
        }

        public async Task<SalesContractDto?> GetSalesContractByOrderAsync(int orderId)
        {
            var order = await _orderRepository.GetOrderWithDetailsAsync(orderId);
            var contract = order?.SalesContracts.FirstOrDefault();
            return contract != null ? _mapper.Map<SalesContract, SalesContractDto>(contract) : null;
        }

        // Payment Management
        public async Task<PaymentDto> ProcessPaymentAsync(int orderId, decimal amount, string paymentMethod)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order == null)
                throw new ArgumentException("Order not found");

            // Không set PaymentId - để database tự generate (IDENTITY column)
            var payment = new Payment
            {
                // PaymentId sẽ được database auto-generate
                OrderId = orderId,
                Amount = amount,
                PaymentMethod = paymentMethod,
                PaymentDate = DateTime.Now,
                Status = "Completed"
            };

            var result = await _paymentRepository.CreatePaymentAsync(payment);
            return _mapper.Map<Payment, PaymentDto>(result);
        }

        public async Task<IEnumerable<PaymentDto>> GetPaymentsByOrderAsync(int orderId)
        {
            var payments = await _paymentRepository.GetPaymentsByOrderAsync(orderId);
            return _mapper.MapList<Payment, PaymentDto>(payments);
        }

        public async Task<decimal> GetRemainingBalanceAsync(int orderId)
        {
            var order = await _orderRepository.GetOrderWithDetailsAsync(orderId);
            if (order == null) return 0;

            var totalPaid = await _paymentRepository.GetTotalPaidAmountByOrderAsync(orderId);
            // Dùng TotalPrice hoặc tính toán từ Variant Price * Quantity
            var totalAmount = order.TotalPrice ?? (order.Variant?.Price * order.Quantity) ?? 0;

            return Math.Max(0, totalAmount - totalPaid);
        }

        // Vehicle Delivery
        public async Task<OrderDto> CompleteOrderAsync(int orderId)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order == null)
                throw new ArgumentException("Order not found");

            var remainingBalance = await GetRemainingBalanceAsync(orderId);
            if (remainingBalance > 0)
                throw new InvalidOperationException("Order has outstanding balance");

            order.Status = "Completed";
            var result = await _orderRepository.UpdateOrderAsync(order);
            return _mapper.Map<Order, OrderDto>(result);
        }

        public async Task<bool> IsOrderReadyForDeliveryAsync(int orderId)
        {
            var remainingBalance = await GetRemainingBalanceAsync(orderId);
            return remainingBalance <= 0;
        }

        public async Task<PaymentDto> UpdatePaymentStatusAsync(int paymentId, string status)
        {
            var payment = await _paymentRepository.GetPaymentByIdAsync(paymentId);
            if (payment == null)
                throw new ArgumentException("Payment not found");

            payment.Status = status;
            var result = await _paymentRepository.UpdatePaymentAsync(payment);
            return _mapper.Map<Payment, PaymentDto>(result);
        }
    }
}