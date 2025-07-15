using Microsoft.IdentityModel.Tokens;
using MXANH.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MXANH.Services.Interfaces;

namespace MXANH.Services.Implementations
{
    public class AuthService
    {
        private readonly IUserService _userService;

        private readonly IConfiguration _configuration;
        public AuthService(IUserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
        }

        public string GenerateToken(User user)
        {
            var claims = new[]
            {
                new Claim("userId", user.Id.ToString()),
                new Claim("username", user.Username ?? ""),
                new Claim("email", user.Email ?? ""),
                new Claim("Role", user.Role.ToString()),

        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(int.Parse(_configuration["Jwt:ExpiresInMinutes"])),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
