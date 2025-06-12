using FUNewsSystem.Domain.Models;
using FUNewsSystem.Services.DTOs.Request.AuthDto;
using FUNewsSystem.Services.DTOs.Request.SystemAccountDto;
using FUNewsSystem.Services.Services.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FUNewsSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("token")]
        public async Task<ActionResult<LoginPayloadDto>> Login(UserCredentialDto dto)
        {
            return Ok(await _authService.Login(dto));
        }

        [HttpGet("introspect")]
        [Authorize]
        public async Task<ActionResult<SystemAccount>> GetMyInfo()
        {
            return Ok(await _authService.GetMe());
        }

        [HttpPost("refresh")]
        public async Task<ActionResult<TokenPayloadDto>> RefreshToken(RefreshRequestDto dto)
        {
            return Ok(await _authService.RefreshTokenAsync(dto));
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout(LogoutRequestDto dto)
        {
            await _authService.LogoutAsync(dto);
            return Ok(new { message = "Logged out successfully." });
        }
    }
}
