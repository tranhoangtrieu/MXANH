using Microsoft.IdentityModel.Tokens;
using MXANH.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MXANH.Services.Implementations
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly HashSet<string> _blacklistedTokens = new HashSet<string>(); // In-memory for simplicity; use Redis/database in production

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(string username)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(30), // Token expires in 30 minutes
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task BlacklistTokenAsync(string token)
        {
            _blacklistedTokens.Add(token);
            // In production, store in Redis or a database with an expiration time matching the token's expiry
            await Task.CompletedTask;
        }

        public async Task<bool> IsTokenBlacklistedAsync(string token)
        {
            return await Task.FromResult(_blacklistedTokens.Contains(token));
        }
    }
}
