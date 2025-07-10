using System.Net;
using MXANH.Enums;

namespace MXANH.Models
{
    public class User
    {
        public int Id { get; set; }
        public string PhoneNumber { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public GendersEnum Gender { get; set; }
        public UsersEnum Role { get; set; } = UsersEnum.User; // Default role is User
        public string? AvatarUrl { get; set; } 
        public string? Username { get; set; }
        public string? Password { get; set; }
        public DateOnly? Dob { get; set; }  
        public DateTime? UpdateAt { get; set; }
        public int Points { get; set; } = 0; // Default points is 0
        public decimal Carsh { get; set; } = 0; // Default cash is 0
        public bool IsActive { get; set; } = false; // Default is active
        public DateTime CreatedAt { get; set; }
        public ICollection<Address> Addresses { get; set; }
        public ICollection<CollectionRequest> CollectionRequests { get; set; }
        public ICollection<PointsTransaction> PointsTransactions { get; set; }
        public ICollection<Order> Orders { get; set; }
        public ICollection<UserEvent> UserEvents { get; set; }
    }

}
