using System.Net;
using MedSolutions.App.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace MedSolutions.Api.Exceptions;

public static class ValidationResponseFactory
{
    public static IActionResult CreateInvalidModelStateResponse(ActionContext context)
    {
        var errors = context.ModelState
            .Where(x => x.Value != null && x.Value.Errors.Count > 0)
            .SelectMany(x => x.Value!.Errors)
            .Select(e => e.ErrorMessage)
            .ToArray();

        var response = new ErrorResponseDTO {
            Status = HttpStatusCode.BadRequest,
            Message = "One or more validation errors occurred.",
            Key = "error.validation",
            Values = errors
        };

        return new BadRequestObjectResult(response);
    }
}
