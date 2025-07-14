using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MXANH.DTO.Request.UserRequestDTO;
using MXANH.Models;
using MXANH.Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;


namespace MXANH.Controllers.UserControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("by-id/{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userService.GetUserResponseDTOByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }



        [HttpGet("by-token/{token}")]
        public async Task<IActionResult> GetUserByToken(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                return BadRequest("Token không được để trống.");
            }

            try
            {
                // Giải mã token để lấy userId
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);
                Console.WriteLine(jwtToken);
                var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "userId");
                if (userIdClaim == null)
                {
                    return BadRequest("Token không chứa userId.");
                }

                int userId = int.Parse(userIdClaim.Value);

                var user = await _userService.GetUserResponseDTOByIdAsync(userId);
                if (user == null)
                {
                    return NotFound("Không tìm thấy người dùng.");
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest("Token không hợp lệ: " + ex.Message);
            }
        }
        

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequestDTO userRequest)
        {
            if (userRequest == null)
            {
                return BadRequest("User data is null");
            }
            await _userService.AddUserAsync(userRequest);
            return Ok("User created successfully");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody]  UpdateUserRequestDTO userRequest)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _userService.UpdateUserAsync(id, userRequest);


            return Ok("User updated successfully");
        }

        [HttpGet("balance/{id}")]
        public async Task<IActionResult> GetUserBalance(int id)
        {
            var balance = await _userService.GetBalance(id);
            if (balance == null)
            {
                return NotFound();
            }
            return Ok(balance);
        }
        [HttpPost("{id}/withdraw")]
        public async Task<IActionResult> WithdrawPoints(int id, [FromBody] WithdrawRequest request)
        {
            await _userService.WithdrawPoints(id, request);
            var user = await _userService.GetUserByIdAsync(id);
            return Ok(new { message = "Withdrawal successful", remaining = user.Points });
        }


        [HttpPost("avatar/{id}")]
        public async Task<IActionResult> UploadAvatar(int id, IFormFile avatar)
        {
            if (avatar == null || avatar.Length == 0)
                return BadRequest("Invalid file");

            var avatarUrl = await _userService.UploadAvatarAsync(id, avatar);
            return Ok(new { avatarUrl });
        }


    }
}
