
using System.Security.Claims;
using MedSolutions.Api.Filters;
using MedSolutions.App.DTOs;
using MedSolutions.App.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedSolutions.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class AuthController(IAuthService authService) : ControllerBase
{
    private readonly IAuthService _authService = authService;


    [HttpPost("login")]
    [ServiceFilter(typeof(RefreshTokenCookieFilter))]
    [AllowAnonymous]
    public async Task<ActionResult<LoginResponseDTO>> Login([FromBody] LoginRequestDTO request)
    {
        var result = await _authService.LoginAsync(request);
        return Ok(result);
    }

    [HttpPost("refreshToken")]
    [ServiceFilter(typeof(RefreshTokenCookieFilter))]
    public async Task<ActionResult<LoginResponseDTO>> RefreshToken()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var refreshToken = Request.Cookies["refreshToken"];
        var result = await _authService.RefreshTokenAsync(userId, refreshToken);
        return Ok(result);
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        await _authService.LogoutAsync(userId);
        return NoContent();
    }
}



