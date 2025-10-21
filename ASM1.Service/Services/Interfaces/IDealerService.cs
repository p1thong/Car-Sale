using ASM1.Service.Dtos;

namespace ASM1.Service.Services.Interfaces
{
    public interface IDealerService
    {
        // Dealer Management
        Task<IEnumerable<DealerDto>> GetAllDealersAsync();
        Task<DealerDto?> GetDealerByIdAsync(int dealerId);
        Task<DealerDto?> GetDealerByEmailAsync(string email);
        Task<DealerDto> CreateDealerAsync(DealerDto dealer);
        Task<DealerDto> UpdateDealerAsync(DealerDto dealer);

        // Dealer Contract Management
        Task<IEnumerable<DealerContractDto>> GetDealerContractsAsync(int dealerId);
        Task<DealerContractDto?> GetDealerContractByIdAsync(int contractId);
        Task<DealerContractDto> CreateDealerContractAsync(DealerContractDto contract);
        Task<DealerContractDto> UpdateDealerContractAsync(DealerContractDto contract);

        // Business Logic Methods
        Task<bool> IsDealerActiveAsync(int dealerId);
        Task<IEnumerable<DealerDto>> GetDealersByRegionAsync(string region);
        Task<bool> CanDeleteDealerAsync(int dealerId);
        Task<decimal> GetDealerTotalSalesAsync(int dealerId, DateTime? fromDate = null, DateTime? toDate = null);
    }
}