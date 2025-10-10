using System.ComponentModel.DataAnnotations;
using AutoMapper;
using MedSolutions.Domain.Entities;
using MedSolutions.Infrastructure.Data.Seed.DTOs;
using MedSolutions.Infrastructure.Data.Seed.Fakers;
using MedSolutions.Infrastructure.Data.Seed.Mapping;
using MedSolutions.Infrastructure.Logging;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Enums = MedSolutions.Domain.Enums;

namespace MedSolutions.Infrastructure.Data.Seed;

public class Seeder(
    MedSolutionsDbContext dbContext,
    UserManager<User> userManager,
    RoleManager<IdentityRole<Guid>> roleManager,
    IConfiguration config,
    ILogger<Seeder> logger,
    ILoggerFactory loggerFactory
    )
{
    private readonly UserManager<User> _userManager = userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager = roleManager;
    private readonly IConfiguration _config = config;
    private readonly ILogger<Seeder> _logger = logger;
    private readonly MedSolutionsDbContext _dbContext = dbContext;
    private readonly ILoggerFactory _loggerFactory = loggerFactory;

    public async Task SeedAsync()
    {
        if (await _dbContext.Users.AnyAsync())
        {
            _logger.SeedSkipped();
            return;
        }

        using var transaction = await _dbContext.Database.BeginTransactionAsync();

        try
        {
            var locale = _config.GetValue<string>("Data:Seed:BogusLocale") ?? "en";

            var medicalSpecialtyIds = await _dbContext.MedicalSpecialties
                .Select(s => s.Id)
                .ToListAsync();

            var patientPairTypeIds = await _dbContext.PatientPairTypes
                .Select(p => p.Id)
                .ToListAsync();

            var appointmentTypes = await _dbContext.AppointmentTypes
                .GroupBy(a => a.MedicalSpecialtyId)
                .ToDictionaryAsync(
                    g => (short)g.Key,
                    g => g.Select(a => a.Id).ToList()
                );

            User? initialUser = default;

            var faker = UserFaker.CreateFaker(medicalSpecialtyIds, patientPairTypeIds, appointmentTypes, locale);

            var fakeUsers = new List<User>();

            var initialUserConfig = _config.GetSection("Data:Seed:InitialUser")
                .Get<UserConfigureDTO>();

            if (initialUserConfig?.MedicalProfile is not null)
            {
                Validator.ValidateObject(
                    initialUserConfig.MedicalProfile,
                    new ValidationContext(initialUserConfig.MedicalProfile),
                    validateAllProperties: true);
                var mapper = GetMapper(_loggerFactory);
                initialUser = mapper.Map<User>(initialUserConfig);
                faker.Populate(initialUser);
                fakeUsers.Add(initialUser);
            }

            fakeUsers.AddRange(faker.Generate(5));

            foreach (var user in fakeUsers)
            {
                var password = user.Email != initialUserConfig?.Email ? "123456" : initialUserConfig!.Password;
                await _userManager.CreateAsync(user, password);
            }

            _logger.PairsCreatedSuccessfully(fakeUsers.Count);

            if (initialUser != null)
            {
                string role = Enums.Role.Admin;
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    await _roleManager.CreateAsync(new IdentityRole<Guid>(role));
                }
                await _userManager.AddToRoleAsync(initialUser, role);
            }
        }
        catch (Exception ex)
        {
            _logger.SeedFailed(ex);

            await transaction.RollbackAsync();
            throw;
        }
        await transaction.CommitAsync();
        _logger.SeedSucceeded();
    }
    private static Mapper GetMapper(ILoggerFactory loggerFactory)
    {
        MapperConfiguration config = new(cfg => {
            cfg.AddProfile<UserProfile>();
            cfg.AddProfile<MedicalProfileProfile>();
        }, loggerFactory);

        config.CompileMappings();

        Mapper mapper = new(config);
        return mapper;
    }
}
