using Microsoft.EntityFrameworkCore;
using MXANH.Models;
using MXANH.Repositories.Interfaces;

namespace MXANH.Repositories
{
    public class MaterialRepository : IMaterialRepository
    {
        private readonly AppDbContext _context;

        public MaterialRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Material>> GetAllActiveMaterialAsync()
        {
            return await _context.Materials
                .Where(w => w.IsActive)
                .Include(w => w.Details.Where(d => d.IsActive))
                .OrderBy(w => w.Category)
                .ThenBy(w => w.Name)
                .ToListAsync();
        }

        public async Task<Material?> GetMaterialByIdAsync(int id)
        {
            return await _context.Materials
                .Include(w => w.Details.Where(d => d.IsActive))
                .FirstOrDefaultAsync(w => w.Id == id && w.IsActive);
        }

        public async Task<IEnumerable<Material>> SearchMaterialAsync(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return await GetAllActiveMaterialAsync();

            var searchTerm = keyword.ToLower().Trim();

            return await _context.Materials
                .Where(w => w.IsActive &&
                           (w.Name.ToLower().Contains(searchTerm) ||
                            (w.Description != null && w.Description.ToLower().Contains(searchTerm))))
                .Include(w => w.Details.Where(d => d.IsActive))
                .OrderBy(w => w.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<Material>> GetMaterialByCategoryAsync(string category)
        {
            if (string.IsNullOrWhiteSpace(category))
                return await GetAllActiveMaterialAsync();

            return await _context.Materials
                .Where(w => w.IsActive && w.Category.ToLower() == category.ToLower())
                .Include(w => w.Details.Where(d => d.IsActive))
                .OrderBy(w => w.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<string>> GetCategoriesAsync()
        {
            return await _context.Materials
                .Where(w => w.IsActive)
                .Select(w => w.Category)
                .Distinct()
                .OrderBy(c => c)
                .ToListAsync();
        }

        public async Task<Material> AddMaterialAsync(Material material)
        {
            material.CreatedAt = DateTime.UtcNow;
            material.UpdatedAt = DateTime.UtcNow;

            _context.Materials.Add(material);
            await _context.SaveChangesAsync();
            return material;
        }

        public async Task<Material> UpdateMaterialAsync(Material material)
        {
            material.UpdatedAt = DateTime.UtcNow;

            _context.Entry(material).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return material;
        }

        public async Task<bool> DeleteMaterialAsync(int id)
        {
            var material = await _context.Materials.FindAsync(id);
            if (material == null) return false;

            material.IsActive = false;
            material.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}