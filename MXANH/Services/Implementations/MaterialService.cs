using MXANH.DTO.Request.MaterialRequestDTO;
using MXANH.DTO.Response.MaterialResponseDTO;
using MXANH.Models;
using MXANH.Repositories.Interfaces;
using MXANH.Services.Interfaces;

namespace MXANH.Services
{
    public class MaterialService : IMaterialService
    {
        private readonly IMaterialRepository _materialRepository;

        public MaterialService(IMaterialRepository materialRepository)
        {
            _materialRepository = materialRepository;
        }

        public async Task<IEnumerable<MaterialResponseDTO>> GetAllActiveMaterialAsync()
        {
            var materials = await _materialRepository.GetAllActiveMaterialAsync();
            return materials.Select(m => new MaterialResponseDTO
            {
                Id = m.Id,
                Name = m.Name,
                Description = m.Description,
                PricePerKg = m.PricePerKg,
                Category = m.Category,
                ImageUrl = m.ImageUrl,
                CreatedAt = m.CreatedAt,
                UpdatedAt = m.UpdatedAt
            });
        }

        public async Task<MaterialResponseDTO> GetMaterialByIdAsync(int id)
        {
            var material = await _materialRepository.GetMaterialByIdAsync(id);
            if (material == null)
                return null;

            return new MaterialResponseDTO
            {
                Id = material.Id,
                Name = material.Name,
                Description = material.Description,
                PricePerKg = material.PricePerKg,
                Category = material.Category,
                ImageUrl = material.ImageUrl,
                CreatedAt = material.CreatedAt,
                UpdatedAt = material.UpdatedAt
            };
        }

        public async Task<MaterialResponseDTO> AddMaterialAsync(CreateMaterialRequestDTO request)
        {
            var material = new Material
            {
                Name = request.Name,
                Description = request.Description,
                PricePerKg = request.PricePerKg,
                Category = request.Category,
                ImageUrl = request.ImageUrl,
                UpdatedAt = DateTime.UtcNow
            };

            var createdMaterial = await _materialRepository.AddMaterialAsync(material);

            return new MaterialResponseDTO
            {
                Id = createdMaterial.Id,
                Name = createdMaterial.Name,
                Description = createdMaterial.Description,
                PricePerKg = createdMaterial.PricePerKg,
                Category = createdMaterial.Category,
                ImageUrl = createdMaterial.ImageUrl,
                CreatedAt = createdMaterial.CreatedAt,
                UpdatedAt =  createdMaterial.UpdatedAt
            };
        }

        public async Task<MaterialResponseDTO> UpdateMaterialAsync(int id, UpdateMaterialRequestDTO request)
        {
            var existingMaterial = await _materialRepository.GetMaterialByIdAsync(id);
            if (existingMaterial == null)
                return null;

            existingMaterial.Name = request.Name;
            existingMaterial.Description = request.Description;
            existingMaterial.PricePerKg = request.PricePerKg;
            existingMaterial.Category = request.Category;
            existingMaterial.ImageUrl = request.ImageUrl;
            existingMaterial.UpdatedAt = DateTime.UtcNow;

            var updatedMaterial = await _materialRepository.UpdateMaterialAsync(existingMaterial);

            return new MaterialResponseDTO
            {
                Id = updatedMaterial.Id,
                Name = updatedMaterial.Name,
                Description = updatedMaterial.Description,
                PricePerKg = updatedMaterial.PricePerKg,
                Category = updatedMaterial.Category,
                ImageUrl = updatedMaterial.ImageUrl,
                CreatedAt = updatedMaterial.CreatedAt,
                UpdatedAt = updatedMaterial.UpdatedAt
            };
        }

        public async Task<bool> DeleteMaterialAsync(int id)
        {
            return await _materialRepository.DeleteMaterialAsync(id);
        }
    }
}