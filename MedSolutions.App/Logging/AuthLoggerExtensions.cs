using Microsoft.Extensions.Logging;

namespace MedSolutions.Api.Logging;

public static partial class AuthLoggerExtensions
{
    [LoggerMessage(Level = LogLevel.Warning, Message = "Invalid login attempt for {Email}")]
    public static partial void AuthFailedLoginAttempt(this ILogger logger, string email);

    [LoggerMessage(Level = LogLevel.Warning, Message = "Invalid token: {RefreshToken}")]
    public static partial void AuthFailedRefreshTokenAttempt(this ILogger logger, string refreshToken);
}
