namespace MXANH.DTO.Response.MaterialResponseDTO
{
    public class MaterialResponseDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal PricePerKg { get; set; }
        public string Category { get; set; }
        public string ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}