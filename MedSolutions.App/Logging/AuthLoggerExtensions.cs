using Microsoft.Extensions.Logging;

namespace MedSolutions.App.Logging;

public static partial class AuthLoggerExtensions
{

    [LoggerMessage(Level = LogLevel.Warning, Message = "User with email {UserId} not found in the database.")]
    public static partial void UserNotFound(this ILogger logger, string? userId);

    [LoggerMessage(Level = LogLevel.Warning, Message = "User with refresh token '{RefreshToken}' not found.")]
    public static partial void UserWithTokenNotFound(this ILogger logger, string refreshToken);

    [LoggerMessage(Level = LogLevel.Warning, Message = "Invalid refresh '{RefreshToken}' for user {Email}.")]
    public static partial void InvalidRefreshToken(this ILogger logger, string refreshToken, string? email);

    [LoggerMessage(Level = LogLevel.Warning, Message = "User with email {Email} not found in the database.")]
    public static partial void EmailNotFound(this ILogger logger, string email);

    [LoggerMessage(Level = LogLevel.Warning, Message = "Login attempt failed due to invalid email/password combination for email {Email}.")]
    public static partial void AuthenticationFailed(this ILogger logger, string email);
}
