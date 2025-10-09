using MedSolutions.App.Common.DTOs;

namespace MedSolutions.App.Interfaces;

public interface IAuthService
{
    Task<TokenDTO> LoginAsync(LoginRequestDTO request);
    Task<TokenDTO> RefreshTokenAsync(string refreshToken);
    Task LogoutAsync(string? userId);
}
