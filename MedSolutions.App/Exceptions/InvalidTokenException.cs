using System.Net;
using MedSolutions.Domain.Exceptions;

namespace MedSolutions.App.Exceptions;

public class InvalidTokenException(string? token = null) : AppException("Invalid token.", "error.invalid_token")
{
    public override HttpStatusCode StatusCode => HttpStatusCode.Unauthorized;
}
