using System.ComponentModel.DataAnnotations;
using System.Net;
using MedSolutions.Api.DTOs;
using MedSolutions.Api.Exceptions;

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
                _logger.LogError(ex, "Unhandled exception occurred");
            }

            var message = ExceptionMapper.GetMessage(ex, statusCode);

            var response = new ErrorResponseDTO {
                Success = false,
                Status = statusCode,
                Message = message
            };

            if (ex is ValidationException vex)
            {
                response.Errors = new[] { vex.Message };
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;
            await context.Response.WriteAsJsonAsync(response, context.RequestAborted);
        }
    }


}
