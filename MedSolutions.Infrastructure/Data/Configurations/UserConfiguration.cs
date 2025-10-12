using MedSolutions.Domain.Entities;
using MedSolutions.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedSolutions.Infrastructure.Data.Configurations;

public class UserConfiguration(DatabaseProviderInfo dbProviderInfo) : IEntityTypeConfiguration<User>
{
    private readonly DatabaseProviderInfo _dbProviderInfo = dbProviderInfo;

    public void Configure(EntityTypeBuilder<User> builder)
    {

        builder.HasIndex(p => new { p.Email, p.IsActive });
        builder.HasIndex(p => new { p.LastName, p.IsActive });
        builder.HasIndex(p => new { p.PhoneNumber, p.IsActive });
        builder.HasIndex(p => new { p.MobileNumber, p.IsActive });

        builder.Property(a => a.IsActive)
            .HasDefaultValueSql("1")
            .ValueGeneratedOnAdd();

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
    }
}
