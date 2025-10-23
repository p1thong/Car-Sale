using ASM1.Repository.Models;

namespace ASM1.Repository.Repositories.Interfaces
{
    public interface IWarrantyRepository
    {
        Task<IEnumerable<Warranty>> GetAllWarrantiesAsync();
        Task<Warranty?> GetWarrantyByIdAsync(int warrantyId);
        Task<IEnumerable<Warranty>> GetWarrantiesByCustomerIdAsync(int customerId);
        Task<IEnumerable<Warranty>> GetWarrantiesByDealerIdAsync(int dealerId);
        Task<IEnumerable<Warranty>> GetWarrantiesByOrderIdAsync(int orderId);
        Task<Warranty> CreateWarrantyAsync(Warranty warranty);
        Task<Warranty> UpdateWarrantyAsync(Warranty warranty);
        Task<bool> DeleteWarrantyAsync(int warrantyId);
        Task<bool> CanRequestWarrantyAsync(int orderId);
    }
}
