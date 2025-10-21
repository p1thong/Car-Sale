using ASM1.Repository.Models;
using ASM1.Repository.Repositories.Interfaces;
using ASM1.Service.Services.Interfaces;
using ASM1.Service.Dtos;
using ASM1.Service.Mappers;
using Microsoft.EntityFrameworkCore;

namespace ASM1.Service.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IMapper _mapper;

        public VehicleService(IVehicleRepository vehicleRepository, IMapper mapper)
        {
            _vehicleRepository = vehicleRepository;
            _mapper = mapper;
        }

        // Manufacturer Management
        public async Task<IEnumerable<ManufacturerDto>> GetAllManufacturersAsync()
        {
            var manufacturers = await _vehicleRepository.GetAllManufacturersAsync();
            return _mapper.MapList<Manufacturer, ManufacturerDto>(manufacturers);
        }

        public async Task<ManufacturerDto?> GetManufacturerByIdAsync(int manufacturerId)
        {
            var manufacturer = await _vehicleRepository.GetManufacturerByIdAsync(manufacturerId);
            return manufacturer != null ? _mapper.Map<Manufacturer, ManufacturerDto>(manufacturer) : null;
        }

        public async Task<ManufacturerDto> CreateManufacturerAsync(ManufacturerDto manufacturerDto)
        {
            var manufacturer = _mapper.Map<ManufacturerDto, Manufacturer>(manufacturerDto);
            
            // Validate manufacturer name uniqueness
            if (await IsManufacturerNameExistsAsync(manufacturer.Name))
            {
                throw new InvalidOperationException($"Manufacturer with name '{manufacturer.Name}' already exists");
            }

            var result = await _vehicleRepository.CreateManufacturerAsync(manufacturer);
            return _mapper.Map<Manufacturer, ManufacturerDto>(result);
        }

        public async Task<ManufacturerDto> UpdateManufacturerAsync(ManufacturerDto manufacturerDto)
        {
            var manufacturer = _mapper.Map<ManufacturerDto, Manufacturer>(manufacturerDto);
            
            // Validate manufacturer name uniqueness (excluding current manufacturer)
            if (await IsManufacturerNameExistsAsync(manufacturer.Name, manufacturer.ManufacturerId))
            {
                throw new InvalidOperationException($"Manufacturer with name '{manufacturer.Name}' already exists");
            }

            var result = await _vehicleRepository.UpdateManufacturerAsync(manufacturer);
            return _mapper.Map<Manufacturer, ManufacturerDto>(result);
        }

        // Vehicle Model Management
        public async Task<IEnumerable<VehicleModelDto>> GetAllVehicleModelsAsync()
        {
            var models = await _vehicleRepository.GetAllVehicleModelsAsync();
            return _mapper.MapList<VehicleModel, VehicleModelDto>(models);
        }

        public async Task<VehicleModelDto?> GetVehicleModelByIdAsync(int modelId)
        {
            var model = await _vehicleRepository.GetVehicleModelByIdAsync(modelId);
            return model != null ? _mapper.Map<VehicleModel, VehicleModelDto>(model) : null;
        }

        public async Task<IEnumerable<VehicleModelDto>> GetVehicleModelsByManufacturerAsync(int manufacturerId)
        {
            var models = await _vehicleRepository.GetVehicleModelsByManufacturerAsync(manufacturerId);
            return _mapper.MapList<VehicleModel, VehicleModelDto>(models);
        }

        public async Task<VehicleModelDto> CreateVehicleModelAsync(VehicleModelDto modelDto)
        {
            var model = _mapper.Map<VehicleModelDto, VehicleModel>(modelDto);
            
            // Validate model name uniqueness within manufacturer
            if (await IsVehicleModelNameExistsAsync(model.Name, model.ManufacturerId))
            {
                throw new InvalidOperationException($"Vehicle model with name '{model.Name}' already exists for this manufacturer");
            }

            var result = await _vehicleRepository.CreateVehicleModelAsync(model);
            return _mapper.Map<VehicleModel, VehicleModelDto>(result);
        }

        public async Task<VehicleModelDto> UpdateVehicleModelAsync(VehicleModelDto modelDto)
        {
            var model = _mapper.Map<VehicleModelDto, VehicleModel>(modelDto);
            
            // Validate model name uniqueness within manufacturer (excluding current model)
            if (await IsVehicleModelNameExistsAsync(model.Name, model.ManufacturerId, model.VehicleModelId))
            {
                throw new InvalidOperationException($"Vehicle model with name '{model.Name}' already exists for this manufacturer");
            }

            var result = await _vehicleRepository.UpdateVehicleModelAsync(model);
            return _mapper.Map<VehicleModel, VehicleModelDto>(result);
        }

        // Vehicle Variant Management
        public async Task<IEnumerable<VehicleVariantDto>> GetAllVehicleVariantsAsync()
        {
            var variants = await _vehicleRepository.GetAllVehicleVariantsAsync();
            return _mapper.MapList<VehicleVariant, VehicleVariantDto>(variants);
        }

        public async Task<VehicleVariantDto?> GetVehicleVariantByIdAsync(int variantId)
        {
            var variant = await _vehicleRepository.GetVehicleVariantByIdAsync(variantId);
            return variant != null ? _mapper.Map<VehicleVariant, VehicleVariantDto>(variant) : null;
        }

        public async Task<IEnumerable<VehicleVariantDto>> GetVehicleVariantsByModelAsync(int modelId)
        {
            var variants = await _vehicleRepository.GetVehicleVariantsByModelAsync(modelId);
            return _mapper.MapList<VehicleVariant, VehicleVariantDto>(variants);
        }

        public async Task<VehicleVariantDto> CreateVehicleVariantAsync(VehicleVariantDto variantDto)
        {
            var variant = _mapper.Map<VehicleVariantDto, VehicleVariant>(variantDto);
            var result = await _vehicleRepository.CreateVehicleVariantAsync(variant);
            return _mapper.Map<VehicleVariant, VehicleVariantDto>(result);
        }

        public async Task<VehicleVariantDto> UpdateVehicleVariantAsync(VehicleVariantDto variantDto)
        {
            var variant = _mapper.Map<VehicleVariantDto, VehicleVariant>(variantDto);
            var result = await _vehicleRepository.UpdateVehicleVariantAsync(variant);
            return _mapper.Map<VehicleVariant, VehicleVariantDto>(result);
        }

        public async Task DeleteVehicleVariantAsync(int variantId)
        {
            await _vehicleRepository.DeleteVehicleVariantAsync(variantId);
        }

        public async Task<IEnumerable<VehicleVariantDto>> GetAvailableVariantsAsync()
        {
            var variants = await _vehicleRepository.GetAvailableVariantsAsync();
            return _mapper.MapList<VehicleVariant, VehicleVariantDto>(variants);
        }

        // Business Logic Methods
        public async Task<bool> IsManufacturerNameExistsAsync(string name, int? excludeId = null)
        {
            return await _vehicleRepository.IsManufacturerNameExistsAsync(name, excludeId);
        }

        public async Task<bool> IsVehicleModelNameExistsAsync(string name, int manufacturerId, int? excludeId = null)
        {
            return await _vehicleRepository.IsVehicleModelNameExistsAsync(name, manufacturerId, excludeId);
        }

        public async Task<bool> CanDeleteManufacturerAsync(int manufacturerId)
        {
            return await _vehicleRepository.CanDeleteManufacturerAsync(manufacturerId);
        }

        public async Task DeleteManufacturerAsync(int manufacturerId)
        {
            if (!await CanDeleteManufacturerAsync(manufacturerId))
            {
                throw new InvalidOperationException("Cannot delete manufacturer because it has associated vehicle models");
            }

            await _vehicleRepository.DeleteManufacturerAsync(manufacturerId);
        }

        public async Task<bool> CanDeleteVehicleModelAsync(int modelId)
        {
            return await _vehicleRepository.CanDeleteVehicleModelAsync(modelId);
        }

        public async Task DeleteVehicleModelAsync(int modelId)
        {
            if (!await CanDeleteVehicleModelAsync(modelId))
            {
                throw new InvalidOperationException("Cannot delete vehicle model because it has associated variants");
            }

            await _vehicleRepository.DeleteVehicleModelAsync(modelId);
        }

        public async Task<IEnumerable<VehicleModelDto>> SearchVehicleModelsAsync(string searchTerm)
        {
            var models = await _vehicleRepository.SearchVehicleModelsAsync(searchTerm);
            return _mapper.MapList<VehicleModel, VehicleModelDto>(models);
        }

        public async Task<IEnumerable<VehicleVariantDto>> SearchVehicleVariantsAsync(string searchTerm)
        {
            var variants = await _vehicleRepository.SearchVehicleVariantsAsync(searchTerm);
            return _mapper.MapList<VehicleVariant, VehicleVariantDto>(variants);
        }
    }
}