using System.Drawing;
using System.Text.RegularExpressions;
using MXANH.DTO.Request.UserRequestDTO;
using MXANH.DTO.Response.UserResponseDTO;
using MXANH.Models;
using MXANH.Repositories.Interfaces;
using MXANH.Services.Interfaces;
namespace MXANH.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IWebHostEnvironment _env;

        private readonly IUserRepository _userRepository;

        private readonly IPointsTransactionRepository _pointsTransactionReository;

        public UserService(IUserRepository userRepository, IPointsTransactionRepository pointsTransactionReository, IWebHostEnvironment env)
        {
            _userRepository = userRepository;
            _pointsTransactionReository = pointsTransactionReository;
            _env = env;
        }
        public async Task<UserResponseDTO> GetUserResponseDTOByIdAsync(int id)
        {
            return await _userRepository.GetUserResponseDTOByIdAsync(id);
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetUserByIdAsync(id);
        }

        public async Task<User> GetUserByPhoneNumberAsync(string phoneNumber)
        {
            return await _userRepository.GetUserByPhoneNumberAsync(phoneNumber);
        }
        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _userRepository.GetUserByEmailAsync(email);
        }
        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await _userRepository.GetUserByUsernameAsync(username);
        }
        public async Task<IEnumerable<UserResponseDTO>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllUsersAsync();
        }
        public async Task AddUserAsync(CreateUserRequestDTO userRequest)
        {
            if (userRequest.Dob > DateOnly.FromDateTime(DateTime.UtcNow))
            {
                throw new Exception("Date of birth cannot be in the future");
            }
            var phoneRegex = new Regex(@"^(0[3|5|7|8|9])[0-9]{8}$");
            if (!phoneRegex.IsMatch(userRequest.PhoneNumber))
            {
                throw new Exception("Invalid Vietnamese phone number");
            }


            var user = new User
            {
                Name = userRequest.Name,
                PhoneNumber = userRequest.PhoneNumber,
                Email = userRequest.Email,
                Username = userRequest.Username,
                Password = BCrypt.Net.BCrypt.HashPassword(userRequest.Password),
                AvatarUrl = "/images/avatars/8226745b-8f08-48cc-a1e3-75c5fbb4f07c.png",
                Gender = userRequest.Gender,
                Points = 0,
                CreatedAt = DateTime.UtcNow,
                UpdateAt = DateTime.UtcNow,
                Dob = userRequest.Dob,
            };

            await _userRepository.AddUserAsync(user);
        }
        public async Task UpdateUserAsync(int id, UpdateUserRequestDTO user)
        {


            var existingUser = await _userRepository.GetUserByIdAsync(id);
            if (existingUser == null)
            {
                throw new Exception("User not found");
            }

            if (existingUser.Dob > DateOnly.FromDateTime(DateTime.UtcNow))
            {
                throw new Exception("Date of birth cannot be in the future");
            }

            // Update the user properties as needed
            existingUser.Name = user.Name;
            existingUser.PhoneNumber = user.PhoneNumber;
            existingUser.Email = user.Email;
            existingUser.Username = user.Username;
            existingUser.AvatarUrl = user.AvatarUrl;
            existingUser.Gender = user.Gender;
            existingUser.UpdateAt = DateTime.UtcNow;
            existingUser.Dob = user.Dob;

            await _userRepository.UpdateUserAsync(existingUser);
        }
        public async Task DeleteUserAsync(int id)
        {
            await _userRepository.DeleteUserAsync(id);
        }

        public async Task<int> GetBalance(int id)
        {
            return await _userRepository.GetBalance(id);
        }

        public async Task WithdrawPoints(int id, WithdrawRequest request)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
            {
                throw new Exception("User not found");
            }
            if (user.Points < request.PointsToWithdraw)
            {
                throw new Exception("Insufficient points");
            }
            user.Points -= request.PointsToWithdraw;
            var transaction = new PointsTransaction
            {
                UserId = id,
                Points = -request.PointsToWithdraw,
                CreatedAt = DateTime.UtcNow,
                Type = "withdrawal",

            };
            await _pointsTransactionReository.AddTransactionAsync(transaction);
            await _userRepository.UpdateUserAsync(user);
        }


        public async Task<string> UploadAvatarAsync(int userId, IFormFile avatarFile)
        {


            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null) throw new Exception("User not found");

            var fileName = Guid.NewGuid() + Path.GetExtension(avatarFile.FileName);
            var folderPath = Path.Combine(_env.WebRootPath, "images", "avatars");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var filePath = Path.Combine(folderPath, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await avatarFile.CopyToAsync(stream);
            }

            var relativeUrl = $"/images/avatars/{fileName}";
            user.AvatarUrl = relativeUrl;
            await _userRepository.UpdateUserAsync(user);

            return relativeUrl;
        }


    }
}
