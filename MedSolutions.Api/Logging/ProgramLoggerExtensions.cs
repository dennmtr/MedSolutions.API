namespace MedSolutions.Api.Logging;

public static partial class ProgramLoggerExtensions
{

    [LoggerMessage(Level = LogLevel.Warning, Message = "{Message}")]
    public static partial void DatabaseWarning(this ILogger logger, string message);
}
