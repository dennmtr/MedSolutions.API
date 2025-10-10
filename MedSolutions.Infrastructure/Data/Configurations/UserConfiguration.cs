using MedSolutions.Domain.Entities;
using MedSolutions.Infrastructure.Data.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedSolutions.Infrastructure.Data.Configurations;

public class UserConfiguration(DbProviderInfo dbProviderInfo) : IEntityTypeConfiguration<User>
{
    private readonly DbProviderInfo _dbProviderInfo = dbProviderInfo;

    public void Configure(EntityTypeBuilder<User> builder)
    {

        builder.HasIndex(p => p.Email);
        builder.HasIndex(p => p.LastName);
        builder.HasIndex(p => p.PhoneNumber);
        builder.HasIndex(p => p.MobileNumber);
        builder.HasIndex(p => p.IsActive);

        builder.Property(a => a.IsActive)
            .HasDefaultValueSql("1")
            .ValueGeneratedOnAdd();

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
    }
}
