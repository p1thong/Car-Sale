using ASM1.Service.Dtos;

namespace ASM1.Service.Services.Interfaces
{
    public interface IWarrantyService
    {
        Task<IEnumerable<WarrantyDto>> GetAllWarrantiesAsync();
        Task<WarrantyDto?> GetWarrantyByIdAsync(int warrantyId);
        Task<IEnumerable<WarrantyDto>> GetWarrantiesByCustomerIdAsync(int customerId);
        Task<IEnumerable<WarrantyDto>> GetWarrantiesByDealerIdAsync(int dealerId);
        Task<IEnumerable<WarrantyDto>> GetWarrantiesByOrderIdAsync(int orderId);
        Task<IEnumerable<WarrantyDto>> GetCustomerOrdersEligibleForWarrantyAsync(int customerId);
        Task<WarrantyDto> CreateWarrantyRequestAsync(WarrantyDto warrantyDto);
        Task<WarrantyDto> DealerConfirmWarrantyAsync(int warrantyId, string notes);
        Task<WarrantyDto> CompleteRepairAsync(int warrantyId, string notes);
        Task<WarrantyDto> CustomerConfirmReceiptAsync(int warrantyId);
        Task<bool> CanRequestWarrantyAsync(int orderId);
    }
}
