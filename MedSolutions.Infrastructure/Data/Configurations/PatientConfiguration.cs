using MedSolutions.Domain.Models;
using MedSolutions.Infrastructure.Data.Helpers;
using MedSolutions.Infrastructure.Data.ValueGenerators;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedSolutions.Infrastructure.Data.Configurations;

public class PatientConfiguration(DbProviderInfo dbProviderInfo) : IEntityTypeConfiguration<Patient>
{
    private readonly DbProviderInfo _dbProviderInfo = dbProviderInfo;

    public void Configure(EntityTypeBuilder<Patient> builder)
    {
        builder.Property(p => p.AMKA)
            .HasMaxLength(9)
            .IsRequired(false);

        builder.Property(p => p.PersonalIdNumber)
            .HasMaxLength(12)
            .IsRequired(false);

        if (_dbProviderInfo.IsMySql() || _dbProviderInfo.IsSqlite())
        {
            builder.ToTable(p => p.HasCheckConstraint(
            "CK_AMKA_Length",
                "LENGTH(AMKA) = 11 OR AMKA IS NULL"));

            builder.ToTable(p => p.HasCheckConstraint(
                "CK_PersonalIdNumber_Length",
                "LENGTH(PersonalIdNumber) = 12 OR PersonalIdNumber IS NULL"));
        }

        builder.HasIndex(p => new { p.MedicalProfileId, p.LastName });
        builder.HasIndex(p => new { p.MedicalProfileId, p.LastNameLatin });

        builder.HasIndex(p => new { p.MedicalProfileId, p.AMKA })
            .IsUnique();

        builder.HasIndex(p => new { p.MedicalProfileId, p.PersonalIdNumber })
            .IsUnique();

        builder.Property(a => a.Biopsy)
            .HasDefaultValueSql("0")
            .ValueGeneratedOnAdd();

        if (_dbProviderInfo.IsMySql())
        {
            builder.Property(p => p.Position)
                .HasColumnType("POINT")
                .HasSrid(4326)
                .HasDefaultValueSql("ST_GeomFromText('POINT(0 0)', 4326)")
                .HasValueGenerator<PointValueGenerator>();
            ;
            builder.HasIndex(p => p.Position)
                .IsSpatial();
        }
        else
        {
            builder.Ignore(p => p.Position);
        }

        builder.HasOne(p => p.MedicalProfile)
            .WithMany(p => p.Patients)
            .HasForeignKey(p => p.MedicalProfileId)
            .HasPrincipalKey(p => p.Id)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

    }
}
