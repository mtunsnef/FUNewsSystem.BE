using FUNewsSystem.Domain.Enums.Auth;
using FUNewsSystem.Domain.Exceptions.Http;
using FUNewsSystem.Domain.Models;
using FUNewsSystem.Services.DTOs.Request.AuthDto;
using FUNewsSystem.Services.DTOs.Request.SystemAccountDto;
using FUNewsSystem.Services.Services.Configs;
using FUNewsSystem.Services.Services.HttpContexts;
using FUNewsSystem.Services.Services.SystemAccounts;
using Microsoft.IdentityModel.Tokens;
using BC = BCrypt.Net.BCrypt;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Azure.Core;

namespace FUNewsSystem.Services.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IConfigService _configService;
        private readonly IHttpContextService _httpContextService;
        private readonly ISystemAccountService _systemAccountService;
        private readonly IBlacklistTokenService _blacklistTokenService;
        public AuthService(IConfigService configService, ISystemAccountService systemAccountService, IHttpContextService httpContextService, IBlacklistTokenService blacklistTokenService)
        {
            _configService = configService;
            _httpContextService = httpContextService;
            _systemAccountService = systemAccountService;
            _blacklistTokenService = blacklistTokenService;
        }

        public async Task<LoginPayloadDto> Login(UserCredentialDto dto)
        {
            var account = await _systemAccountService.GetAccountByEmail(dto.Email);

            if (account is null || !BC.Verify(dto.Password, account.AccountPassword))
            {
                throw new UnauthorizedException("Username or Password is not correct.");
            }

            return new LoginPayloadDto
            {
                AccessToken = GenerateTokenPayload(account),
                Authenticated = true
            };
        }
        public async Task LogoutAsync(LogoutRequestDto request)
        {
            try
            {
                var token = request.Token;

                var handler = new JwtSecurityTokenHandler();
                var jwt = handler.ReadJwtToken(token);

                var jti = jwt.Id;
                if (string.IsNullOrEmpty(jti))
                    throw new UnauthorizedException("Invalid token jti");

                var expiry = GetTokenExpiry(token);

                await _blacklistTokenService.AddAsync(jti, expiry);
            }
            catch (SecurityTokenExpiredException)
            {
                throw new Exception("Token already expired.");
            }
        }

        public async Task<SystemAccount> GetMe()
        {
            return await _httpContextService.GetSystemAccountAndThrow();
        }
        private ClaimsPrincipal? ValidateToken(string token, string secret)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = false, 
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret))
                };

                return tokenHandler.ValidateToken(token, validationParameters, out _);
            }
            catch
            {
                return null;
            }
        }

        private DateTime GetTokenExpiry(string token)
        {
            var jwt = new JwtSecurityTokenHandler().ReadJwtToken(token);
            return jwt.ValidTo;
        }
        private TokenPayloadDto GenerateTokenPayload(SystemAccount systemAccount)
        {
            var accessToken = GenerateToken(systemAccount, AuthToken.AccessToken);
            var refreshToken = GenerateToken(systemAccount, AuthToken.RefreshToken);

            return new TokenPayloadDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiresIn = _configService.GetInt("Jwt:Lifetime:AccessToken")
            };
        }

        public async Task<TokenPayloadDto> RefreshTokenAsync(RefreshRequestDto dto)
        {
            if (await _blacklistTokenService.IsBlacklistedAsync(dto.Token))
                throw new UnauthorizedException("Token has been revoked");

            var principal = ValidateToken(dto.Token, _configService.GetString("Jwt:SecretKey"));
            if (principal == null)
                throw new UnauthorizedException("Invalid token");

            var tokenType = principal.FindFirst(JwtRegisteredClaimNames.Typ)?.Value;
            if (tokenType != AuthToken.RefreshToken.ToString())
                throw new UnauthorizedException("Invalid token type");

            var accountIdStr = principal.FindFirst("AccountId")?.Value;
            if (!short.TryParse(accountIdStr, out var accountId))
                throw new UnauthorizedException("Invalid user");

            var account = await _systemAccountService.GetUserByIdAsync(accountId) ?? throw new UnauthorizedException("User not found");
            var token = dto.Token;

            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);

            var jti = jwt.Id;
            if (string.IsNullOrEmpty(jti))
                throw new UnauthorizedException("Invalid token jti");

            var expiry = GetTokenExpiry(token);
            await _blacklistTokenService.AddAsync(jti, expiry);

            var newToken = GenerateTokenPayload(account);
            return newToken;
        }

        private string GenerateToken(SystemAccount account, AuthToken type)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var secretKey = Encoding.UTF8.GetBytes(_configService.GetString("Jwt:SecretKey"));

            var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Sub, _configService.GetString("Jwt:Subject")),
            new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
            new Claim(JwtRegisteredClaimNames.Typ, type.ToString()),
            new Claim("AccountId", account.AccountId.ToString()),
            new Claim("Email", account.AccountEmail ?? ""),
            new Claim(ClaimTypes.Role, account.AccountRole?.ToString() ?? "")
        };

            var lifetime = type == AuthToken.AccessToken
                ? _configService.GetInt("Jwt:Lifetime:AccessToken")
                : _configService.GetInt("Jwt:Lifetime:RefreshToken");

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddSeconds(lifetime),
                Issuer = _configService.GetString("Jwt:Issuer"),
                Audience = _configService.GetString("Jwt:Audience"),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

    }
}
