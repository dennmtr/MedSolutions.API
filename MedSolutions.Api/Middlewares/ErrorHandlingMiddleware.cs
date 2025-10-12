using System.Net;
using MedSolutions.Api.Exceptions;
using MedSolutions.Api.Logging;
using MedSolutions.App.Common.DTOs;
using MedSolutions.Domain.Exceptions;
using Microsoft.Extensions.Options;
using JsonOptions = Microsoft.AspNetCore.Mvc.JsonOptions;

namespace MedSolutions.Api.Middlewares;

public class ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger, IWebHostEnvironment env)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger = logger;
    private readonly IWebHostEnvironment _env = env;

    public async Task Invoke(HttpContext context)
    {

        var jsonOptions = context.RequestServices
            .GetService(typeof(IOptions<JsonOptions>))
            as IOptions<JsonOptions>;

        var namingPolicy = jsonOptions?.Value?.JsonSerializerOptions?.PropertyNamingPolicy;

        string ApplyNamingPolicy(string propertyName)
        {
            return namingPolicy == null ? propertyName : namingPolicy.ConvertName(propertyName);
        }

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
                Title = ex.Message,
                Type = ex.GetType().Name,
                Params = new Dictionary<string, object>() {
                    { "key", $"error.exception.{ApplyNamingPolicy(ex.GetType().Name)}"},
                    { "values", new List<string>()}
                },
                Instance = context.Request.PathBase + context.Request.Path
            };

            if (ex is AppException aex)
            {
                response.Title = aex.Message;
                if (!string.IsNullOrWhiteSpace(aex.Key))
                {
                    response.Params["key"] = aex.Key;
                    response.Params["values"] = aex.Values
                        .Select(v => v.Trim())
                        .Where(v => !string.IsNullOrWhiteSpace(v))
                        .ToList();
                }
            }

            if (_env.IsDevelopment())
            {
                response.StackTrace = ex.StackTrace;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;
            await context.Response.WriteAsJsonAsync(response, context.RequestAborted);
        }
    }


}
