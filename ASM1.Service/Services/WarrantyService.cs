using ASM1.Repository.Models;
using ASM1.Repository.Repositories.Interfaces;
using ASM1.Service.Dtos;
using ASM1.Service.Services.Interfaces;

namespace ASM1.Service.Services
{
    public class WarrantyService : IWarrantyService
    {
        private readonly IWarrantyRepository _warrantyRepository;
        private readonly IOrderRepository _orderRepository;

        public WarrantyService(IWarrantyRepository warrantyRepository, IOrderRepository orderRepository)
        {
            _warrantyRepository = warrantyRepository;
            _orderRepository = orderRepository;
        }

        public async Task<IEnumerable<WarrantyDto>> GetAllWarrantiesAsync()
        {
            var warranties = await _warrantyRepository.GetAllWarrantiesAsync();
            return warranties.Select(MapToDto);
        }

        public async Task<WarrantyDto?> GetWarrantyByIdAsync(int warrantyId)
        {
            var warranty = await _warrantyRepository.GetWarrantyByIdAsync(warrantyId);
            return warranty == null ? null : MapToDto(warranty);
        }

        public async Task<IEnumerable<WarrantyDto>> GetWarrantiesByCustomerIdAsync(int customerId)
        {
            var warranties = await _warrantyRepository.GetWarrantiesByCustomerIdAsync(customerId);
            return warranties.Select(MapToDto);
        }

        public async Task<IEnumerable<WarrantyDto>> GetWarrantiesByDealerIdAsync(int dealerId)
        {
            var warranties = await _warrantyRepository.GetWarrantiesByDealerIdAsync(dealerId);
            return warranties.Select(MapToDto);
        }

        public async Task<IEnumerable<WarrantyDto>> GetWarrantiesByOrderIdAsync(int orderId)
        {
            var warranties = await _warrantyRepository.GetWarrantiesByOrderIdAsync(orderId);
            return warranties.Select(MapToDto);
        }

        public async Task<IEnumerable<WarrantyDto>> GetCustomerOrdersEligibleForWarrantyAsync(int customerId)
        {
            var orders = await _orderRepository.GetOrdersByCustomerAsync(customerId);
            var eligibleOrders = new List<WarrantyDto>();

            foreach (var order in orders)
            {
                // Only completed orders are eligible for warranty
                if (order.Status == "Completed" && order.OrderDate.HasValue)
                {
                    var orderDateTime = order.OrderDate.Value.ToDateTime(TimeOnly.MinValue);
                    var warrantyExpiryDate = orderDateTime.AddYears(3);
                    if (DateTime.Now <= warrantyExpiryDate)
                    {
                        // Check if there's already a warranty request for this order
                        var existingWarranties = await _warrantyRepository.GetWarrantiesByOrderIdAsync(order.OrderId);
                        
                        eligibleOrders.Add(new WarrantyDto
                        {
                            OrderId = order.OrderId,
                            CustomerId = order.CustomerId,
                            DealerId = order.DealerId,
                            VehicleModelName = order.Variant?.VehicleModel?.Name ?? "N/A",
                            VariantVersion = order.Variant?.Version ?? "N/A",
                            OrderDate = orderDateTime,
                            WarrantyExpiryDate = warrantyExpiryDate
                        });
                    }
                }
            }

            return eligibleOrders;
        }

        public async Task<WarrantyDto> CreateWarrantyRequestAsync(WarrantyDto warrantyDto)
        {
            var warranty = new Warranty
            {
                OrderId = warrantyDto.OrderId,
                CustomerId = warrantyDto.CustomerId,
                DealerId = warrantyDto.DealerId,
                WarrantyType = warrantyDto.WarrantyType,
                Reason = warrantyDto.Reason,
                RequestDate = DateTime.Now,
                Status = "Pending"
            };

            var createdWarranty = await _warrantyRepository.CreateWarrantyAsync(warranty);
            return MapToDto(createdWarranty);
        }

        public async Task<WarrantyDto> DealerConfirmWarrantyAsync(int warrantyId, string notes)
        {
            var warranty = await _warrantyRepository.GetWarrantyByIdAsync(warrantyId);
            if (warranty == null)
                throw new Exception("Warranty not found");

            warranty.Status = "DealerConfirmed";
            warranty.DealerConfirmedDate = DateTime.Now;
            warranty.Notes = notes;

            var updatedWarranty = await _warrantyRepository.UpdateWarrantyAsync(warranty);
            return MapToDto(updatedWarranty);
        }

        public async Task<WarrantyDto> CompleteRepairAsync(int warrantyId, string notes)
        {
            var warranty = await _warrantyRepository.GetWarrantyByIdAsync(warrantyId);
            if (warranty == null)
                throw new Exception("Warranty not found");

            warranty.Status = "RepairCompleted";
            warranty.RepairCompletedDate = DateTime.Now;
            warranty.Notes = notes;

            var updatedWarranty = await _warrantyRepository.UpdateWarrantyAsync(warranty);
            return MapToDto(updatedWarranty);
        }

        public async Task<WarrantyDto> CustomerConfirmReceiptAsync(int warrantyId)
        {
            var warranty = await _warrantyRepository.GetWarrantyByIdAsync(warrantyId);
            if (warranty == null)
                throw new Exception("Warranty not found");

            warranty.Status = "CustomerReceived";
            warranty.CustomerReceivedDate = DateTime.Now;

            var updatedWarranty = await _warrantyRepository.UpdateWarrantyAsync(warranty);
            return MapToDto(updatedWarranty);
        }

        public async Task<bool> CanRequestWarrantyAsync(int orderId)
        {
            return await _warrantyRepository.CanRequestWarrantyAsync(orderId);
        }

        private WarrantyDto MapToDto(Warranty warranty)
        {
            return new WarrantyDto
            {
                WarrantyId = warranty.WarrantyId,
                OrderId = warranty.OrderId,
                CustomerId = warranty.CustomerId,
                DealerId = warranty.DealerId,
                WarrantyType = warranty.WarrantyType,
                Reason = warranty.Reason,
                RequestDate = warranty.RequestDate,
                DealerConfirmedDate = warranty.DealerConfirmedDate,
                RepairCompletedDate = warranty.RepairCompletedDate,
                CustomerReceivedDate = warranty.CustomerReceivedDate,
                Status = warranty.Status,
                Notes = warranty.Notes,
                CustomerName = warranty.Customer?.FullName ?? "N/A",
                CustomerEmail = warranty.Customer?.Email ?? "N/A",
                DealerName = warranty.Dealer?.FullName ?? "N/A",
                VehicleModelName = warranty.Order?.Variant?.VehicleModel?.Name ?? "N/A",
                VariantVersion = warranty.Order?.Variant?.Version ?? "N/A",
                OrderDate = warranty.Order?.OrderDate?.ToDateTime(TimeOnly.MinValue),
                WarrantyExpiryDate = warranty.Order?.OrderDate?.ToDateTime(TimeOnly.MinValue).AddYears(3)
            };
        }
    }
}
