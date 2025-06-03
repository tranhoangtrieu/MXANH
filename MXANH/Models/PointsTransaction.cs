namespace MXANH.Models
{
    public class PointsTransaction
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int Points { get; set; }
        public string Type { get; set; }
        public DateTime CreatedAt { get; set; }

        public User User { get; set; }
    }

}
