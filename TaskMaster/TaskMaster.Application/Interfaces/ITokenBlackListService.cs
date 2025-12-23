
namespace TaskMaster.Application.Interfaces
{
    public interface ITokenBlackListService
    {
        Task<bool> BlacklistTokenAsync(string tokenId, DateTime expiresAt);
        Task<bool> IsTokenBlacklistedAsync(string tokenId);
        Task<bool> CleanupExpiredTokensAsync();
    }
}
