using System.Net;

namespace MedSolutions.Api.DTOs;

public class ErrorResponseDTO
{
    public bool Success { get; set; }
    public HttpStatusCode Status { get; set; }
    public string Message { get; set; } = null!;
    public IEnumerable<string> Errors { get; set; } = Array.Empty<string>();
}
