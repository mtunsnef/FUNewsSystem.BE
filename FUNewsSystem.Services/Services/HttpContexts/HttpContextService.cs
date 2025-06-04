using FUNewsSystem.Domain.Models;
using FUNewsSystem.Services.Services.SystemAccounts;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUNewsSystem.Services.Services.HttpContexts
{
    public class HttpContextService : IHttpContextService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ISystemAccountService _systemAccountService;
        private SystemAccount? _systemAccount;

        public HttpContextService(IHttpContextAccessor httpContextAccessor, ISystemAccountService systemAccountService)
        {
            _httpContextAccessor = httpContextAccessor;
            _systemAccountService = systemAccountService;
        }

        public string GetIpAddress()
        {
            var context = _httpContextAccessor.HttpContext;
            if (context == null)
            {
                return string.Empty;
            }

            var ipAddress = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (string.IsNullOrEmpty(ipAddress))
            {
                ipAddress = context.Connection.RemoteIpAddress?.ToString();
            }
            return ipAddress ?? string.Empty;
        }

        public async Task<SystemAccount?> GetSystemAccount()
        {
            if (_systemAccount != null)
                return _systemAccount;

            var userId = GetUserId();

            if (userId != null)
            {
                _systemAccount = await _systemAccountService.GetUserByIdAsync((short)userId);
            }

            return _systemAccount;
        }

        public async Task<SystemAccount> GetSystemAccountAndThrow()
        {
            var user = await GetSystemAccount();
            return user ?? throw new UnauthorizedAccessException("Không thể xác định người dùng từ JWT.");
        }


        private short? GetUserId()
        {
            var claimValue = _httpContextAccessor.HttpContext?.User.FindFirst("AccountId")?.Value;
            return short.TryParse(claimValue, out var id) ? id : null;
        }
    }
}
