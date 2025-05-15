namespace MXANH.DTO.Request.UserRequestDTO
{
    public class UpdateUserRequestDTO
    {
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string AvatarUrl { get; set; }
        public string Gender { get; set; }
        public int Points { get; set; }
    }
}
