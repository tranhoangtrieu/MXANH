namespace MXANH.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public string PaymentMethod { get; set; }
        public decimal Amount { get; set; }
        public DateTime CreatedAt { get; set; }

        public User User { get; set; }
        public MarketplaceProduct Product { get; set; }
    }

}
