using System.Text;
using System.Threading.RateLimiting;
using MedSolutions.Api;
using MedSolutions.Api.Exceptions;
using MedSolutions.Api.Filters;
using MedSolutions.Api.Logging;
using MedSolutions.Api.Middlewares;
using MedSolutions.App;
using MedSolutions.App.Mapping;
using MedSolutions.Domain.Models;
using MedSolutions.Infrastructure;
using MedSolutions.Infrastructure.Data;
using MedSolutions.Infrastructure.Data.Interceptors;
using MedSolutions.Infrastructure.Data.Seed;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);



builder.Logging.ClearProviders();

builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options => {
        options.InvalidModelStateResponseFactory = ValidationResponseFactory.CreateInvalidModelStateResponse;
    });

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services
    .AddApplication()
    .AddInfrastructure()
    .AddApi()
    ;

builder.Services.AddControllers(options => {
    options.Filters.Add(new ApiResponseWrapperFilter());
});
builder.Services.AddHttpContextAccessor();


builder.Services.AddAutoMapper(config => {
    config.AddProfile<UserProfile>();
    config.AddProfile<MedicalProfileProfile>();
});
builder.Services.AddOpenApi();

builder.Services.AddDbContext<MedSolutionsDbContext>(option => {
    string? connectionString = builder.Configuration.GetConnectionString("MedSolutions");

    if (builder.Environment.IsDevelopment())
    {
        if (string.IsNullOrEmpty(connectionString))
        {
            string dbPath = Path.Combine(Path.GetTempPath(), "MedSolutions", "data.sqlite");
            connectionString = $"Data Source=\"{dbPath}\"";
        }
        option.UseSqlite(connectionString);
        option.EnableSensitiveDataLogging();
        option.EnableDetailedErrors();
        option.AddInterceptors(new SqliteForeignKeyEnabler());
    }
    else
    {
        string? version = builder.Configuration.GetValue<string>("Database:MySQL:Version");
        option.UseMySql(connectionString,
        new MySqlServerVersion(new Version(version)), x => x.UseNetTopologySuite());
    }
});

builder.Services.AddIdentity<User, IdentityRole>(option => {
    option.Password.RequireDigit = false;
    option.Password.RequiredLength = 6;
    option.Password.RequireNonAlphanumeric = false;
    option.Password.RequireUppercase = false;
    option.Password.RequireLowercase = false;
})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<MedSolutionsDbContext>()
    .AddDefaultTokenProviders();

var jwtKey = builder.Configuration["JWT:Key"];

if (string.IsNullOrEmpty(jwtKey))
{
    throw new InvalidOperationException("JWT: Security key is not configured.");
}

builder.Services.AddAuthentication(x => {
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(o => {
        // o.SaveToken = true;
        o.TokenValidationParameters = new TokenValidationParameters {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            ValidAudience = builder.Configuration["JWT:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]!))
        };
    });

if (builder.Environment.IsProduction())
{
    builder.Services.Configure<ForwardedHeadersOptions>(options => {
        options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    });
}

builder.Services.AddRateLimiter(options => {
    options.AddPolicy("RefreshTokenPerCookie", context => {

        var refreshToken = context.Request.Cookies.FirstOrDefault(c => c.Key == "RefreshToken").Value ?? "";

        return string.IsNullOrEmpty(refreshToken)
            ? throw new InvalidOperationException("Cannot determine refresh token for rate limiting.")
            : RateLimitPartition.GetTokenBucketLimiter(
            partitionKey: refreshToken,
            factory: _ => new TokenBucketRateLimiterOptions {
                TokenLimit = 1,
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                QueueLimit = 0,
                ReplenishmentPeriod = TimeSpan.FromMinutes(int.Parse(builder.Configuration["JWT:AccessTokenExpirationMinutes"] ?? "15") - 1),
                TokensPerPeriod = 1,
                AutoReplenishment = true
            });
    });
});

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
// ⚠️ Do NOT use this in production! It will delete your data if EnsureDeletedAsync() is executed.
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
        logger.DatabaseWarning("Pending migrations found and applied to the database.");
    }
    else
    {
        // For development purposes only, drop and recreate the database to match the current model.
        // Remove this section if you plan to maintain migrations for schema updates.
        // await dbContext.Database.EnsureDeletedAsync();
        // logger.DatabaseWarning("Database deleted because no migrations exist.");
        await dbContext.Database.EnsureCreatedAsync();
        logger.DatabaseWarning("Database created fresh from the current model.");
    }

    await seeder.SeedAsync();
}
app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseHttpsRedirection();

if (app.Environment.IsProduction())
{
    app.UseForwardedHeaders();


}
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    // app.UseSwagger();
    // app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}


app.UseRouting();
app.UseRateLimiter();

app.UseAuthorization();
app.MapControllers();
app.MapFallbackToFile("index.html");
await app.RunAsync();
