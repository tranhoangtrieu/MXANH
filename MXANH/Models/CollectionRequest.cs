namespace MXANH.Models
{
    public class CollectionRequest
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int MaterialId { get; set; }
        public float Weight { get; set; }
        public string Condition { get; set; }
        public string Status { get; set; }
        public string Image { get; set; }
        public decimal EstimatedEarnings { get; set; }
        public int Points { get; set; }
        public DateTime CreatedAt { get; set; }

        public User User { get; set; }
        public Material Material { get; set; }
    }

}
