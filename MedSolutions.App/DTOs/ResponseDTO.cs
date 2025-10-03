using System.Net;

namespace MedSolutions.App.DTOs;

public class ResponseDTO<T>(T data, HttpStatusCode status = HttpStatusCode.OK)
{
    public bool Success { get; set; } = true;
    public HttpStatusCode Status { get; set; } = status;
    public T? Data { get; set; } = data;
}
