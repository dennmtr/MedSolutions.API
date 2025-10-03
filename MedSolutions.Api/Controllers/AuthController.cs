
using System.Security.Claims;
using MedSolutions.App.DTOs;
using MedSolutions.App.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace MedSolutions.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class AuthController(IAuthService authService) : ControllerBase
{
    private readonly IAuthService _authService = authService;

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<LoginResponseDTO>> Login([FromBody] LoginRequestDTO request)
    {
        Response.Cookies.Delete("RefreshToken");

        var tokens = await _authService.LoginAsync(request);

        Response.Cookies.Append(
                "RefreshToken",
                Uri.EscapeDataString(tokens.RefreshToken),
                new CookieOptions {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = tokens.RefreshTokenExpirationDate
                });

        return Ok(new LoginResponseDTO() {
            Token = tokens.Token
        });
    }

    [HttpPost("refresh-token")]
    [EnableRateLimiting("RefreshTokenPerCookie")]
    [AllowAnonymous]
    public async Task<ActionResult<LoginResponseDTO>> RefreshToken()
    {
        var currentCookie = Uri.UnescapeDataString(Request.Cookies.FirstOrDefault(c => c.Key == "RefreshToken").Value ?? "");

        var tokens = await _authService.RefreshTokenAsync(currentCookie);

        if (tokens.RefreshToken != currentCookie)
        {
            Response.Cookies.Delete("RefreshToken");
            Response.Cookies.Append(
                "RefreshToken",
                Uri.EscapeDataString(tokens.RefreshToken),
                new CookieOptions {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = tokens.RefreshTokenExpirationDate
                });
        }

        return Ok(new LoginResponseDTO() {
            Token = tokens.Token
        });
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        Response.Cookies.Delete("RefreshToken");
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        await _authService.LogoutAsync(userId);
        return NoContent();
    }

    [HttpGet()]
    public IActionResult Test() => NoContent();
}



