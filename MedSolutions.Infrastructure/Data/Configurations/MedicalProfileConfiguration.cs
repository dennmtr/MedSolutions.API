using MedSolutions.Domain.Entities;
using MedSolutions.Infrastructure.Data.Converters;
using MedSolutions.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedSolutions.Infrastructure.Data.Configurations;

public class MedicalProfileConfiguration(DatabaseProviderInfo dbProviderInfo) : IEntityTypeConfiguration<MedicalProfile>
{
    private readonly DatabaseProviderInfo _dbProviderInfo = dbProviderInfo;

    public void Configure(EntityTypeBuilder<MedicalProfile> builder)
    {
        builder.HasIndex(p => p.CompanyName);
        ;
        builder.HasIndex(p => p.Tin);
        ;
        builder.HasIndex(p => p.SubscriptionEndDate);
        ;

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

        builder.Property(a => a.MedicalSpecialtyId)
            .HasDefaultValueSql("1")
            .ValueGeneratedOnAdd();

        builder.HasOne(p => p.MedicalSpecialty)
            .WithMany()
            .HasForeignKey(p => p.MedicalSpecialtyId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.User)
            .WithOne(p => p.MedicalProfile)
            .IsRequired()
            .HasForeignKey<MedicalProfile>(p => p.Id)
            .OnDelete(DeleteBehavior.Cascade);


        builder.HasMany(p => p.PatientPairs)
            .WithOne(p => p.MedicalProfile)
            .HasForeignKey(p => p.MedicalProfileId)
            .HasPrincipalKey(p => p.Id)
            .IsRequired(true)
            .OnDelete(DeleteBehavior.Restrict);


        builder.Property(p => p.SubscriptionStartDate).HasConversion(DateTrimToMinutesConverter.Instance);
        builder.Property(p => p.SubscriptionEndDate).HasConversion(DateTrimToMinutesConverter.Instance);
    }
}
