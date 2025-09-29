using MedSolutions.App.Interfaces;
using MedSolutions.App.Services;
using Microsoft.Extensions.DependencyInjection;

namespace MedSolutions.App;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        return services;
    }
}
