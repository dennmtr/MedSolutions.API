using MedSolutions.Domain.Models;
using MedSolutions.Infrastructure.Data.Helpers;
using Microsoft.EntityFrameworkCore;

namespace MedSolutions.Infrastructure.Data.Configurations;

public class BaseEntityConfiguration
{
    public static void Apply(ModelBuilder builder, DbProviderInfo dbProviderInfo)
    {
        IEnumerable<Microsoft.EntityFrameworkCore.Metadata.IMutableEntityType> entityTypes = builder.Model.GetEntityTypes();

        foreach (Microsoft.EntityFrameworkCore.Metadata.IMutableEntityType entityType in entityTypes)
        {
            if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
            {
                Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder entityBuilder = builder.Entity(entityType.ClrType);

                entityBuilder.HasIndex("IsDeleted");
                entityBuilder.HasIndex("DateModified");
                entityBuilder.HasIndex("DateCreated");

                entityBuilder.Property("IsDeleted")
                    .HasDefaultValueSql("0")
                    .ValueGeneratedOnAdd();

                if (dbProviderInfo.IsSqlite())
                {
                    entityBuilder.Property("DateModified")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");
                    entityBuilder.Property("DateCreated")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                }
            }
        }
    }
}
