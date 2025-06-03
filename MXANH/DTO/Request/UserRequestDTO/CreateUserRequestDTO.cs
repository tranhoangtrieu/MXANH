using System.ComponentModel.DataAnnotations;

namespace MXANH.DTO.Request.UserRequestDTO
{
    public class CreateUserRequestDTO
    {
        public string Name { get; set; }
        public string PhoneNumber { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Gender { get; set; }
        public DateOnly Dob { get; set; }

    }
}
