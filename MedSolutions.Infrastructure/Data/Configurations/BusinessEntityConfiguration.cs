using MedSolutions.Domain.Models;
using MedSolutions.Infrastructure.Data.Helpers;
using Microsoft.EntityFrameworkCore;

namespace MedSolutions.Infrastructure.Data.Configurations;

public class BusinessEntityConfiguration
{
    public static void Apply(ModelBuilder builder, DbProviderInfo dbProviderInfo)
    {
        IEnumerable<Microsoft.EntityFrameworkCore.Metadata.IMutableEntityType> entityTypes = builder.Model.GetEntityTypes();

        foreach (Microsoft.EntityFrameworkCore.Metadata.IMutableEntityType entityType in entityTypes)
        {
            if (typeof(BusinessEntity).IsAssignableFrom(entityType.ClrType))
            {
                Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder entityBuilder = builder.Entity(entityType.ClrType);

                entityBuilder.HasIndex("DisplayOrder")
                .IsUnique();

                entityBuilder.Property("BusinessId")
                    .IsRequired();

                entityBuilder.HasIndex("BusinessId")
                .IsUnique();

                if (dbProviderInfo.IsSqlite())
                {
                    entityBuilder.ToTable(t => t.HasCheckConstraint(
                        "CK_BusinessId_Length",
                        "LENGTH(BusinessId) BETWEEN 5 AND 100"
                    ));
                }

                if (dbProviderInfo.IsMySql())
                {
                    entityBuilder.ToTable(t => t.HasCheckConstraint(
                        "CK_BusinessId_Length",
                        "CHAR_LENGTH(BusinessId) BETWEEN 5 AND 100"
                    ));
                }

            }
        }
    }
}
