using FUNewsSystem.Domain.Models;
using FUNewsSystem.Services.DTOs.Request.AuthDto;
using FUNewsSystem.Services.DTOs.Request.SystemAccountDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FUNewsSystem.Services.Services.Auth
{
    public interface IAuthService
    {
        Task<SystemAccount> GetMe();
        Task LogoutAsync(LogoutRequestDto request);
        Task<TokenPayloadDto> RefreshTokenAsync(RefreshRequestDto dto);
        Task<LoginPayloadDto> Login(UserCredentialDto dto);
    }
}
