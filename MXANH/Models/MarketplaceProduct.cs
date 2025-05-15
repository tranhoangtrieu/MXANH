namespace MXANH.Models
{
    public class MarketplaceProduct
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int PointsCost { get; set; }
        public string Tags { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }

        public ICollection<Order> Orders { get; set; }
    }

}
