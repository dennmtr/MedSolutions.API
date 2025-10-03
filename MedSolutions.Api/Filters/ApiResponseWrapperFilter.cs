using System.Net;
using MedSolutions.App.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MedSolutions.Api.Filters;

public class ApiResponseWrapperFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {

        var executedContext = await next();

        if (executedContext.Result is ObjectResult objectResult)
        {

            var data = objectResult.Value;
            var statusCode = (HttpStatusCode)(objectResult.StatusCode ?? 200);

            if (statusCode == HttpStatusCode.NoContent)
            {
                return;
            }

            var responseDtoType = typeof(ResponseDTO<>).MakeGenericType(data?.GetType() ?? typeof(object));
            var responseDto = Activator.CreateInstance(responseDtoType, data, statusCode);

            executedContext.Result = new ObjectResult(responseDto) {
                StatusCode = objectResult.StatusCode
            };
        }

        else if (executedContext.Result is NoContentResult)
        {
            return;
        }
    }
}
