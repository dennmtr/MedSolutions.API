using MedSolutions.Domain.Models;
using MedSolutions.Infrastructure.Data.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedSolutions.Infrastructure.Data.Configurations;

public class AppointmentConfiguration(DbProviderInfo dbProviderInfo) : IEntityTypeConfiguration<Appointment>
{
    private readonly DbProviderInfo _dbProviderInfo = dbProviderInfo;

    public void Configure(EntityTypeBuilder<Appointment> builder)
    {
        builder.HasIndex(a => new { a.MedicalProfileId, a.Date });
        builder.HasIndex(a => new { a.MedicalProfileId, a.PatientId });

        builder.HasIndex(a => new { a.MedicalProfileId, a.Date, a.PatientId })
        .IsUnique();

        builder.HasIndex(a => new { a.MedicalProfileId, a.AppointmentTypeId });
        builder.HasIndex(a => new { a.MedicalProfileId, a.State });
        builder.HasIndex(a => new { a.MedicalProfileId, a.Priority });

        if (_dbProviderInfo.IsMySql())
        {
            builder.Property(p => p.Id)
                .HasDefaultValueSql("UUID()");
        }

        //if (_dbProviderInfo.IsMsAccess())
        //{
        //    builder.Property(p => p.Id)
        //        .HasDefaultValueSql("CREATEGUID()");
        //}

        builder.Property(a => a.IsHidden)
            .HasDefaultValueSql("0")
            .ValueGeneratedOnAdd();

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

    }
}
