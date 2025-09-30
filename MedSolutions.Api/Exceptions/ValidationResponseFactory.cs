using System.Net;
using MedSolutions.Api.DTOs;
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
            Success = false,
            Status = HttpStatusCode.BadRequest,
            Message = "validation.error",
            Errors = errors
        };

        return new BadRequestObjectResult(response);
    }
}
