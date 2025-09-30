using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Security;
using Microsoft.EntityFrameworkCore;

namespace MedSolutions.Api.Exceptions;

public static class ExceptionMapper
{
    private static readonly Dictionary<Type, HttpStatusCode> _exceptionStatusCodeMap = new()
    {
        { typeof(UnauthorizedAccessException), HttpStatusCode.Unauthorized },
        { typeof(SecurityException), HttpStatusCode.Forbidden },
        { typeof(ArgumentException), HttpStatusCode.BadRequest },
        { typeof(ArgumentNullException), HttpStatusCode.BadRequest },
        { typeof(FormatException), HttpStatusCode.BadRequest },
        { typeof(InvalidOperationException), HttpStatusCode.BadRequest },
        { typeof(ValidationException), HttpStatusCode.BadRequest },
        { typeof(KeyNotFoundException), HttpStatusCode.NotFound },
        { typeof(FileNotFoundException), HttpStatusCode.NotFound },
        { typeof(DirectoryNotFoundException), HttpStatusCode.NotFound },
        { typeof(DuplicateWaitObjectException), HttpStatusCode.Conflict },
        { typeof(DbUpdateConcurrencyException), HttpStatusCode.Conflict },
        { typeof(TaskCanceledException), HttpStatusCode.RequestTimeout },
        { typeof(OperationCanceledException), HttpStatusCode.RequestTimeout },
        { typeof(TimeoutException), HttpStatusCode.RequestTimeout },
        { typeof(HttpRequestException), HttpStatusCode.BadGateway },
    };

    public static HttpStatusCode GetStatusCode(Exception ex) => _exceptionStatusCodeMap.TryGetValue(ex.GetType(), out var status) ? status : HttpStatusCode.InternalServerError;

    public static string GetMessage(Exception ex, HttpStatusCode statusCode)
    {
        return statusCode == HttpStatusCode.InternalServerError
            ? "unexpected.error"
            : ex.Message;
    }
}
