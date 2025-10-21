using ASM1.Repository.Models;
using ASM1.Repository.Repositories.Interfaces;
using ASM1.Service.Services.Interfaces;
using ASM1.Service.Dtos;
using ASM1.Service.Mappers;

namespace ASM1.Service.Services
{
    public class DealerService : IDealerService
    {
        private readonly IDealerRepository _dealerRepository;
        private readonly IMapper _mapper;

        public DealerService(IDealerRepository dealerRepository, IMapper mapper)
        {
            _dealerRepository = dealerRepository;
            _mapper = mapper;
        }

        // Dealer Management
        public async Task<IEnumerable<DealerDto>> GetAllDealersAsync()
        {
            var dealers = await _dealerRepository.GetAllDealersAsync();
            return _mapper.MapList<Dealer, DealerDto>(dealers);
        }

        public async Task<DealerDto?> GetDealerByIdAsync(int dealerId)
        {
            var dealer = await _dealerRepository.GetDealerByIdAsync(dealerId);
            return dealer != null ? _mapper.Map<Dealer, DealerDto>(dealer) : null;
        }

        public async Task<DealerDto?> GetDealerByEmailAsync(string email)
        {
            var dealer = await _dealerRepository.GetDealerByEmailAsync(email);
            return dealer != null ? _mapper.Map<Dealer, DealerDto>(dealer) : null;
        }

        public async Task<DealerDto> CreateDealerAsync(DealerDto dealerDto)
        {
            var dealer = _mapper.Map<DealerDto, Dealer>(dealerDto);
            var result = await _dealerRepository.CreateDealerAsync(dealer);
            return _mapper.Map<Dealer, DealerDto>(result);
        }

        public async Task<DealerDto> UpdateDealerAsync(DealerDto dealerDto)
        {
            var dealer = _mapper.Map<DealerDto, Dealer>(dealerDto);
            var result = await _dealerRepository.UpdateDealerAsync(dealer);
            return _mapper.Map<Dealer, DealerDto>(result);
        }

        // Dealer Contract Management
        public async Task<IEnumerable<DealerContractDto>> GetDealerContractsAsync(int dealerId)
        {
            var contracts = await _dealerRepository.GetDealerContractsAsync(dealerId);
            return _mapper.MapList<DealerContract, DealerContractDto>(contracts);
        }

        public async Task<DealerContractDto?> GetDealerContractByIdAsync(int contractId)
        {
            var contract = await _dealerRepository.GetDealerContractByIdAsync(contractId);
            return contract != null ? _mapper.Map<DealerContract, DealerContractDto>(contract) : null;
        }

        public async Task<DealerContractDto> CreateDealerContractAsync(DealerContractDto contractDto)
        {
            var contract = _mapper.Map<DealerContractDto, DealerContract>(contractDto);
            var result = await _dealerRepository.CreateDealerContractAsync(contract);
            return _mapper.Map<DealerContract, DealerContractDto>(result);
        }

        public async Task<DealerContractDto> UpdateDealerContractAsync(DealerContractDto contractDto)
        {
            var contract = _mapper.Map<DealerContractDto, DealerContract>(contractDto);
            var result = await _dealerRepository.UpdateDealerContractAsync(contract);
            return _mapper.Map<DealerContract, DealerContractDto>(result);
        }

        // Business Logic Methods
        public async Task<bool> IsDealerActiveAsync(int dealerId)
        {
            return await _dealerRepository.IsDealerActiveAsync(dealerId);
        }

        public async Task<IEnumerable<DealerDto>> GetDealersByRegionAsync(string region)
        {
            var dealers = await _dealerRepository.GetDealersByRegionAsync(region);
            return _mapper.MapList<Dealer, DealerDto>(dealers);
        }

        public async Task<bool> CanDeleteDealerAsync(int dealerId)
        {
            return await _dealerRepository.CanDeleteDealerAsync(dealerId);
        }

        public async Task<decimal> GetDealerTotalSalesAsync(int dealerId, DateTime? fromDate = null, DateTime? toDate = null)
        {
            return await _dealerRepository.GetDealerTotalSalesAsync(dealerId, fromDate, toDate);
        }
    }
}