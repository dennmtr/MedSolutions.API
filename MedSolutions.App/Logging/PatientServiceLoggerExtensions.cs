using Microsoft.Extensions.Logging;

namespace MedSolutions.App.Logging;

public static partial class PatientSeriveLoggerExtensions
{

    [LoggerMessage(Level = LogLevel.Error, Message = "Error occurred while getting patients.")]
    public static partial void ErrorGettingPatients(this ILogger logger, Exception ex);
}
