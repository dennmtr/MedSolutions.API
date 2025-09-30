using System.Security.Claims;
using System.Text;
using MedSolutions.App.DTOs;
using MedSolutions.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography;
using MedSolutions.Api.Logging;
using MedSolutions.App.Interfaces;

namespace MedSolutions.App.Services;

public class AuthService(
    UserManager<User> userManager,
    SignInManager<User> signInManager,
    IConfiguration config,
    ILogger<AuthService> logger) : IAuthService
{
    private readonly UserManager<User> _userManager = userManager;
    private readonly SignInManager<User> _signInManager = signInManager;
    private readonly IConfiguration _config = config;
    private readonly ILogger<AuthService> _logger = logger;

    public async Task<LoginResponseDTO> LoginAsync(LoginRequestDTO loginRequest)
    {
        if (string.IsNullOrWhiteSpace(loginRequest.Email) || string.IsNullOrWhiteSpace(loginRequest.Password))
        {
            throw new ArgumentException("auth.required.credentials");
        }

        var user = await _userManager.FindByEmailAsync(loginRequest.Email);
        if (user?.Email == null)
        {
            _logger.AuthFailedLoginAttempt(loginRequest.Email);
            throw new UnauthorizedAccessException("auth.invalid.email");
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, loginRequest.Password, lockoutOnFailure: false);
        if (!result.Succeeded)
        {
            _logger.AuthFailedLoginAttempt(loginRequest.Email);
            throw new UnauthorizedAccessException("auth.invalid.credentials");
        }

        var userRoles = await _userManager.GetRolesAsync(user);

        var token = CreateJwtToken(user, userRoles);

        var existingClaims = await _userManager.GetClaimsAsync(user);
        var oldRefreshClaims = existingClaims.Where(c => c.Type == "RefreshToken").ToList();
        foreach (var old in oldRefreshClaims)
        {
            await _userManager.RemoveClaimAsync(user, old);
        }

        var refreshToken = GenerateRefreshToken(out DateTime refreshTokenExpiration);
        await _userManager.AddClaimAsync(user, new Claim("RefreshToken", $"{refreshToken}|{refreshTokenExpiration:o}"));

        return new LoginResponseDTO {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            RefreshToken = refreshToken
        };
    }

    public async Task<LoginResponseDTO> RefreshTokenAsync(string? userId, string? refreshToken)
    {

        if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(refreshToken))
        {
            throw new ArgumentException("auth.required.credentials");
        }

        var user = await _userManager.FindByIdAsync(userId ?? "") ?? throw new UnauthorizedAccessException("auth.unauthorized");

        var existingClaims = await _userManager.GetClaimsAsync(user);

        var oldRefreshTokenClaim = existingClaims
            .FirstOrDefault(c => c.Type == "RefreshToken" && c.Value.Split('|')[0] == refreshToken);

        if (oldRefreshTokenClaim == null)
        {
            _logger.AuthFailedRefreshTokenAttempt(refreshToken);
            throw new UnauthorizedAccessException("auth.unauthorized");
        }

        var parts = oldRefreshTokenClaim.Value.Split('|');
        if (!DateTime.TryParse(parts[1], out var refreshTokenExpiration) || refreshTokenExpiration < DateTime.UtcNow)
        {
            throw new UnauthorizedAccessException("auth.unauthorized");
        }

        await _userManager.RemoveClaimAsync(user, oldRefreshTokenClaim);

        var userRoles = await _userManager.GetRolesAsync(user);

        var token = CreateJwtToken(user, userRoles);

        refreshToken = GenerateRefreshToken(out refreshTokenExpiration);
        await _userManager.AddClaimAsync(user, new Claim("RefreshToken", $"{refreshToken}|{refreshTokenExpiration:o}"));

        return new LoginResponseDTO {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            RefreshToken = refreshToken
        };
    }

    public async Task LogoutAsync(string? userId)
    {
        var user = await _userManager.FindByIdAsync(userId ?? "");
        if (user is null)
        {
            return;
        }

        var claims = await _userManager.GetClaimsAsync(user);
        var refreshTokenClaims = claims.Where(c => c.Type == "RefreshToken").ToList();

        foreach (var claim in refreshTokenClaims)
        {
            await _userManager.RemoveClaimAsync(user, claim);
        }

        await _signInManager.SignOutAsync();
    }

    private JwtSecurityToken CreateJwtToken(User user, IEnumerable<string> roles)
    {
        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Key"]!));
        var creds = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

        var tokenExpiration = DateTime.UtcNow.AddMinutes(int.Parse(_config["JWT:AccessTokenExpirationMinutes"] ?? "15"));

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(JwtRegisteredClaimNames.Email, user.Email!),
            new(JwtRegisteredClaimNames.Name, user.FirstName),
            new(JwtRegisteredClaimNames.FamilyName, user.LastName)
        };
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        return new JwtSecurityToken(
            issuer: _config["JWT:Issuer"],
            audience: _config["JWT:Audience"],
            claims: claims,
            expires: tokenExpiration,
            signingCredentials: creds
        );
    }

    private string GenerateRefreshToken(out DateTime expiration)
    {
        var randomBytes = RandomNumberGenerator.GetBytes(64);
        var refreshToken = Convert.ToBase64String(randomBytes)
            .TrimEnd('=')
            .Replace('+', '-')
            .Replace('/', '_');

        expiration = DateTime.UtcNow.AddDays(int.Parse(_config["JWT:RefreshTokenExpirationDays"] ?? "7"));
        return refreshToken;
    }
}
