using Microsoft.EntityFrameworkCore;
using MXANH.DTO.Response.AddressResponseDTO;
using MXANH.DTO.Response.UserResponseDTO;
using MXANH.Models;
using MXANH.Repositories.Interfaces;

namespace MXANH.Repositories.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context)
        {
            _context = context;
        }
        // Implement IUserRepository methods here
        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }


        public async Task<UserResponseDTO> GetUserResponseDTOByIdAsync(int id)
        {
            var user = await _context.Users.Select(

               u => new UserResponseDTO
               {
                   Id = u.Id,
                   Name = u.Name,
                   PhoneNumber = u.PhoneNumber,
                   Email = u.Email,
                   Username = u.Username,
                   Points = u.Points,
                   Dob = u.Dob,
                   AvatarUrl = u.AvatarUrl,
                   CreatedAt = u.CreatedAt,
                   UpdateAt = u.UpdateAt,
                   IsActive = u.IsActive,
                   
                   Addresses = u.Addresses.Select(a => new Address
                   {
                       Id = a.Id,
                       UserId = a.UserId,
                       Street = a.Street,
                       City = a.City,
                       IsDefault = a.IsDefault,

                   }).ToList()
               }).FirstOrDefaultAsync();


            return user;
        }
        public async Task<User> GetUserByPhoneNumberAsync(string phoneNumber)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);
        }
        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }
        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }
        public async Task<IEnumerable<UserResponseDTO>> GetAllUsersAsync()
        {
            var users = await _context.Users.Select(

                u => new UserResponseDTO
                {
                    Id = u.Id,
                    Name = u.Name,
                    PhoneNumber = u.PhoneNumber,
                    Email = u.Email,
                    Username = u.Username,
                    Points = u.Points,
                    Dob = u.Dob,
                    AvatarUrl = u.AvatarUrl,
                    CreatedAt = u.CreatedAt,
                    UpdateAt = u.UpdateAt,
                    IsActive = u.IsActive,
                    Addresses = u.Addresses.Select(a => new Address
                    {
                        Id = a.Id,
                        UserId = a.UserId,
                        Street = a.Street,
                        City = a.City,
                        IsDefault = a.IsDefault,

                    }).ToList()
                }).ToListAsync();

           return users;
        }
        public async Task AddUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return; // User not found, nothing to delete
            }
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();


        }
        public async Task<int> GetBalance(int id)
        {
         return await _context.Users
                .Where(u => u.Id == id)
                .Select(u => u.Points)
                .FirstOrDefaultAsync();
        }


    }

}