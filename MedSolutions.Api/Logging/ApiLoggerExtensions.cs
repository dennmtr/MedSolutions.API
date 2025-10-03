namespace MedSolutions.Api.Logging;

public static partial class ApiLoggerExtensions
{

    [LoggerMessage(Level = LogLevel.Error, Message = "Internal server error.")]
    public static partial void InternalServerError(this ILogger logger, Exception ex);
}
