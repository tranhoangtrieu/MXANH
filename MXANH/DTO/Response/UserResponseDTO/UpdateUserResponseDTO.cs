namespace MXANH.DTO.Response.UserResponseDTO
{
    public class UpdateUserResponseDTO
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
    }
}
