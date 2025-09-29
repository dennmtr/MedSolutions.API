using MedSolutions.Infrastructure.Data.Seed;
using Microsoft.Extensions.DependencyInjection;

namespace MedSolutions.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<Seeder>();
        return services;
    }
}
