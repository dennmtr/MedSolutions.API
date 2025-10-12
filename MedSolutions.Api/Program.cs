using MedSolutions.Api;
using MedSolutions.Api.Logging;
using MedSolutions.Api.Middlewares;
using MedSolutions.App;
using MedSolutions.Infrastructure;
using MedSolutions.Infrastructure.Data;
using MedSolutions.Infrastructure.Data.Seed;
using Microsoft.EntityFrameworkCore;
using Serilog;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

if (!builder.Environment.IsProduction())
{
    builder.Logging.ClearProviders();
}

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger()
    ;

builder.Host.UseSerilog();

builder.Services
    .AddApi(builder.Configuration, builder.Environment)
    .AddApplication()
    .AddInfrastructure()
    ;

await using WebApplication app = builder.Build();

if (app.Environment.IsProduction())
{
    app.UseForwardedHeaders();
}

app.UseSerilogRequestLogging(options => {
    options.EnrichDiagnosticContext = (diagnosticContext, httpContext) => {
        diagnosticContext.Set("UserAgent", httpContext.Request.Headers.UserAgent.ToString());
    };
});

// DEVELOPMENT ONLY: This section drops, creates, or migrates the database automatically.
// Do NOT use this in production! It will delete your data if EnsureDeletedAsync() is executed.
if (app.Environment.IsDevelopment())
{
    using IServiceScope scope = app.Services.CreateScope();
    IServiceProvider services = scope.ServiceProvider;

    MedSolutionsDbContext dbContext = services.GetRequiredService<MedSolutionsDbContext>();
    ILogger<Program> logger = services.GetRequiredService<ILogger<Program>>();
    Seeder seeder = services.GetRequiredService<Seeder>();

    if ((await dbContext.Database.GetPendingMigrationsAsync()).Any())
    {

        await dbContext.Database.MigrateAsync();
        logger.MigrationsFoundAndApplied();
    }
    else
    {
        // For development purposes only, drop and recreate the database to match the current model.
        // Remove this section if you plan to maintain migrations for schema updates.

        //await dbContext.Database.EnsureDeletedAsync();
        //logger.DatabaseDeleted();
        //await dbContext.Database.EnsureCreatedAsync();
        //logger.DatabaseCreated();
    }

    await seeder.SeedAsync();

}
app.UseMiddleware<ErrorHandlingMiddleware>();


if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

if (app.Environment.IsProduction())
{
    app.UseForwardedHeaders();
}
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    // app.UseSwagger();
    // app.UseSwaggerUI();
    //app.UseDeveloperExceptionPage();
}


app.UseRouting();
app.UseRateLimiter();

app.UseAuthorization();
app.MapControllers();
app.MapFallbackToFile("index.html");
await app.RunAsync();
