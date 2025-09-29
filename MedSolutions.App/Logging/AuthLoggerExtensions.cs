using Microsoft.Extensions.Logging;

namespace MedSolutions.App.Logging;

public static partial class AuthLoggerExtensions
{

    [LoggerMessage(Level = LogLevel.Information, Message = "{Message}")]
    public static partial void AuthNotify(this ILogger logger, string message);

    [LoggerMessage(Level = LogLevel.Warning, Message = "{Message}")]
    public static partial void AuthWarning(this ILogger logger, string message);

    [LoggerMessage(Level = LogLevel.Error, Message = "Auth failed: {Message}")]
    public static partial void AuthFailed(this ILogger logger, string message, Exception? exception = null);
}
