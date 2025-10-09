using MedSolutions.Domain.Interfaces;
using MedSolutions.Infrastructure.Data.Seed;
using MedSolutions.Infrastructure.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MedSolutions.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration? config = null, IWebHostEnvironment? env = null)
    {
        services.AddScoped<Seeder>();
        services.AddScoped<IPatientRepository, PatientRepository>();
        return services;
    }
}
