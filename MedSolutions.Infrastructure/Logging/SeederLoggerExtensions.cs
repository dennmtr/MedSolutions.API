using Microsoft.Extensions.Logging;

namespace MedSolutions.Infrastructure.Logging;

public static partial class SeederLoggerExtensions
{
    [LoggerMessage(Level = LogLevel.Information, Message = "Seeding completed successfully.")]
    public static partial void SeedSucceeded(this ILogger logger);

    [LoggerMessage(Level = LogLevel.Information, Message = "Seeding skipped: users already exist.")]
    public static partial void SeedSkipped(this ILogger logger);

    [LoggerMessage(Level = LogLevel.Error, Message = "Seeding failed.")]
    public static partial void SeedFailed(this ILogger logger, Exception ex);
    [LoggerMessage(Level = LogLevel.Information, Message = "Added {Count} fake profiles.")]
    public static partial void ProfilesCreatedSuccessfully(this ILogger logger, int count);

}
