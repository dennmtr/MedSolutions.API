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

        builder.Property(p => p.PatientPairTypeId)
            .HasDefaultValueSql("1")
            .ValueGeneratedOnAdd();

    }
}
