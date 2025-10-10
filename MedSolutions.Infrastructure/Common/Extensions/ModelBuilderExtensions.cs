using System.Linq.Expressions;
using MedSolutions.Domain.Common.Models;
using MedSolutions.Infrastructure.Data.Converters;
using MedSolutions.Infrastructure.Data.Helpers;
using MedSolutions.Infrastructure.Data.ValueGenerators;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedSolutions.Infrastructure.Common.Extensions;

public static class ModelBuilderExtensions
{
    public static void ConfigureCaseInsensitive(this ModelBuilder modelBuilder, DbProviderInfo dbProviderInfo)
    {
        if (!dbProviderInfo.IsSqlite())
        {
            return;
        }
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            var properties = entity.GetProperties().Where(x => x.ClrType == typeof(string) && x.Name != "GeometryType");

            foreach (var property in properties)
            {
                property.SetCollation("NOCASE");
            }
        }
    }
    public static void ConfigureGuids(this ModelBuilder modelBuilder, DbProviderInfo dbProviderInfo)
    {
        if (!dbProviderInfo.IsSqlite())
        {
            return;
        }
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            var properties = entity.GetProperties().Where(x => x.ClrType == typeof(Guid));

            foreach (var property in properties)
            {
                property.SetColumnType("BLOB");
                property.SetValueConverter(GuidConverter.Instance);
            }

            properties = entity.GetProperties().Where(x => x.ClrType == typeof(Guid?));

            foreach (var property in properties)
            {
                property.SetColumnType("BLOB");
                property.SetValueConverter(NullableGuidConverter.Instance);
            }
        }
    }
    public static void ConfigureSequentialGuids(this ModelBuilder modelBuilder, DbProviderInfo dbProviderInfo)
    {
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            var keyProps = entity.GetProperties()
                .Where(p => p.ClrType == typeof(Guid) && p.IsPrimaryKey());

            foreach (var prop in keyProps)
            {
                prop.SetValueGeneratorFactory((_, __) => new SequentialGuidValueGenerator());
            }
        }
    }

    public static void UseLowercaseGuids(this ModelBuilder modelBuilder, DbProviderInfo dbProviderInfo)
    {
        if (!dbProviderInfo.IsSqlite())
        {
            return;
        }
        foreach (var entity in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var prop in entity.GetProperties())
            {
                if (prop.ClrType == typeof(Guid))
                {
                    prop.SetValueConverter(new LowercaseGuidConverter());
                }
                else if (prop.ClrType == typeof(Guid?))
                {
                    prop.SetValueConverter(new NullableLowercaseGuidConverter());
                }
            }
        }
    }

    public static void ConfigureBusinessEntity(this ModelBuilder modelBuilder, DbProviderInfo dbProviderInfo)
    {
        IEnumerable<IMutableEntityType> entityTypes = modelBuilder.Model.GetEntityTypes();

        foreach (IMutableEntityType entityType in entityTypes)
        {
            if (typeof(BusinessEntity).IsAssignableFrom(entityType.ClrType))
            {
                EntityTypeBuilder entityBuilder = modelBuilder.Entity(entityType.ClrType);

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

    public static void ConfigureBaseEntity(this ModelBuilder modelBuilder, DbProviderInfo dbProviderInfo)
    {
        IEnumerable<IMutableEntityType> entityTypes = modelBuilder.Model.GetEntityTypes();

        foreach (IMutableEntityType entityType in entityTypes)
        {
            if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
            {
                EntityTypeBuilder entityBuilder = modelBuilder.Entity(entityType.ClrType);

                entityBuilder.HasIndex("IsDeleted");
                entityBuilder.HasIndex("DateModified");
                entityBuilder.HasIndex("DateCreated");

                entityBuilder.Property("IsDeleted")
                    .HasDefaultValueSql("0")
                    .ValueGeneratedOnAdd();

                var parameter = Expression.Parameter(entityType.ClrType, "e");
                var property = Expression.Property(parameter, "IsDeleted");

                var notDeleted = Expression.NotEqual(property, Expression.Constant(true, typeof(bool?)));

                var filter = Expression.Lambda(notDeleted, parameter);
                entityBuilder.HasQueryFilter(filter);


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


