using Microsoft.Extensions.Logging;

namespace MedSolutions.Infrastructure.Logging;

public static partial class SeederLoggerExtensions
{
    [LoggerMessage(Level = LogLevel.Information, Message = "Seeding completed successfully.")]
    public static partial void SeedSucceeded(this ILogger logger);

    [LoggerMessage(Level = LogLevel.Information, Message = "{Message}")]
    public static partial void SeedNotify(this ILogger logger, string message);

    [LoggerMessage(Level = LogLevel.Warning, Message = "{Message}")]
    public static partial void SeedWarning(this ILogger logger, string message);

    [LoggerMessage(Level = LogLevel.Error, Message = "Seeding failed: {Message}")]
    public static partial void SeedFailed(this ILogger logger, string message, Exception? exception = null);
}
