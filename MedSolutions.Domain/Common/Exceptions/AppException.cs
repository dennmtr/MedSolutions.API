using System.Net;

namespace MedSolutions.Domain.Common.Exceptions;

public abstract class AppException : Exception
{
    public abstract HttpStatusCode StatusCode { get; }
    public string? Key { get; }
    public string[] Values { get; } = [];

    protected AppException(string message)
        : base(message) { }

    protected AppException(string message, Exception iex)
        : base(message, iex) { }

    protected AppException(string message, params string[] values)
        : base(string.Format(message, values))
    {

        Values = values ?? [];
    }
    protected AppException(string message, string key, params string[] values)
        : base(string.Format(message, values))
    {

        Key = key;
        Values = values ?? [];
    }
}
