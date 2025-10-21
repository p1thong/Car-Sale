using ASM1.Repository.Models;
using ASM1.Repository.Repositories.Interfaces;
using ASM1.Service.Services.Interfaces;
using ASM1.Service.Dtos;
using ASM1.Service.Mappers;

namespace ASM1.Service.Services
{
    public class CustomerRelationshipService : ICustomerRelationshipService
    {
        private readonly ITestDriveRepository _testDriveRepository;
        private readonly IFeedbackRepository _feedbackRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IMapper _mapper;

        public CustomerRelationshipService(
            ITestDriveRepository testDriveRepository,
            IFeedbackRepository feedbackRepository,
            ICustomerRepository customerRepository,
            IOrderRepository orderRepository,
            IPaymentRepository paymentRepository,
            IMapper mapper)
        {
            _testDriveRepository = testDriveRepository;
            _feedbackRepository = feedbackRepository;
            _customerRepository = customerRepository;
            _orderRepository = orderRepository;
            _paymentRepository = paymentRepository;
            _mapper = mapper;
        }

        // Customer Management
        public async Task<IEnumerable<CustomerDto>> GetCustomersByDealerAsync(int dealerId)
        {
            var customers = await _customerRepository.GetCustomersByDealerAsync(dealerId);
            return _mapper.MapList<Customer, CustomerDto>(customers);
        }

        public async Task<CustomerDto?> GetCustomerByIdAsync(int customerId)
        {
            var customer = await _customerRepository.GetCustomerByIdAsync(customerId);
            return customer != null ? _mapper.Map<Customer, CustomerDto>(customer) : null;
        }

        public async Task<CustomerDto?> GetCustomerByEmailAsync(string email)
        {
            var customer = await _customerRepository.GetCustomerByEmailAsync(email);
            return customer != null ? _mapper.Map<Customer, CustomerDto>(customer) : null;
        }

        public async Task<CustomerDto> CreateCustomerAsync(CustomerDto customerDto)
        {
            var customer = _mapper.Map<CustomerDto, Customer>(customerDto);
            var result = await _customerRepository.CreateCustomerAsync(customer);
            return _mapper.Map<Customer, CustomerDto>(result);
        }

        public async Task<CustomerDto> UpdateCustomerAsync(CustomerDto customerDto)
        {
            var customer = _mapper.Map<CustomerDto, Customer>(customerDto);
            var result = await _customerRepository.UpdateCustomerAsync(customer);
            return _mapper.Map<Customer, CustomerDto>(result);
        }

        // Test Drive Management
        public async Task<TestDriveDto> ScheduleTestDriveAsync(int customerId, int variantId, DateOnly scheduledDate, TimeOnly? scheduledTime = null)
        {
            var customer = await _customerRepository.GetCustomerByIdAsync(customerId);
            if (customer == null)
                throw new ArgumentException("Customer not found");

            var testDrive = new TestDrive
            {
                CustomerId = customerId,
                VariantId = variantId,
                ScheduledDate = scheduledDate,
                ScheduledTime = scheduledTime,
                Status = "Scheduled",
            };

            var result = await _testDriveRepository.CreateTestDriveAsync(testDrive);
            return _mapper.Map<TestDrive, TestDriveDto>(result);
        }

        public async Task<TestDriveDto> ConfirmTestDriveAsync(int testDriveId)
        {
            var testDrive = await _testDriveRepository.GetTestDriveByIdAsync(testDriveId);
            if (testDrive == null)
                throw new ArgumentException("Test drive not found");

            testDrive.Status = "Confirmed";
            var result = await _testDriveRepository.UpdateTestDriveAsync(testDrive);
            return _mapper.Map<TestDrive, TestDriveDto>(result);
        }

        public async Task<TestDriveDto> CompleteTestDriveAsync(int testDriveId)
        {
            var testDrive = await _testDriveRepository.GetTestDriveByIdAsync(testDriveId);
            if (testDrive == null)
                throw new ArgumentException("Test drive not found");

            testDrive.Status = "Completed";
            var result = await _testDriveRepository.UpdateTestDriveAsync(testDrive);
            return _mapper.Map<TestDrive, TestDriveDto>(result);
        }

        public async Task<TestDriveDto?> GetTestDriveByIdAsync(int testDriveId)
        {
            var testDrive = await _testDriveRepository.GetTestDriveByIdAsync(testDriveId);
            return testDrive != null ? _mapper.Map<TestDrive, TestDriveDto>(testDrive) : null;
        }

        public async Task<IEnumerable<TestDriveDto>> GetTestDriveScheduleAsync(DateOnly date)
        {
            var testDrives = await _testDriveRepository.GetScheduledTestDrivesAsync(date);
            return _mapper.MapList<TestDrive, TestDriveDto>(testDrives);
        }

        public async Task<IEnumerable<TestDriveDto>> GetAllTestDrivesAsync()
        {
            var testDrives = await _testDriveRepository.GetAllTestDrivesAsync();
            return _mapper.MapList<TestDrive, TestDriveDto>(testDrives);
        }

        public async Task<IEnumerable<TestDriveDto>> GetCustomerTestDrivesAsync(int customerId)
        {
            var testDrives = await _testDriveRepository.GetTestDrivesByCustomerAsync(customerId);
            return _mapper.MapList<TestDrive, TestDriveDto>(testDrives);
        }

        public async Task<IEnumerable<TestDriveDto>> GetTestDrivesByDealerAsync(int dealerId)
        {
            var testDrives = await _testDriveRepository.GetTestDrivesByDealerAsync(dealerId);
            return _mapper.MapList<TestDrive, TestDriveDto>(testDrives);
        }

        public async Task<TestDriveDto> CreateTestDriveAsync(TestDriveDto testDriveDto)
        {
            var testDrive = _mapper.Map<TestDriveDto, TestDrive>(testDriveDto);
            var result = await _testDriveRepository.CreateTestDriveAsync(testDrive);
            return _mapper.Map<TestDrive, TestDriveDto>(result);
        }

        public async Task UpdateTestDriveStatusAsync(int testDriveId, string status)
        {
            var testDrive = await _testDriveRepository.GetTestDriveByIdAsync(testDriveId);
            if (testDrive != null)
            {
                testDrive.Status = status;
                await _testDriveRepository.UpdateTestDriveAsync(testDrive);
            }
        }

        // Feedback Management
        public async Task<FeedbackDto> CreateFeedbackAsync(int customerId, string content, int rating)
        {
            var feedback = new Feedback
            {
                CustomerId = customerId,
                Content = content,
                Rating = rating,
                FeedbackDate = DateTime.Now
            };

            var result = await _feedbackRepository.CreateFeedbackAsync(feedback);
            return _mapper.Map<Feedback, FeedbackDto>(result);
        }

        public async Task<IEnumerable<FeedbackDto>> GetCustomerFeedbacksAsync(int customerId)
        {
            var feedbacks = await _feedbackRepository.GetFeedbacksByCustomerAsync(customerId);
            return _mapper.MapList<Feedback, FeedbackDto>(feedbacks);
        }

        public async Task<IEnumerable<FeedbackDto>> GetAllFeedbacksAsync()
        {
            var feedbacks = await _feedbackRepository.GetAllFeedbacksAsync();
            return _mapper.MapList<Feedback, FeedbackDto>(feedbacks);
        }

        public async Task<IEnumerable<FeedbackDto>> GetFeedbacksByDealerAsync(int dealerId)
        {
            var feedbacks = await _feedbackRepository.GetFeedbacksByDealerAsync(dealerId);
            return _mapper.MapList<Feedback, FeedbackDto>(feedbacks);
        }

        // Customer Profile & History
        public async Task<CustomerProfileDto> GetCustomerProfileAsync(int customerId)
        {
            var customer = await _customerRepository.GetCustomerByIdAsync(customerId);
            if (customer == null)
                throw new ArgumentException("Customer not found");

            var orders = await _orderRepository.GetOrdersByCustomerAsync(customerId);
            var testDrives = await _testDriveRepository.GetTestDrivesByCustomerAsync(customerId);
            var feedbacks = await _feedbackRepository.GetFeedbacksByCustomerAsync(customerId);

            var totalPurchaseAmount = orders.Sum(o => o.TotalPrice ?? (o.Variant?.Price * o.Quantity) ?? 0);
            var outstandingBalance = orders.Where(o => o.Status != "Completed" && o.Status != "Cancelled")
                .Sum(o => (o.TotalPrice ?? (o.Variant?.Price * o.Quantity) ?? 0) - 
                         (o.Payments?.Sum(p => p.Amount) ?? 0));

            return new CustomerProfileDto
            {
                Customer = _mapper.Map<Customer, CustomerDto>(customer),
                Orders = _mapper.MapList<Order, OrderDto>(orders),
                TestDrives = _mapper.MapList<TestDrive, TestDriveDto>(testDrives),
                Feedbacks = _mapper.MapList<Feedback, FeedbackDto>(feedbacks),
                TotalPurchaseAmount = totalPurchaseAmount,
                OutstandingBalance = outstandingBalance
            };
        }

        public async Task<IEnumerable<OrderDto>> GetCustomerOrderHistoryAsync(int customerId)
        {
            var orders = await _orderRepository.GetOrdersByCustomerAsync(customerId);
            return _mapper.MapList<Order, OrderDto>(orders);
        }

        public async Task<IEnumerable<PaymentDto>> GetCustomerPaymentHistoryAsync(int customerId)
        {
            var orders = await _orderRepository.GetOrdersByCustomerAsync(customerId);
            var allPayments = new List<Payment>();
            
            foreach (var order in orders)
            {
                var payments = await _paymentRepository.GetPaymentsByOrderAsync(order.OrderId);
                allPayments.AddRange(payments);
            }

            return _mapper.MapList<Payment, PaymentDto>(allPayments);
        }

        public async Task<decimal> GetCustomerOutstandingBalanceAsync(int customerId)
        {
            var orders = await _orderRepository.GetOrdersByCustomerAsync(customerId);
            return orders.Where(o => o.Status != "Completed" && o.Status != "Cancelled")
                .Sum(o => (o.TotalPrice ?? (o.Variant?.Price * o.Quantity) ?? 0) - 
                         (o.Payments?.Sum(p => p.Amount) ?? 0));
        }

        // Customer Care & Promotions
        public async Task<IEnumerable<PromotionDto>> GetCustomerEligiblePromotionsAsync(int customerId)
        {
            // Simplified - in real implementation, this would have business logic for eligibility
            var allPromotions = new List<Promotion>(); // This would come from a PromotionRepository
            return _mapper.MapList<Promotion, PromotionDto>(allPromotions);
        }

        public async Task<CustomerCareReportDto> GenerateCustomerCareReportAsync(int dealerId, DateTime fromDate, DateTime toDate)
        {
            var allCustomers = await _customerRepository.GetCustomersByDealerAsync(dealerId);
            var allTestDrives = await _testDriveRepository.GetTestDrivesByDealerAsync(dealerId);
            var allFeedbacks = await _feedbackRepository.GetFeedbacksByDealerAsync(dealerId);

            var newCustomers = allCustomers.Where(c => c.DealerId == dealerId); // Add date filtering in real implementation

            return new CustomerCareReportDto
            {
                TotalCustomers = allCustomers.Count(),
                TotalTestDrives = allTestDrives.Count(),
                TotalFeedbacks = allFeedbacks.Count(),
                AverageRating = allFeedbacks.Any() ? allFeedbacks.Average(f => f.Rating) : 0,
                TotalSales = 0, // Calculate from orders
                NewCustomers = _mapper.MapList<Customer, CustomerDto>(newCustomers),
                RecentFeedbacks = _mapper.MapList<Feedback, FeedbackDto>(allFeedbacks.OrderByDescending(f => f.FeedbackDate).Take(10))
            };
        }
    }
}