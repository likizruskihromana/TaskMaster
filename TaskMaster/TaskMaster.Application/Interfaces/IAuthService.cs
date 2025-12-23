using TaskMaster.Application.DTOs.Auth;

namespace TaskMaster.Application.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResultDto> RegisterAsync(RegisterDto registerDto);
        Task<AuthResultDto> LoginAsync(LoginDto loginDto);
        Task<bool> LogoutAsync(int userId, string tokenId, DateTime expirationDate);
    }
}
