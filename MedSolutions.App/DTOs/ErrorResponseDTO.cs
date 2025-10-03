using System.Net;

namespace MedSolutions.App.DTOs;

public class ErrorResponseDTO
{
    public bool Success { get; set; }
    public required HttpStatusCode Status { get; set; }
    public required string Message { get; set; } = null!;
    public string? Key { get; set; }
    public string[]? Values { get; set; } = [];
}
