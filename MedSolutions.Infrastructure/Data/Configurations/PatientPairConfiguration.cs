using MedSolutions.Domain.Entities;
using MedSolutions.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace MedSolutions.Infrastructure.Data.Configurations;

public class PatientPairConfiguration(DatabaseProviderInfo dbProviderInfo) : IEntityTypeConfiguration<PatientPair>
{
    private readonly DatabaseProviderInfo _dbProviderInfo = dbProviderInfo;

    public void Configure(EntityTypeBuilder<PatientPair> builder)
    {
        // For online/offline sync background process
        builder.HasIndex(a => new { a.MedicalProfileId, a.DateModified })
        ;

        builder.HasIndex(p => new { p.MedicalProfileId, p.PatientId, p.PairedPatientId })
            .HasFilter("IsDeleted = false")
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

        if (_dbProviderInfo.IsPostgreSql())
        {
            builder.Property(p => p.Id)
                .HasDefaultValueSql("gen_random_uuid()");
        }

        builder.HasOne(p => p.PatientPairType)
            .WithMany()
            .HasForeignKey(p => p.PatientPairTypeId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
