using MXANH.Models;

namespace MXANH.Repositories.Interfaces
{
    public interface IMaterialRepository
    {
        Task<IEnumerable<Material>> GetAllActiveMaterialAsync();
        Task<Material> GetMaterialByIdAsync(int id);
        Task<IEnumerable<Material>> SearchMaterialAsync(string keyword);
        Task<IEnumerable<Material>> GetMaterialByCategoryAsync(string category);
        Task<IEnumerable<string>> GetCategoriesAsync();
        Task<Material> AddMaterialAsync(Material material);
        Task<Material> UpdateMaterialAsync(Material material);
        Task<bool> DeleteMaterialAsync(int id);
    }
}