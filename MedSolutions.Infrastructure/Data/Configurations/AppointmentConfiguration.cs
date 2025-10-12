using MedSolutions.Domain.Entities;
using MedSolutions.Infrastructure.Data.Converters;
using MedSolutions.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedSolutions.Infrastructure.Data.Configurations;

public class AppointmentConfiguration(DatabaseProviderInfo dbProviderInfo) : IEntityTypeConfiguration<Appointment>
{
    private readonly DatabaseProviderInfo _dbProviderInfo = dbProviderInfo;

    public void Configure(EntityTypeBuilder<Appointment> builder)
    {
        // For online/offline sync background process
        builder.HasIndex(a => new { a.MedicalProfileId, a.DateModified })
        ;

        builder.HasIndex(a => new { a.MedicalProfileId, a.Date })
                .HasFilter("IsDeleted = false");
        ;

        builder.HasIndex(a => new { a.MedicalProfileId, a.Date, a.PatientId })
            .IsUnique()
            .HasFilter("IsDeleted = false");
        ;

        if (_dbProviderInfo.IsMySql())
        {
            builder.Property(p => p.Id)
                .HasDefaultValueSql("UUID()");

            builder.Property("Date")
                .HasColumnType("DATETIME(0)");
        }

        if (_dbProviderInfo.IsPostgreSql())
        {
            builder.Property(p => p.Id)
                .HasDefaultValueSql("gen_random_uuid()");
            builder.Property("Date")
                .HasColumnType("TIMESTAMP WITHOUT TIME ZONE");
        }

        builder.Property(a => a.IsHidden)
            .HasDefaultValue(false);

        builder.HasOne(p => p.MedicalProfile)
            .WithMany(p => p.Appointments)
            .HasForeignKey(p => p.MedicalProfileId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.Patient)
            .WithMany(p => p.Appointments)
            .HasForeignKey(p => p.PatientId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);


        builder.HasOne(p => p.AppointmentType)
            .WithMany()
            .HasForeignKey(p => p.AppointmentTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(p => p.Date).HasConversion(DateTrimToMinutesConverter.Instance);

    }
}
