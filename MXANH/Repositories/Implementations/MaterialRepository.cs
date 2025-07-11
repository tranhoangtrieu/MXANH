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

        public async Task<IEnumerable<Material>> GetAllActiveMaterialsAsync()
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

        public async Task<IEnumerable<Material>> SearchMaterialsAsync(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return await GetAllActiveMaterialsAsync();

            var searchTerm = RemoveDiacritics(keyword.ToLower().Trim());

            return _context.Materials
                .Where(w => w.IsActive)
                .Include(w => w.Details.Where(d => d.IsActive))
                .AsEnumerable() // Switch to client-side for diacritic-insensitive search
                .Where(w =>
                {
                    var name = RemoveDiacritics(w.Name?.ToLower() ?? "");
                    var desc = RemoveDiacritics(w.Description?.ToLower() ?? "");
                    return name.Contains(searchTerm) || desc.Contains(searchTerm);
                })
                .OrderBy(w => w.Name)
                .ToList(); // Use ToList instead of ToListAsync since the query is now client-side
        }

        // Helper method to remove Vietnamese diacritics
        private static string RemoveDiacritics(string text)
        {
            if (string.IsNullOrEmpty(text))
                return text;

            var normalized = text.Normalize(System.Text.NormalizationForm.FormD);
            var sb = new System.Text.StringBuilder();

            foreach (var c in normalized)
            {
                var unicodeCategory = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != System.Globalization.UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(c);
                }
            }
            return sb.ToString().Normalize(System.Text.NormalizationForm.FormC);
        }

        public async Task<IEnumerable<Material>> GetMaterialsByCategoryAsync(string category)
        {
            if (string.IsNullOrWhiteSpace(category))
                return await GetAllActiveMaterialsAsync();

            return await _context.Materials
                .Where(w => w.IsActive && w.Category.ToLower() == category.ToLower())
                .Include(w => w.Details.Where(d => d.IsActive))
                .OrderBy(w => w.Name)
                .ToListAsync();
        }

        public async Task<IEnumerable<string>> GetAllCategoriesAsync()
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