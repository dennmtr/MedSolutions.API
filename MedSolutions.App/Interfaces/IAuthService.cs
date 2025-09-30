using MedSolutions.App.DTOs;

namespace MedSolutions.App.Interfaces;

public interface IAuthService
{
    Task<LoginResponseDTO> LoginAsync(LoginRequestDTO loginRequest);
    Task<LoginResponseDTO> RefreshTokenAsync(string? userId, string? refreshToken);
    Task LogoutAsync(string? userId);
}
