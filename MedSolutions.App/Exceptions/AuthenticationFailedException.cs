using System.Net;
using MedSolutions.Domain.Common.Exceptions;

namespace MedSolutions.App.Exceptions;

public class AuthenticationFailedException(string? email = null) : AppException("Login failed. The email or password you entered is incorrect.", "error.exception.invalidCredentials")
{
    public override HttpStatusCode StatusCode => HttpStatusCode.Unauthorized;
}
