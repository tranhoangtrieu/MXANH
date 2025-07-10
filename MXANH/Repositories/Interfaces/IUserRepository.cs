using MXANH.DTO.Response.UserResponseDTO;
using MXANH.Models;

namespace MXANH.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<UserResponseDTO> GetUserResponseDTOByIdAsync(int id);
        Task<User> GetUserByIdAsync(int id);
        Task<User> GetUserByPhoneNumberAsync(string phoneNumber);
        Task<User> GetUserByEmailAsync(string email);
        Task<User> GetUserByUsernameAsync(string username);
        Task<IEnumerable<UserResponseDTO>> GetAllUsersAsync();
        Task AddUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(int id);

        Task<int> GetBalance(int id);
    }
}
