using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MedSolutions.Api.Logging;
using MedSolutions.App.DTOs;
using MedSolutions.App.Exceptions;
using MedSolutions.App.Interfaces;
using MedSolutions.Domain.Models;
using MedSolutions.Shared.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

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
    public async Task<TokenDTO> LoginAsync(LoginRequestDTO request)
    {
        if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
        {
            throw new AuthenticationFailedException();
        }

        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null)
        {
            _logger.EmailNotFound(request.Email);
            throw new AuthenticationFailedException(request.Email);
        }

        var signInResult = await _signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: false);
        if (!signInResult.Succeeded)
        {
            _logger.AuthenticationFailed(request.Email);
            throw new AuthenticationFailedException(request.Email);
        }

        var roles = await _userManager.GetRolesAsync(user);
        var accessToken = CreateJwtToken(user, roles);

        var existingClaims = await _userManager.GetClaimsAsync(user);
        await RevokeAllRefreshTokensForUserAsync(user, existingClaims);


        var refreshTokenDays = int.Parse(_config["JWT:RefreshTokenExpirationDays"] ?? "7");
        var refreshTokenExpiry = request.RememberMe
            ? DateTime.MaxValue
            : DateTime.UtcNow.AddDays(refreshTokenDays);

        var refreshToken = GenerateRefreshToken(refreshTokenExpiry);
        await _userManager.AddClaimAsync(user, new Claim("RefreshToken", refreshToken));

        return new TokenDTO {
            Token = new JwtSecurityTokenHandler().WriteToken(accessToken),
            RefreshToken = refreshToken,
            RefreshTokenExpirationDate = refreshTokenExpiry
        };
    }

    public async Task<TokenDTO> RefreshTokenAsync(string refreshToken)
    {
        if (string.IsNullOrEmpty(refreshToken))
        {
            throw new InvalidTokenException(refreshToken);
        }

        var users = await _userManager.GetUsersForClaimAsync(new Claim("RefreshToken", refreshToken));
        var user = users.SingleOrDefault();
        if (user is null)
        {
            _logger.UserWithTokenNotFound(refreshToken);
            throw new InvalidTokenException(refreshToken);
        }

        var claims = await _userManager.GetClaimsAsync(user);
        var activeTokenClaim = claims.SingleOrDefault(c => c.Type == "RefreshToken" && c.Value == refreshToken);
        if (activeTokenClaim == null)
        {
            _logger.InvalidRefreshToken(refreshToken, user.Email);
            throw new InvalidTokenException(refreshToken);
        }

        var principal = ValidateRefreshToken(refreshToken) ?? throw new InvalidTokenException(refreshToken);
        var jwt = new JwtSecurityTokenHandler().ReadJwtToken(refreshToken);
        var refreshTokenExpiry = jwt.ValidTo;

        var extendedExpiry = DateTime.UtcNow.AddDays(int.Parse(_config["JWT:RefreshTokenExpirationDays"] ?? "7"));

        if (refreshTokenExpiry.TrimToMinutes() < extendedExpiry.TrimToMinutes())
        {
            await _userManager.RemoveClaimAsync(user, activeTokenClaim);
            var newRefreshToken = GenerateRefreshToken(extendedExpiry);
            await _userManager.AddClaimAsync(user, new Claim("RefreshToken", newRefreshToken));

            refreshToken = newRefreshToken;
            refreshTokenExpiry = extendedExpiry;
        }
        var roles = await _userManager.GetRolesAsync(user);

        var newAccessToken = CreateJwtToken(user, roles);

        return new TokenDTO() {
            Token = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
            RefreshToken = refreshToken,
            RefreshTokenExpirationDate = refreshTokenExpiry
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
        await RevokeAllRefreshTokensForUserAsync(user, claims);

        await _signInManager.SignOutAsync();
    }

    private JwtSecurityToken CreateJwtToken(User user, IEnumerable<string> roles)
    {
        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Key"]!));
        var creds = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

        var expiration = DateTime.UtcNow.AddMinutes(int.Parse(_config["JWT:AccessTokenExpirationMinutes"] ?? "15"));

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
            expires: expiration,
            signingCredentials: creds
        );
    }

    private async Task RevokeAllRefreshTokensForUserAsync(User user, IEnumerable<Claim> currentClaims)
    {
        var refreshClaims = currentClaims.Where(c => c.Type == "RefreshToken").ToList();
        foreach (var claim in refreshClaims)
        {
            await _userManager.RemoveClaimAsync(user, claim);
        }
    }

    private string GenerateRefreshToken(DateTime expiration)
    {
        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Key"]!));
        var creds = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
        new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new(JwtRegisteredClaimNames.Exp, new DateTimeOffset(expiration).ToUnixTimeSeconds().ToString())
    };

        var token = new JwtSecurityToken(
            issuer: _config["JWT:Issuer"],
            audience: _config["JWT:Audience"],
            claims: claims,
            expires: expiration,
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private ClaimsPrincipal? ValidateRefreshToken(string refreshToken)
    {
        var handler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_config["JWT:Key"]!);

        try
        {
            return handler.ValidateToken(refreshToken, new TokenValidationParameters {
                ValidateIssuer = true,
                ValidIssuer = _config["JWT:Issuer"],
                ValidateAudience = true,
                ValidAudience = _config["JWT:Audience"],
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            }, out _);
        }
        catch
        {
            return null;
        }
    }

}
