using MedSolutions.App.Interfaces;
using MedSolutions.Domain.Entities;
using MedSolutions.Infrastructure.Data.Configurations;
using MedSolutions.Infrastructure.Data.Interceptors;
using MedSolutions.Infrastructure.Extensions;
using MedSolutions.Shared.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace MedSolutions.Infrastructure.Data;

public class MedSolutionsDbContext(DbContextOptions<MedSolutionsDbContext> options,
IAppContextProvider appContextProvider) : IdentityDbContext<User, IdentityRole<Guid>, Guid>(options)
{

    private readonly IAppContextProvider _appContextProvider = appContextProvider;

    public required DbSet<MedicalProfile> MedicalProfiles { get; set; }
    public required DbSet<MedicalSpecialty> MedicalSpecialties { get; set; }
    public required DbSet<Patient> Patients { get; set; }
    public required DbSet<AppointmentType> AppointmentTypes { get; set; }
    public required DbSet<Appointment> Appointments { get; set; }
    public required DbSet<PatientPairType> PatientPairTypes { get; set; }
    public required DbSet<PatientPair> PatientPairs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(new UpdateTimestampsInterceptor());
        optionsBuilder.AddInterceptors(new PatientPairSaveChangesInterceptor());
        optionsBuilder.AddInterceptors(new PatientSaveChangesInterceptor());
        optionsBuilder.AddInterceptors(new SoftDeleteInterceptor());
    }
    protected override void OnModelCreating(ModelBuilder builder)
    {

        base.OnModelCreating(builder);

        var dbProviderInfo = new DatabaseProviderInfo(Database.ProviderName);

        builder.ConfigureBaseEntity(dbProviderInfo);
        builder.ConfigureBusinessEntity(dbProviderInfo);

        builder.ApplyConfiguration(new UserConfiguration(dbProviderInfo));
        builder.ApplyConfiguration(new MedicalProfileConfiguration(dbProviderInfo));
        builder.ApplyConfiguration(new MedicalSpecialtyConfiguration(dbProviderInfo));
        builder.ApplyConfiguration(new PatientConfiguration(dbProviderInfo));
        builder.ApplyConfiguration(new AppointmentTypeConfiguration(dbProviderInfo));
        builder.ApplyConfiguration(new AppointmentConfiguration(dbProviderInfo));
        builder.ApplyConfiguration(new PatientPairTypeConfiguration(dbProviderInfo));
        builder.ApplyConfiguration(new PatientPairConfiguration(dbProviderInfo));

        builder.ConfigureSequentialGuids(dbProviderInfo);
        builder.UseLowercaseGuids(dbProviderInfo);
        builder.ConfigureCaseInsensitive(dbProviderInfo);

        builder.ApplyFilters(_appContextProvider);

    }

}
