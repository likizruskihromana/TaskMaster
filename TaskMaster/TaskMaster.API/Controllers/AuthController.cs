using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TaskMaster.Application.DTOs.Auth;
using TaskMaster.Application.Interfaces;
using TaskMaster.Application.Services;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    IAuthService service { get; set; }
    public AuthController(IAuthService service) => this.service = service;

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var result = await service.RegisterAsync(dto);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }


    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    { 
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var result = await service.LoginAsync(dto);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }
    [HttpPost("logout")]
    [Authorize]  // Requires authentication
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Logout()
    {
        // Get user ID from JWT token claims
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var jtiClaim = User.FindFirstValue(JwtRegisteredClaimNames.Jti);
        var expClaim = User.FindFirstValue(JwtRegisteredClaimNames.Exp);

        if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
        {
            return Unauthorized(new { message = "Invalid token" });
        }

        // Convert expiration from Unix timestamp
        var tokenExpiration = DateTimeOffset.FromUnixTimeSeconds(long.Parse(expClaim)).UtcDateTime;

        
        var result = await service.LogoutAsync(userId, jtiClaim, tokenExpiration);

        if (!result)
        {
            return BadRequest(new { message = "Logout failed" });
        }

        return Ok(new
        {
            message = "Logged out successfully. Please delete the token on client side.",
            timestamp = DateTime.UtcNow
        });
    }
}