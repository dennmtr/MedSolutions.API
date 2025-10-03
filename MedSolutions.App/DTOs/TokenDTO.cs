namespace MedSolutions.App.DTOs;

public class TokenDTO
{
    public required string Token { get; set; }
    public required string RefreshToken { get; set; }
    public required DateTime RefreshTokenExpirationDate { get; set; }
}
