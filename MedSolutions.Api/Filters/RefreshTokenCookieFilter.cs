using MedSolutions.App.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MedSolutions.Api.Filters;

public class RefreshTokenCookieFilter(
    IConfiguration config
) : IAsyncResultFilter
{
    private readonly IConfiguration _config = config;
    public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        if (context.Result is ObjectResult objectResult && objectResult.Value is LoginResponseDTO tokenDetails)
        {
            if (!string.IsNullOrEmpty(tokenDetails.RefreshToken))
            {
                context.HttpContext.Response.Cookies.Append("refreshToken", tokenDetails.RefreshToken, new CookieOptions {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTimeOffset.UtcNow.AddDays(int.Parse(_config["JWT:RefreshTokenExpirationDays"] ?? "7"))
                });

                tokenDetails.RefreshToken = null;
            }
        }

        await next();
    }
}
