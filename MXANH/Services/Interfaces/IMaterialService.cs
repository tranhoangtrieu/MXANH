using MXANH.DTO.Request.MaterialRequestDTO;
using MXANH.DTO.Response.MaterialResponseDTO;

namespace MXANH.Services.Interfaces
{
    public interface IMaterialService
    {
        Task<IEnumerable<MaterialResponseDTO>> GetAllActiveMaterialsAsync();
        Task<MaterialResponseDTO> GetMaterialByIdAsync(int id);
        Task<IEnumerable<MaterialResponseDTO>> SearchMaterialsAsync(string keyword);
        Task<IEnumerable<MaterialResponseDTO>> GetMaterialsByCategoryAsync(string category);
        Task<IEnumerable<string>> GetAllCategoriesAsync();
        Task<MaterialResponseDTO> AddMaterialAsync(CreateMaterialRequestDTO request);
        Task<MaterialResponseDTO> UpdateMaterialAsync(int id, UpdateMaterialRequestDTO request);
        Task<bool> DeleteMaterialAsync(int id);
    }
}