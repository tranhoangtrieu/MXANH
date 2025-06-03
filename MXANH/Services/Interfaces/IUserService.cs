using MXANH.DTO.Request.UserRequestDTO;
using MXANH.Models;

namespace MXANH.Services.Interfaces
{
    public interface IUserService
    {
        Task<User> GetUserByIdAsync(int id);
        Task<User> GetUserByPhoneNumberAsync(string phoneNumber);
        Task<User> GetUserByEmailAsync(string email);
        Task<User> GetUserByUsernameAsync(string username);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task AddUserAsync(CreateUserRequestDTO user);
        Task UpdateUserAsync(int id ,UpdateUserRequestDTO user);
        Task DeleteUserAsync(int id);

        Task<int> GetBalance(int id);

        Task WithdrawPoints(int id, WithdrawRequest request);

        Task<string> UploadAvatarAsync(int userId, IFormFile avatarFile);
    }
}
