using System.Text;
using System.Threading.RateLimiting;
using MedSolutions.Api.Exceptions;
using MedSolutions.Api.Filters;
using MedSolutions.Api.Services;
using MedSolutions.App.Interfaces;
using MedSolutions.App.Mapping;
using MedSolutions.Domain.Models;
using MedSolutions.Infrastructure.Data;
using MedSolutions.Infrastructure.Data.Interceptors;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace MedSolutions.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddApi(this IServiceCollection services, IConfiguration config, IWebHostEnvironment env)
    {
        services.AddScoped<ApiResponseWrapperFilter>();
        services.AddScoped<ICurrentMedicalProfileService, CurrentMedicalProfileService>();
        services.AddHttpContextAccessor();

        services.AddControllers()
            .ConfigureApiBehaviorOptions(options => {
                options.InvalidModelStateResponseFactory = ValidationResponseFactory.CreateInvalidModelStateResponse;
            });

        services.AddControllers(options => {
            options.Filters.Add(new ApiResponseWrapperFilter());
        });

        services.AddAutoMapper(config => {
            config.AddProfile<UserProfile>();
            config.AddProfile<MedicalProfileProfile>();
            config.AddProfile<PatientProfile>();
        });

        services.AddOpenApi();

        services.AddDbContext<MedSolutionsDbContext>(option => {
            string? connectionString = config.GetConnectionString("MedSolutions");

            if (env.IsDevelopment())
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
                string? version = config.GetValue<string>("Database:MySQL:Version");
                option.UseMySql(connectionString,
                new MySqlServerVersion(new Version(version)), x => x.UseNetTopologySuite());
            }
        });

        services.AddIdentity<User, IdentityRole<Guid>>(option => {
            option.Password.RequireDigit = false;
            option.Password.RequiredLength = 6;
            option.Password.RequireNonAlphanumeric = false;
            option.Password.RequireUppercase = false;
            option.Password.RequireLowercase = false;
        })
            .AddRoles<IdentityRole<Guid>>()
            .AddEntityFrameworkStores<MedSolutionsDbContext>()
            .AddDefaultTokenProviders();

        var jwtKey = config["JWT:Key"];

        if (string.IsNullOrEmpty(jwtKey))
        {
            throw new InvalidOperationException("JWT: Security key is not configured.");
        }

        services.AddAuthentication(x => {
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
                    ValidIssuer = config["JWT:Issuer"],
                    ValidAudience = config["JWT:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:Key"]!))
                };
            });

        services.AddRateLimiter(options => {
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
                        ReplenishmentPeriod = TimeSpan.FromMinutes(int.Parse(config["JWT:AccessTokenExpirationMinutes"] ?? "15") - 1),
                        TokensPerPeriod = 1,
                        AutoReplenishment = true
                    });
            });
        });

        if (env.IsProduction())
        {
            services.Configure<ForwardedHeadersOptions>(options => {
                options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });
        }

        return services;
    }
}
