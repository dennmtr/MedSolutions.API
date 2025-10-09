using System.Net;
using MedSolutions.Domain.Common.Exceptions;

namespace MedSolutions.App.Exceptions;

public class InvalidTokenException(string? token = null) : AppException("Invalid token.", "error.exception.invalidToken")
{
    public override HttpStatusCode StatusCode => HttpStatusCode.Unauthorized;
}
