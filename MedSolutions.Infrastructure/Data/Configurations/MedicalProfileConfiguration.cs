using MedSolutions.Domain.Models;
using MedSolutions.Infrastructure.Data.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedSolutions.Infrastructure.Data.Configurations;

public class MedicalProfileConfiguration(DbProviderInfo dbProviderInfo) : IEntityTypeConfiguration<MedicalProfile>
{
    private readonly DbProviderInfo _dbProviderInfo = dbProviderInfo;

    public void Configure(EntityTypeBuilder<MedicalProfile> builder)
    {
        builder.HasIndex(p => p.CompanyName);
        builder.HasIndex(p => p.Tin);
        builder.HasIndex(p => p.City);
        builder.HasIndex(p => p.SubscriptionStartDate);
        builder.HasIndex(p => p.SubscriptionEndDate);

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
    }
}
