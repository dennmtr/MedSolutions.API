namespace MedSolutions.Api.Logging;

public static partial class ProgramLoggerExtensions
{
    [LoggerMessage(Level = LogLevel.Warning, Message = "Pending migrations found and applied to the database.")]
    public static partial void MigrationsFoundAndApplied(this ILogger logger);
    [LoggerMessage(Level = LogLevel.Warning, Message = "Database deleted because no migrations exist.")]
    public static partial void DatabaseDeleted(this ILogger logger);
    [LoggerMessage(Level = LogLevel.Warning, Message = "Database created fresh from the current model.")]
    public static partial void DatabaseCreated(this ILogger logger);
}
