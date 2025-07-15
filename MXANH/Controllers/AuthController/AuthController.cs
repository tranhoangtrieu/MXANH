using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MXANH.DTO.Request.AuthRequestDTO;
using MXANH.Models;
using MXANH.Services.Implementations;
using MXANH.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using MXANH.DTO.Request.UserRequestDTO;

namespace MXANH.Controllers.AuthController
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        private readonly IUserService _userService;

        private readonly AppDbContext _context;
        public AuthController(AuthService authService, IUserService userService, AppDbContext context)
        {
            _authService = authService;
            _userService = userService;
            _context = context;
        }

        //[HttpPost("login")]
        //public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        //{
        //    var user = await _userService.GetUserByUsernameAsync(loginRequest.Username);
        //    if (user == null || !BCrypt.Net.BCrypt.Verify(loginRequest.Password, user.Password))
        //    {
        //        return NotFound("User not found");
        //    }
        //    var token = _authService.GenerateToken(user);

        //    return Ok(new { token });

        //}




        [HttpPost("send-otp")]
        public async Task<IActionResult> SendOtp([FromBody] string phoneNumber)
        {
          
            var phoneRegex = new Regex(@"^(0[3|5|7|8|9])[0-9]{8}$");
            if (!phoneRegex.IsMatch(phoneNumber))
            {
                throw new Exception("Invalid Vietnamese phone number");
            }
            var lastOtp = await _context.OtpCodes
                .Where(o => o.PhoneNumber == phoneNumber)
                .OrderByDescending(o => o.ExpireAt)
                .FirstOrDefaultAsync();

            if (lastOtp?.LockedUntil > DateTime.UtcNow)
                return BadRequest("Tài khoản bị khóa. Vui lòng thử lại sau 10 phút.");

            var code = new Random().Next(100000, 999999).ToString();
            var otp = new OtpCode
            {
                PhoneNumber = phoneNumber,
                Code = code,
                ExpireAt = DateTime.UtcNow.AddMinutes(1)
            };
            _context.OtpCodes.Add(otp);
            await _context.SaveChangesAsync();

            // Dev: In OTP
            Console.WriteLine($"[OTP for {phoneNumber}]: {code}");

            return Ok("OTP đã được gửi.");
        }

        [HttpPost("Active-account")]
        public async Task<IActionResult> ActiveAccount([FromBody] ActiveAccountRequest rq)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.PhoneNumber == rq.PhoneNumber);
            if (user == null)
            {
                return NotFound("User not found");
            }
            user.Name = rq.UserName;
            if (string.IsNullOrEmpty(user.Name))
            {
                return BadRequest("Tên đăng nhập không được để trống.");
            }
            user.IsActive = true;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return Ok("Tài khoản đã được kích hoạt thành công.");
        }




        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromBody] OtpVerifyRequest req)
        {
            var otp = await _context.OtpCodes
                .Where(o => o.PhoneNumber == req.PhoneNumber && !o.IsUsed)
                .OrderByDescending(o => o.ExpireAt)
                .FirstOrDefaultAsync();

            if (otp == null || otp.ExpireAt < DateTime.UtcNow)
                return BadRequest("Mã OTP đã hết hạn hoặc không tồn tại.");

            if (otp.LockedUntil.HasValue && otp.LockedUntil > DateTime.UtcNow)
                return BadRequest("Bạn đã nhập sai nhiều lần. Hãy thử lại sau 10 phút.");

            if (otp.Code != req.Code)
            {
                otp.FailedAttempts++;
                if (otp.FailedAttempts >= 3)
                {
                    otp.LockedUntil = DateTime.UtcNow.AddMinutes(10);
                }

                await _context.SaveChangesAsync();
                return BadRequest("Mã OTP không đúng.");
            }

            otp.IsUsed = true;
            await _context.SaveChangesAsync();

            var user = await _context.Users.FirstOrDefaultAsync(u => u.PhoneNumber == req.PhoneNumber);
            bool isNewUser = false;

            if (user == null)
            {
               user = new User
               {
                   PhoneNumber = req.PhoneNumber,
                   CreatedAt = DateTime.UtcNow,
                   IsActive = false,

               };
                _context.Users.Add(user);
                isNewUser = true;
            }
            else
            {
                _context.Users.Update(user);
            }

            await _context.SaveChangesAsync();
            var token = _authService.GenerateToken(user);
            return Ok(new { 
            token = token,
            userId = user.Id,
            isActive = user.IsActive,
            });
        }




        [HttpGet("login-google")]
        public IActionResult LoginGoogle()
        {
            var redirectUrl = Url.Action(nameof(GoogleCallback), "Auth");
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet("google-callback")]
        public async Task<IActionResult> GoogleCallback()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            if (!result.Succeeded)
            {
                return Unauthorized("Đăng nhập Google thất bại.");
            }

            var claims = result.Principal.Identities
                .FirstOrDefault()?.Claims.ToList();

            var email = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var name = claims?.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(email))
                return BadRequest("Không lấy được email từ Google");

            // Kiểm tra user trong database
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null)
            {
                // Tạo user mới
                user = new User
                {
                    Email = email,
                    Name = name,
                    CreatedAt = DateTime.UtcNow,
                    PhoneNumber = null,
                    Username = null
                };
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }

            // Tạo JWT token
            var token = _authService.GenerateToken(user);
            return Ok(new { token });


        }


    }
}
