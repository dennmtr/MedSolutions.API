namespace MedSolutions.Api.Logging;

public static partial class ProgramLoggerExtensions
{
    [LoggerMessage(Level = LogLevel.Warning, Message = "{Message}")]
    public static partial void StartupWarning(this ILogger logger, string message);

    [LoggerMessage(Level = LogLevel.Error, Message = "{Message}")]
    public static partial void StartupError(this ILogger logger, string message, Exception? ex = null);

    [LoggerMessage(Level = LogLevel.Warning, Message = "{Message}")]
    public static partial void DatabaseWarning(this ILogger logger, string message);
}
