using System.ComponentModel.DataAnnotations;
using AutoMapper;
using MedSolutions.Domain.Models;
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
    RoleManager<IdentityRole> roleManager,
    IConfiguration config,
    ILogger<Seeder> logger,
    ILoggerFactory loggerFactory
    )
{
    private readonly UserManager<User> _userManager = userManager;
    private readonly RoleManager<IdentityRole> _roleManager = roleManager;
    private readonly IConfiguration _config = config;
    private readonly ILogger<Seeder> _logger = logger;
    private readonly MedSolutionsDbContext _dbContext = dbContext;
    private readonly ILoggerFactory _loggerFactory = loggerFactory;

    public async Task SeedAsync()
    {
        if (await _dbContext.Users.AnyAsync())
        {

            _logger.SeedNotify("Seeding skipped: users already exist.");
            return;
        }

        await using Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction transaction = await _dbContext.Database.BeginTransactionAsync();
        try
        {

            Mapper mapper = GetMapper(_loggerFactory);

            UserConfigureDTO? initialUser = _config.GetSection("Data:Seed:InitialUser")
                .Get<UserConfigureDTO>();

            if (initialUser?.MedicalProfile is not null)
            {
                Validator.ValidateObject(
                    initialUser.MedicalProfile,
                    new ValidationContext(initialUser.MedicalProfile),
                    validateAllProperties: true);
                User user = mapper.Map<User>(initialUser);

                IdentityResult result = await _userManager.CreateAsync(user, initialUser.Password);

                if (result.Succeeded)
                {
                    string role = Enums.Role.Admin;
                    if (!await _roleManager.RoleExistsAsync(role))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(role));
                    }
                    await _userManager.AddToRoleAsync(user, role);

                    _logger.SeedNotify($"Initial user '{user.UserName}' created and assigned to '{role}' role.");
                }
                else
                {
                    _logger.SeedFailed($"Failed to create initial user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }
            else
            {
                _logger.SeedWarning("No initial user config found. Skipping initial user seeding.");
            }

            string locale = _config.GetValue<string>("Data:Seed:BogusLocale") ?? "en";
            List<Enums.MedicalSpecialty> medicalSpecialtyIds = await _dbContext.MedicalSpecialties
                .Select(s => s.Id)
                .ToListAsync();

            List<User> fakeUsers = UserFaker.CreateFaker(medicalSpecialtyIds, locale)
            .Generate(5);

            foreach (User fakeUser in fakeUsers)
            {
                await _userManager
            .CreateAsync(fakeUser, "123456");
            }

            _logger.SeedNotify($"Added {fakeUsers.Count} fake profiles.");

            List<MedicalProfile> profiles = await _dbContext.MedicalProfiles
            .Select(p => new MedicalProfile() {
                Id = p.Id,
                MedicalSpecialtyId = p.MedicalSpecialtyId
            })
            .ToListAsync();

            List<AppointmentType> appointmentTypes = await _dbContext.AppointmentTypes
            .Select(a => new AppointmentType() {
                Id = a.Id,
                MedicalSpecialtyId = a.MedicalSpecialtyId
            })
            .ToListAsync();

            foreach (MedicalProfile? profile in profiles)
            {
                List<short> appointmentTypeIds = appointmentTypes
                .Where(t => t.MedicalSpecialtyId == profile.MedicalSpecialtyId)
                .Select(t => t.Id)
                .ToList();

                List<Patient> fakePatients = PatientFaker.CreateFaker(profile.Id, appointmentTypeIds, locale)
                .Generate(50);
                await _dbContext.Patients
                .AddRangeAsync(fakePatients);
            }
            await _dbContext.SaveChangesAsync();

            _logger.SeedNotify($"Added fake patients for {fakeUsers.Count} profiles.");

            List<short> patientPairTypeIds = await _dbContext.PatientPairTypes
                .Select(p => p.Id)
                .ToListAsync();
            foreach (MedicalProfile? profile in profiles)
            {

                List<int> patientIds = await _dbContext.Patients
                    .Where(p => p.MedicalProfileId == profile.Id)
                    .Select(p => p.Id)
                    .Take(20)
                    .ToListAsync();

                List<PatientPair> fakePatientPairs = PatientPairFaker
                    .CreateFaker(profile.Id, patientIds, patientPairTypeIds, locale)
                    .Generate(5);

                fakePatientPairs.ForEach(p => p.Normalize());

                var existingPairs = await _dbContext.PatientPairs
                    .Select(p => new { p.PatientId, p.PairedPatientId })
                    .ToListAsync();

                HashSet<(int, int)> pairSet = new HashSet<(int, int)>(
                    existingPairs.Select(ep => (ep.PatientId, ep.PairedPatientId))
                );

                List<PatientPair> uniquePairs = new List<PatientPair>();

                foreach (PatientPair p in fakePatientPairs)
                {
                    (int PatientId, int PairedPatientId) key = (p.PatientId, p.PairedPatientId);

                    if (pairSet.Add(key))
                    {
                        uniquePairs.Add(p);
                    }
                }


                await _dbContext.PatientPairs.AddRangeAsync(uniquePairs);

            }
            await _dbContext.SaveChangesAsync();
            _logger.SeedNotify($"Added fake pairs for {profiles.Count} profiles.");

        }
        catch (Exception ex)
        {
            _logger.SeedFailed("Database seeding failed. Rolling back transaction.", ex);

            await transaction.RollbackAsync();
            throw;
        }
        await transaction.CommitAsync();
        _logger.SeedSucceeded();
    }

    private static Mapper GetMapper(ILoggerFactory loggerFactory)
    {
        MapperConfiguration config = new MapperConfiguration(cfg => {
            cfg.AddProfile<UserProfile>();
            cfg.AddProfile<MedicalProfileProfile>();
        }, loggerFactory);

        config.CompileMappings();

        Mapper mapper = new Mapper(config);
        return mapper;
    }
}
