using System.Drawing;
using MXANH.DTO.Request.UserRequestDTO;
using MXANH.Models;
using MXANH.Repositories.Interfaces;
using MXANH.Services.Interfaces;
namespace MXANH.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        private readonly IPointsTransactionRepository _pointsTransactionReository;

        public UserService(IUserRepository userRepository, IPointsTransactionRepository pointsTransactionReository)
        {
            _userRepository = userRepository;
            _pointsTransactionReository = pointsTransactionReository;
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
        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllUsersAsync();
        }
        public async Task AddUserAsync(CreateUserRequestDTO userRequest)
        {
            var user = new User
            {
                Name = userRequest.Name,
                PhoneNumber = userRequest.PhoneNumber,
                Email = userRequest.Email,
                Username = userRequest.Username,
                Password = userRequest.Password,
                Gender = userRequest.Gender,
                AvatarUrl = userRequest.AvatarUrl,
                CreatedAt = DateTime.UtcNow,
                UpdateAt = DateTime.UtcNow,
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
            // Update the user properties as needed
            existingUser.Name = user.Name;
            existingUser.PhoneNumber = user.PhoneNumber;
            existingUser.Email = user.Email;
            existingUser.Username = user.Username;
            existingUser.AvatarUrl = user.AvatarUrl;
            existingUser.Gender = user.Gender;
            existingUser.Points = user.Points;
            existingUser.UpdateAt = DateTime.UtcNow;


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



    }
}
