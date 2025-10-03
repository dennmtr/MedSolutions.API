using System.Net;
using MedSolutions.Api.Exceptions;
using MedSolutions.Api.Logging;
using MedSolutions.App.DTOs;
using MedSolutions.Domain.Exceptions;

namespace MedSolutions.Api.Middlewares;

public class ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger = logger;

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            var statusCode = ExceptionMapper.GetStatusCode(ex);

            if (statusCode == HttpStatusCode.InternalServerError)
            {
                _logger.InternalServerError(ex);
            }

            var response = new ErrorResponseDTO {
                Status = statusCode,
                Message = ex.Message
            };

            if (ex is AppException aex)
            {
                response.Key = aex.Key;
                response.Values = aex.Values;
            }
            else
            {
                response.Key = "error.unexpected";
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;
            await context.Response.WriteAsJsonAsync(response, context.RequestAborted);
        }
    }


}
