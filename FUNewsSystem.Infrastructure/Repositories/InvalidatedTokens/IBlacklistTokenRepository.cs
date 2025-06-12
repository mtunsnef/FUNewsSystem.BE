using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUNewsSystem.Infrastructure.Repositories.InvalidatedTokens
{
    public interface IBlacklistTokenRepository
    {
        Task<bool> IsBlacklistedAsync(string token);
        Task AddAsync(string token, DateTime expiryTime);
        Task CleanupExpiredAsync();
    }

}
