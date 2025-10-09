using System.ComponentModel.DataAnnotations;
using System.Net;
using MedSolutions.App.Common.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace MedSolutions.Api.Exceptions;

public static class ValidationResponseFactory
{
    public static IActionResult CreateInvalidModelStateResponse(ActionContext context)
    {
        var modelType = context.ActionDescriptor.Parameters
            .FirstOrDefault()?.ParameterType;

        var jsonOptions = context.HttpContext.RequestServices
            .GetService(typeof(IOptions<JsonOptions>))
            as IOptions<JsonOptions>;

        var namingPolicy = jsonOptions?.Value?.JsonSerializerOptions?.PropertyNamingPolicy;

        string ApplyNamingPolicy(string propertyName)
        {
            return namingPolicy == null ? propertyName : namingPolicy.ConvertName(propertyName);
        }

        var errors = new Dictionary<string, object>();

        foreach (var entry in context.ModelState)
        {
            var key = entry.Key;
            var property = modelType?.GetProperty(key);
            if (property == null)
            {
                continue;
            }

            var attributes = property.GetCustomAttributes(true);
            var errorEntry = new Dictionary<string, object>();
            var values = new Dictionary<string, object>();
            string validationKey = string.Empty;

            string defaultMessage = entry.Value.Errors.FirstOrDefault()?.ErrorMessage
                                    ?? "One or more validation errors occurred.";

            var serializedKey = ApplyNamingPolicy(key);

            foreach (var attr in attributes)
            {
                switch (attr)
                {
                    case RequiredAttribute:
                        validationKey = $"error.validation.{serializedKey}.required";
                        break;

                    case StringLengthAttribute s:
                        validationKey = $"error.validation.{serializedKey}.length";
                        values["minimumLength"] = s.MinimumLength;
                        values["maximumLength"] = s.MaximumLength;
                        break;

                    case RangeAttribute r:
                        validationKey = $"error.validation.{serializedKey}.range";
                        values["minimum"] = r.Minimum;
                        values["maximum"] = r.Maximum;
                        break;

                    default:
                        continue;
                }
            }

            if (!string.IsNullOrEmpty(validationKey))
            {
                errorEntry["message"] = defaultMessage;
                errorEntry["key"] = validationKey;
                errorEntry["values"] = values;
                errors[serializedKey] = errorEntry;
            }
        }

        var response = new ErrorResponseDTO {
            Status = HttpStatusCode.BadRequest,
            Title = "One or more validation errors occurred.",
            Params = errors,
            Instance = context.HttpContext.Request.PathBase + context.HttpContext.Request.Path
        };

        return new BadRequestObjectResult(response);
    }
}
