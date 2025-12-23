using Microsoft.EntityFrameworkCore;
using TaskMaster.Application.Interfaces;
using TaskMaster.Domain.Entities;
using TaskMaster.Infrastructure.Data;

namespace TaskMaster.Infrastructure.Services
{
    public class TokenBlackListService : ITokenBlackListService
    {
        private readonly ApplicationDbContext _context;

        public TokenBlackListService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> BlacklistTokenAsync(string tokenId, DateTime expiresAt)
        {
            var blacklistedToken = new TokenBlackList
            {
                TokenId = tokenId,
                BlackListedAt = DateTime.UtcNow,
                ExpiresAt = expiresAt
            };

            await _context.TokenBlacklists.AddAsync(blacklistedToken);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> IsTokenBlacklistedAsync(string tokenId)
        {
            return await _context.TokenBlacklists
                .AnyAsync(t => t.TokenId == tokenId);
        }

        public async Task<bool> CleanupExpiredTokensAsync()
        {
            var expiredTokens = await _context.TokenBlacklists
                .Where(t => t.ExpiresAt < DateTime.UtcNow)
                .ToListAsync();

            _context.TokenBlacklists.RemoveRange(expiredTokens);
            await _context.SaveChangesAsync();
            return expiredTokens.Any();
        }
    }
}