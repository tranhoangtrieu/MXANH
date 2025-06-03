namespace MXANH.Models
{
    public class UserEvent
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int EventId { get; set; }
        public string Status { get; set; }
        public int PointsUsed { get; set; }
        public DateTime CreatedAt { get; set; }

        public User User { get; set; }
        public Event Event { get; set; }
    }

}
