using System.Net;
using MedSolutions.Domain.Common.Exceptions;

namespace MedSolutions.App.Exceptions;

public class UserNotFoundException(string? userId) : AppException("Invalid authentication data.", "error.exception.invalidAuthenticationData")
{
    public override HttpStatusCode StatusCode => HttpStatusCode.Unauthorized;
}
