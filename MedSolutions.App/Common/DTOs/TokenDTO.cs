namespace MedSolutions.App.Common.DTOs;

public class TokenDTO : LoginResponseDTO
{
    public required string RefreshToken { get; set; }
    public required DateTime RefreshTokenExpirationDate { get; set; }
}
