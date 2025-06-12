using FUNewsSystem.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUNewsSystem.Infrastructure.Repositories.InvalidatedTokens
{
    public class BlacklistTokenRepository : IBlacklistTokenRepository
    {
        private readonly FunewsSystemApiDbContext _context;

        public BlacklistTokenRepository(FunewsSystemApiDbContext context)
        {
            _context = context;
        }

        public async Task<bool> IsBlacklistedAsync(string token)
        {
            return await _context.InvalidatedTokens
                .AnyAsync(t => t.Token == token && t.ExpiryTime > DateTime.UtcNow);
        }

        public async Task AddAsync(string token, DateTime expiryTime)
        {
            var entity = new InvalidatedToken
            {
                Token = token,
                ExpiryTime = expiryTime
            };

            _context.InvalidatedTokens.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task CleanupExpiredAsync()
        {
            var expiredTokens = await _context.InvalidatedTokens
                .Where(t => t.ExpiryTime <= DateTime.UtcNow)
                .ToListAsync();

            if (expiredTokens.Any())
            {
                _context.InvalidatedTokens.RemoveRange(expiredTokens);
                await _context.SaveChangesAsync();
            }
        }
    }

}
