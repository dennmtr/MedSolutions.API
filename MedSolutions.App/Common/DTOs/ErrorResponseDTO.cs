using System.Net;

namespace MedSolutions.App.Common.DTOs;

public class ErrorResponseDTO
{
    public bool Success { get; set; }
    public required HttpStatusCode Status { get; set; }
    public string? Type { get; set; }
    public required string Title { get; set; } = default!;
    public Dictionary<string, object>? Params { get; set; } = [];
    public string? Instance { get; set; }
    public object? StackTrace { get; set; }
}
