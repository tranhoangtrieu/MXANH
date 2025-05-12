using MXANH.Services.Interfaces;

namespace MXANH.Middlewares.Auth
{
    public class TokenBlacklistMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ITokenService _tokenService;

        public TokenBlacklistMiddleware(RequestDelegate next, ITokenService tokenService)
        {
            _next = next;
            _tokenService = tokenService;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            if (!string.IsNullOrEmpty(token))
            {
                if (await _tokenService.IsTokenBlacklistedAsync(token))
                {
                    context.Response.StatusCode = 401;
                    await context.Response.WriteAsync("Token is blacklisted");
                    return;
                }
            }
            await _next(context);
        }
    }

    public static class TokenBlacklistMiddlewareExtensions
    {
        public static IApplicationBuilder UseTokenBlacklist(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<TokenBlacklistMiddleware>();
        }
    }
}
