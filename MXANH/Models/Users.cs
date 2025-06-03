using System.Net;

namespace MXANH.Models
{
    public class User
    {
        public int Id { get; set; }
        public string PhoneNumber { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
        public string AvatarUrl { get; set; } 
        public string Username { get; set; }
        public string Password { get; set; }
        public DateOnly Dob { get; set; }
        public DateTime UpdateAt { get; set; }
        public int Points { get; set; }
        public DateTime CreatedAt { get; set; }
        public ICollection<Address> Addresses { get; set; }
        public ICollection<CollectionRequest> CollectionRequests { get; set; }
        public ICollection<PointsTransaction> PointsTransactions { get; set; }
        public ICollection<Order> Orders { get; set; }
        public ICollection<UserEvent> UserEvents { get; set; }
    }

}
