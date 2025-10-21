using ASM1.Service.Dtos;

namespace ASM1.Service.Services.Interfaces
{
    public interface IVehicleService
    {
        // Manufacturer Management
        Task<IEnumerable<ManufacturerDto>> GetAllManufacturersAsync();
        Task<ManufacturerDto?> GetManufacturerByIdAsync(int manufacturerId);
        Task<ManufacturerDto> CreateManufacturerAsync(ManufacturerDto manufacturer);
        Task<ManufacturerDto> UpdateManufacturerAsync(ManufacturerDto manufacturer);

        // Vehicle Model Management
        Task<IEnumerable<VehicleModelDto>> GetAllVehicleModelsAsync();
        Task<VehicleModelDto?> GetVehicleModelByIdAsync(int modelId);
        Task<IEnumerable<VehicleModelDto>> GetVehicleModelsByManufacturerAsync(int manufacturerId);
        Task<VehicleModelDto> CreateVehicleModelAsync(VehicleModelDto model);
        Task<VehicleModelDto> UpdateVehicleModelAsync(VehicleModelDto model);

        // Vehicle Variant Management
        Task<IEnumerable<VehicleVariantDto>> GetAllVehicleVariantsAsync();
        Task<VehicleVariantDto?> GetVehicleVariantByIdAsync(int variantId);
        Task<IEnumerable<VehicleVariantDto>> GetVehicleVariantsByModelAsync(int modelId);
        Task<VehicleVariantDto> CreateVehicleVariantAsync(VehicleVariantDto variant);
        Task<VehicleVariantDto> UpdateVehicleVariantAsync(VehicleVariantDto variant);
        Task DeleteVehicleVariantAsync(int variantId);
        Task<IEnumerable<VehicleVariantDto>> GetAvailableVariantsAsync();

        // Business Logic Methods
        Task<bool> IsManufacturerNameExistsAsync(string name, int? excludeId = null);
        Task<bool> IsVehicleModelNameExistsAsync(
            string name,
            int manufacturerId,
            int? excludeId = null
        );
        Task<bool> CanDeleteManufacturerAsync(int manufacturerId);
        Task DeleteManufacturerAsync(int manufacturerId);
        Task<bool> CanDeleteVehicleModelAsync(int modelId);
        Task DeleteVehicleModelAsync(int modelId);
        Task<IEnumerable<VehicleModelDto>> SearchVehicleModelsAsync(string searchTerm);
        Task<IEnumerable<VehicleVariantDto>> SearchVehicleVariantsAsync(string searchTerm);
    }
}
