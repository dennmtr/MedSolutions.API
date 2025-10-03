using MedSolutions.Api.Filters;

namespace MedSolutions.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddApi(this IServiceCollection services)
    {
        services.AddScoped<ApiResponseWrapperFilter>();
        return services;
    }
}
