using MedSolutions.Domain.Models;
using MedSolutions.Infrastructure.Data.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedSolutions.Infrastructure.Data.Configurations;

public class PatientPairConfiguration(DbProviderInfo dbProviderInfo) : IEntityTypeConfiguration<PatientPair>
{
    private readonly DbProviderInfo _dbProviderInfo = dbProviderInfo;

    public void Configure(EntityTypeBuilder<PatientPair> builder)
    {
        builder.HasIndex(p => new { p.MedicalProfileId, p.PatientId, p.PairedPatientId })
        .IsUnique();

        if (_dbProviderInfo.IsMySql() || _dbProviderInfo.IsSqlite())
        {
            builder.ToTable(p => p.HasCheckConstraint(
                "CK_PatientPair_DifferentPatients",
                "PatientId <> PairedPatientId")
            );
        }

        builder.Property(p => p.PatientPairTypeId)
            .HasDefaultValueSql("1")
            .ValueGeneratedOnAdd();

        builder.HasOne(p => p.Patient)
            .WithMany(p => p.FirstGroupPair)
            .HasForeignKey(p => p.PatientId)
            .HasPrincipalKey(p => p.Id)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.PairedPatient)
            .WithMany(p => p.SecondGroupPair)
            .HasForeignKey(p => p.PairedPatientId)
            .HasPrincipalKey(p => p.Id)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
