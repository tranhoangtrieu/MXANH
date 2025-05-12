using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace MXANH.Controllers.UserControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;

        public AuthController(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            // REPLACE with real user validation (e.g., check against a database)
            if (request.Username == "user" && request.Password == "pass")
            {
                var token = _tokenService.GenerateToken(request.Username);
                return Ok(new { token });
            }
            return Unauthorized(new { message = "Invalid username or password" });
        }

        [HttpPost("logout")]
        [Authorize] // Requires a valid JWT
        public async Task<IActionResult> Logout()
        {
            // Get the JWT from the Authorization header
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            if (!string.IsNullOrEmpty(token))
            {
                await _tokenService.BlacklistTokenAsync(token);
            }
            return Ok(new { message = "Logged out successfully" });
        }
    }
}
