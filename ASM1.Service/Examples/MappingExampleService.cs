using ASM1.Repository.Models;
using ASM1.Service.Dtos;
using ASM1.Service.Mappers;

namespace ASM1.Service.Examples
{
    /// <summary>
    /// Example service showing how to use the generic mapper
    /// </summary>
    public class MappingExampleService
    {
        private readonly IMapper _mapper;

        public MappingExampleService(IMapper mapper)
        {
            _mapper = mapper;
        }

        /// <summary>
        /// Convert Customer entity to DTO
        /// </summary>
        public CustomerDto? ConvertToDto(Customer? customer)
        {
            return _mapper.Map<Customer, CustomerDto>(customer);
        }

        /// <summary>
        /// Convert list of customers to DTOs
        /// </summary>
        public IEnumerable<CustomerDto> ConvertToDto(IEnumerable<Customer> customers)
        {
            return _mapper.MapList<Customer, CustomerDto>(customers);
        }

        /// <summary>
        /// Convert VehicleVariant to DTO
        /// </summary>
        public VehicleVariantDto? ConvertToDto(VehicleVariant? variant)
        {
            return _mapper.Map<VehicleVariant, VehicleVariantDto>(variant);
        }

        /// <summary>
        /// Convert Order to DTO
        /// </summary>
        public OrderDto? ConvertToDto(Order? order)
        {
            return _mapper.Map<Order, OrderDto>(order);
        }

        /// <summary>
        /// Convert Payment to DTO
        /// </summary>
        public PaymentDto? ConvertToDto(Payment? payment)
        {
            return _mapper.Map<Payment, PaymentDto>(payment);
        }

        /// <summary>
        /// Convert TestDrive to DTO
        /// </summary>
        public TestDriveDto? ConvertToDto(TestDrive? testDrive)
        {
            return _mapper.Map<TestDrive, TestDriveDto>(testDrive);
        }

        /// <summary>
        /// Convert Dealer to DTO (excludes password)
        /// </summary>
        public DealerDto? ConvertToDto(Dealer? dealer)
        {
            return _mapper.Map<Dealer, DealerDto>(dealer);
        }

        /// <summary>
        /// Example of complex conversion with manual property setting
        /// </summary>
        public CustomerDto? ConvertToDtoWithCustomLogic(Customer? customer)
        {
            if (customer == null) return null;

            var dto = _mapper.Map<Customer, CustomerDto>(customer);
            
            // Add custom logic here if needed
            // Example: Format phone number, calculate age, etc.
            
            return dto;
        }
    }
}