using System.ComponentModel.DataAnnotations;

namespace MXANH.DTO.Request.MaterialRequestDTO
{
    public class CreateMaterialRequestDTO
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal PricePerKg { get; set; }
        public string Category { get; set; }
        public IFormFile ImageFile { get; set; }
    }
}