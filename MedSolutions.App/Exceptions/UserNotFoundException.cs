using System.Net;
using MedSolutions.Domain.Exceptions;

namespace MedSolutions.App.Exceptions;

public class UserNotFoundException(string? userId) : AppException("Invalid authentication data.", "error.invalid_auth_data")
{
    public override HttpStatusCode StatusCode => HttpStatusCode.Unauthorized;
}
