using MXANH.Models;

namespace MXANH.Repositories.Interfaces
{
    public interface IMaterialRepository
    {
        Task<IEnumerable<Material>> GetAllActiveMaterialsAsync();
        Task<Material> GetMaterialByIdAsync(int id);
        Task<IEnumerable<Material>> SearchMaterialsAsync(string keyword);
        Task<IEnumerable<Material>> GetMaterialsByCategoryAsync(string category);
        Task<IEnumerable<string>> GetAllCategoriesAsync();
        Task<Material> AddMaterialAsync(Material material);
        Task<Material> UpdateMaterialAsync(Material material);
        Task<bool> DeleteMaterialAsync(int id);
    }
}