namespace MXANH.Models
{
    public class Material
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public decimal PricePerKg { get; set; }
        public string Category { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public virtual ICollection<MaterialDetail> Details { get; set; } = new List<MaterialDetail>();
        public ICollection<CollectionRequest> CollectionRequests { get; set; }
    }

}
