using MedSolutions.Domain.Entities;
using MedSolutions.Infrastructure.Data.Converters;
using MedSolutions.Infrastructure.Data.ValueGenerators;
using MedSolutions.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedSolutions.Infrastructure.Data.Configurations;

public class PatientConfiguration(DatabaseProviderInfo dbProviderInfo) : IEntityTypeConfiguration<Patient>
{
    private readonly DatabaseProviderInfo _dbProviderInfo = dbProviderInfo;

    public void Configure(EntityTypeBuilder<Patient> builder)
    {
        if (_dbProviderInfo.IsSqlite())
        {
            builder.ToTable(p => p.HasCheckConstraint(
            "CK_AMKA_Length",
                "LENGTH(AMKA) = 11 OR AMKA IS NULL"));

            builder.ToTable(p => p.HasCheckConstraint(
                "CK_PersonalIdNumber_Length",
                "LENGTH(PersonalIdNumber) = 12 OR PersonalIdNumber IS NULL"));
        }

        if (_dbProviderInfo.IsMySql())
        {
            builder.Property(p => p.Id)
                .HasDefaultValueSql("UUID()");
        }

        if (_dbProviderInfo.IsPostgreSql())
        {
            builder.Property(p => p.Id)
                .HasDefaultValueSql("gen_random_uuid()");
        }

        if (!_dbProviderInfo.IsSqlite())
        {
            builder.Property(p => p.Latitude)
                .HasColumnType("decimal(9,6)");
            builder.Property(p => p.Longitude)
                .HasColumnType("decimal(9,6)");
        }

        // For online/offline sync background process
        builder.HasIndex(a => new { a.MedicalProfileId, a.DateModified })
        ;

        builder.HasIndex(p => new { p.MedicalProfileId, p.LastNameLatin })
            .HasFilter("IsDeleted = false")
        ;

        builder.HasIndex(p => new { p.MedicalProfileId, p.LastName })
            .HasFilter("IsDeleted = false")
        ;

        builder.HasIndex(p => new { p.MedicalProfileId, p.MobileNumber })
            .HasFilter("MobileNumber IS NOT NULL AND IsDeleted = false")
        ;

        builder.HasIndex(p => new { p.MedicalProfileId, p.PhoneNumber })
            .HasFilter("PhoneNumber IS NOT NULL AND IsDeleted = false")
        ;

        builder.HasIndex(p => new { p.MedicalProfileId, p.AMKA })
            .IsUnique()
            .HasFilter("AMKA IS NOT NULL AND IsDeleted = false");
        ;

        builder.HasIndex(p => new { p.MedicalProfileId, p.PersonalIdNumber })
            .IsUnique()
            .HasFilter("PersonalIdNumber IS NOT NULL AND IsDeleted = false");
        ;

        builder.Property(a => a.Biopsy)
            .HasDefaultValue(false);

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

        builder.Property(p => p.Latitude).HasConversion(LatLngConverter.Instance);
        builder.Property(p => p.Longitude).HasConversion(LatLngConverter.Instance);
    }
}
