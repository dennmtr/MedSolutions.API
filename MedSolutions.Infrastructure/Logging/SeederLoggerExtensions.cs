using Microsoft.Extensions.Logging;

namespace MedSolutions.Infrastructure.Logging;

public static partial class SeederLoggerExtensions
{
    [LoggerMessage(Level = LogLevel.Information, Message = "Seeding completed successfully.")]
    public static partial void SeedSucceeded(this ILogger logger);

    [LoggerMessage(Level = LogLevel.Information, Message = "Seeding skipped: users already exist.")]
    public static partial void SeedSkipped(this ILogger logger);

    [LoggerMessage(Level = LogLevel.Error, Message = "Seeding failed: Database seeding failed. Rolling back transaction.")]
    public static partial void SeedFailed(this ILogger logger, Exception? ex = null);

    [LoggerMessage(Level = LogLevel.Error, Message = "Seeding failed: {ResultErrors}")]
    public static partial void SeedFailed(this ILogger logger, string resultErrors);

    [LoggerMessage(Level = LogLevel.Warning, Message = "No initial user config found. Skipping initial user seeding.")]
    public static partial void UserConfigurationIsMissing(this ILogger logger);

    [LoggerMessage(Level = LogLevel.Information, Message = "Initial user {UserName} created and assigned to {Role} role.")]
    public static partial void UserCreatedSuccessfully(this ILogger logger, string userName, string role);

    [LoggerMessage(Level = LogLevel.Information, Message = "Added {Count} fake profiles.")]
    public static partial void ProfilesCreatedSuccessfully(this ILogger logger, int count);

    [LoggerMessage(Level = LogLevel.Information, Message = "Added fake patients for {Count} profiles.")]
    public static partial void PatientsCreatedSuccessfully(this ILogger logger, int count);

    [LoggerMessage(Level = LogLevel.Information, Message = "Added fake pairs for {Count} profiles.")]
    public static partial void PairsCreatedSuccessfully(this ILogger logger, int count);
}
