using DotNetEnv;
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
        private readonly IWebHostEnvironment _env;
        public MaterialService(IMaterialRepository materialRepository, IWebHostEnvironment env)
        {
            _materialRepository = materialRepository;
            _env = env;
        }

        public async Task<IEnumerable<MaterialResponseDTO>> GetAllActiveMaterialsAsync()
        {
            var materials = await _materialRepository.GetAllActiveMaterialsAsync();
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

        public async Task<IEnumerable<MaterialResponseDTO>> SearchMaterialsAsync(string keyword)
        {
            var materials = await _materialRepository.SearchMaterialsAsync(keyword);
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

        public async Task<IEnumerable<MaterialResponseDTO>> GetMaterialsByCategoryAsync(string category)
        {
            var materials = await _materialRepository.GetMaterialsByCategoryAsync(category);
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

        public async Task<IEnumerable<string>> GetAllCategoriesAsync()
        {
            return await _materialRepository.GetAllCategoriesAsync();
        }

        public async Task<MaterialResponseDTO> AddMaterialAsync(CreateMaterialRequestDTO request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                throw new ArgumentException("Material name cannot be empty", nameof(request.Name));
            if (request.PricePerKg <= 0)
                throw new ArgumentException("Price per kg must be greater than zero", nameof(request.PricePerKg));
            if (string.IsNullOrWhiteSpace(request.Category))
                throw new ArgumentException("Material category cannot be empty", nameof(request.Category));
            if (request.ImageFile == null || request.ImageFile.Length == 0)
                throw new ArgumentException("File cannot be null or empty", nameof(request.ImageFile));
            var folderPath = Path.Combine(_env.WebRootPath, "images", "materials");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            var fileName = $"{Guid.NewGuid()}_{request.ImageFile.FileName}";
            var filePath = Path.Combine(folderPath, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await request.ImageFile.CopyToAsync(stream);
            }
            var imageUrl = $"/images/materials/{fileName}";

            var material = new Material
            {
                Name = request.Name,
                Description = request.Description,
                PricePerKg = request.PricePerKg,
                Category = request.Category,

                ImageUrl = imageUrl,
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

                UpdatedAt = createdMaterial.UpdatedAt

            };
        }

        public async Task<MaterialResponseDTO> UpdateMaterialAsync(int id, UpdateMaterialRequestDTO request)
        {
            var existingMaterial = await _materialRepository.GetMaterialByIdAsync(id);
            if (existingMaterial == null)
                return null;
            if (string.IsNullOrWhiteSpace(request.Name))
                throw new ArgumentException("Material name cannot be empty", nameof(request.Name));
            if (request.PricePerKg <= 0)
                throw new ArgumentException("Price per kg must be greater than zero", nameof(request.PricePerKg));
            if (string.IsNullOrWhiteSpace(request.Category))
                throw new ArgumentException("Material category cannot be empty", nameof(request.Category));
            if (request.ImageFile != null && request.ImageFile.Length > 0)
            {
                var folderPath = Path.Combine(_env.WebRootPath, "images", "materials");
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }
                var fileName = $"{Guid.NewGuid()}_{request.ImageFile.FileName}";
                var filePath = Path.Combine(folderPath, fileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await request.ImageFile.CopyToAsync(stream);
                }
                existingMaterial.ImageUrl = $"/images/materials/{fileName}";
            }

            existingMaterial.Name = request.Name;
            existingMaterial.Description = request.Description;
            existingMaterial.PricePerKg = request.PricePerKg;
            existingMaterial.Category = request.Category;
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

        public async Task<string> UploadMaterialImageAsync(int id, IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File cannot be null or empty", nameof(file));
            var material = await _materialRepository.GetMaterialByIdAsync(id);
            if (material == null)
                throw new KeyNotFoundException($"Material with ID {id} not found");
            var fileName = $"{Guid.NewGuid()}_{file.FileName}";
            var folderPath = Path.Combine(_env.WebRootPath, "images", "materials");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            var filePath = Path.Combine(folderPath, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            var imageUrl = $"/images/materials/{fileName}";
            material.ImageUrl = imageUrl;
            await _materialRepository.UpdateMaterialAsync(material);
            return imageUrl;
        }
    }
}