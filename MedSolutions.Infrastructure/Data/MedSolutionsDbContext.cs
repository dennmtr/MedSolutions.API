using MedSolutions.App.Interfaces;
using MedSolutions.Domain.Models;
using MedSolutions.Infrastructure.Data.Configurations;
using MedSolutions.Infrastructure.Data.Helpers;
using MedSolutions.Infrastructure.Data.Interceptors;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace MedSolutions.Infrastructure.Data;

public class MedSolutionsDbContext(DbContextOptions<MedSolutionsDbContext> options,
        ICurrentMedicalProfileService currentMedicalProfileService) : IdentityDbContext<User>(options)
{
    public required DbSet<MedicalProfile> MedicalProfiles { get; set; }
    public required DbSet<MedicalSpecialty> MedicalSpecialties { get; set; }
    public required DbSet<Patient> Patients { get; set; }
    public required DbSet<AppointmentType> AppointmentTypes { get; set; }
    public required DbSet<Appointment> Appointments { get; set; }
    public required DbSet<PatientPairType> PatientPairTypes { get; set; }
    public required DbSet<PatientPair> PatientPairs { get; set; }

    public string? CurrentMedicalProfileId { get; } = currentMedicalProfileService.MedicalProfileId;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(new UpdateTimestampsInterceptor());
        optionsBuilder.AddInterceptors(new PatientPairNormalizeInterceptor());
        optionsBuilder.AddInterceptors(new PatientSaveChangesInterceptor());
    }
    protected override void OnModelCreating(ModelBuilder builder)
    {

        base.OnModelCreating(builder);

        var dbProviderInfo = new DbProviderInfo(Database.ProviderName);

        builder.ApplyConfiguration(new UserConfiguration(dbProviderInfo));
        builder.ApplyConfiguration(new MedicalProfileConfiguration(dbProviderInfo));
        builder.ApplyConfiguration(new MedicalSpecialtyConfiguration(dbProviderInfo));
        builder.ApplyConfiguration(new PatientConfiguration(dbProviderInfo));
        builder.ApplyConfiguration(new AppointmentTypeConfiguration(dbProviderInfo));
        builder.ApplyConfiguration(new AppointmentConfiguration(dbProviderInfo));
        builder.ApplyConfiguration(new PatientPairTypeConfiguration(dbProviderInfo));
        builder.ApplyConfiguration(new PatientPairConfiguration(dbProviderInfo));

        BusinessEntityConfiguration.Apply(builder, dbProviderInfo);
        BaseEntityConfiguration.Apply(builder, dbProviderInfo);

        builder.Entity<Patient>()
            .HasQueryFilter(p => p.MedicalProfileId == CurrentMedicalProfileId);

    }

}
