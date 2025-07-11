using Microsoft.AspNetCore.Mvc;
using MXANH.DTO.Request.MaterialRequestDTO;
using MXANH.Services.Interfaces;

namespace MXANH.Controllers.MaterialControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialsController : ControllerBase
    {
        private readonly IMaterialService _materialService;

        public MaterialsController(IMaterialService materialService)
        {
            _materialService = materialService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllActiveMaterials()
        {
            var materials = await _materialService.GetAllActiveMaterialsAsync();
            return Ok(materials);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMaterialById(int id)
        {
            var material = await _materialService.GetMaterialByIdAsync(id);
            if (material == null)
            {
                return NotFound();
            }
            return Ok(material);
        }

        [HttpGet("search")]
        public async Task<IActionResult> SearchMaterials([FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return BadRequest("Search query cannot be empty");
            }
            var materials = await _materialService.SearchMaterialsAsync(query);
            return Ok(materials);
        }

        [HttpGet("category/{category}")]
        public async Task<IActionResult> GetMaterialsByCategory(string category)
        {
            if (string.IsNullOrWhiteSpace(category))
            {
                return BadRequest("Category cannot be empty");
            }
            var materials = await _materialService.GetMaterialsByCategoryAsync(category);
            return Ok(materials);
        }
        [HttpGet("categories")]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _materialService.GetAllCategoriesAsync();
            return Ok(categories);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMaterial([FromBody] CreateMaterialRequestDTO materialRequest)
        {
            if (materialRequest == null)
            {
                return BadRequest("Material data is null");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdMaterial = await _materialService.AddMaterialAsync(materialRequest);
            return CreatedAtAction(nameof(GetMaterialById), new { id = createdMaterial.Id }, createdMaterial);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMaterial(int id, [FromBody] UpdateMaterialRequestDTO materialRequest)
        {
            if (materialRequest == null)
            {
                return BadRequest("Material data is null");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedMaterial = await _materialService.UpdateMaterialAsync(id, materialRequest);
            if (updatedMaterial == null)
            {
                return NotFound();
            }

            return Ok(updatedMaterial);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMaterial(int id)
        {
            var deleted = await _materialService.DeleteMaterialAsync(id);
            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}