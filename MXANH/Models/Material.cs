namespace MXANH.Models
{
    public class Material
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal PricePerKg { get; set; }
        public string ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }

        public ICollection<CollectionRequest> CollectionRequests { get; set; }
    }

}
