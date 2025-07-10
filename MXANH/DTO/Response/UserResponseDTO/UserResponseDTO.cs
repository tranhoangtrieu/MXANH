using MXANH.Models;
using MXANH.DTO.Response.AddressResponseDTO;
using MXANH.Enums;

namespace MXANH.DTO.Response.UserResponseDTO
{
    public class UserResponseDTO
    {
        public int Id { get; set; }
        public string PhoneNumber { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? AvatarUrl { get; set; }
        public string? Username { get; set; }
        public DateOnly? Dob { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdateAt { get; set; }
        public int Points { get; set; } = 0;
        public decimal Carsh { get; set; } = 0;
        public bool IsActive { get; set; } = false;
        public GendersEnum Gender { get; set; }

        public List<Address> Addresses { get; set; }
    }
}
