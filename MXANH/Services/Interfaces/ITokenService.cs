namespace MXANH.Services.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(string username);
        Task BlacklistTokenAsync(string token);
        Task<bool> IsTokenBlacklistedAsync(string token);
    }
}
