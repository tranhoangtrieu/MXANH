using MXANH.DTO.Request.MaterialRequestDTO;
using MXANH.DTO.Response.MaterialResponseDTO;

namespace MXANH.Services.Interfaces
{
    public interface IMaterialService
    {
        Task<IEnumerable<MaterialResponseDTO>> GetAllActiveMaterialAsync();
        Task<MaterialResponseDTO> GetMaterialByIdAsync(int id);
        Task<MaterialResponseDTO> AddMaterialAsync(CreateMaterialRequestDTO request);
        Task<MaterialResponseDTO> UpdateMaterialAsync(int id, UpdateMaterialRequestDTO request);
        Task<bool> DeleteMaterialAsync(int id);
    }
}