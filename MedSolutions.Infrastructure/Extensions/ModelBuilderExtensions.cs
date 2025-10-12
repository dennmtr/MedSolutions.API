using System.Linq.Expressions;
using MedSolutions.App.Interfaces;
using MedSolutions.Domain.Common.Entities;
using MedSolutions.Domain.Entities;
using MedSolutions.Infrastructure.Data.Converters;
using MedSolutions.Infrastructure.Data.ValueGenerators;
using MedSolutions.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MedSolutions.Infrastructure.Extensions;

public static class ModelBuilderExtensions
{
    public static void ConfigureCaseInsensitive(this ModelBuilder modelBuilder, DatabaseProviderInfo dbProviderInfo)
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
    public static void ConfigureGuids(this ModelBuilder modelBuilder, DatabaseProviderInfo dbProviderInfo)
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
    public static void ConfigureSequentialGuids(this ModelBuilder modelBuilder, DatabaseProviderInfo dbProviderInfo)
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

    public static void UseLowercaseGuids(this ModelBuilder modelBuilder, DatabaseProviderInfo dbProviderInfo)
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

    public static void ConfigureBusinessEntity(this ModelBuilder modelBuilder, DatabaseProviderInfo dbProviderInfo)
    {
        IEnumerable<IMutableEntityType> entityTypes = modelBuilder.Model.GetEntityTypes();

        foreach (IMutableEntityType entityType in entityTypes)
        {
            if (typeof(BusinessEntity).IsAssignableFrom(entityType.ClrType))
            {
                EntityTypeBuilder entityBuilder = modelBuilder.Entity(entityType.ClrType);

                var medicalSpecialtyProperty = entityType.FindProperty("MedicalSpecialtyId");

                if (medicalSpecialtyProperty != null)
                {
                    entityBuilder.HasIndex("MedicalSpecialtyId", "DisplayOrder")
                        .IsUnique()
                        ;
                }
                else
                {
                    entityBuilder.HasIndex("DisplayOrder")
                        .IsUnique()
                        ;
                }

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

    public static void ApplyFilters(this ModelBuilder modelBuilder, IAppContextProvider appContextProvider)
    {
        // Don't store profileProvider.MedicalProfileId in a local variable (var medicalProfileId =).
        // EF caches query filters at model build time, locals become constants.
        // Access the provider property directly so its evaluated dynamically per query.

        if (appContextProvider.IsVisibilityModeEnabled)
        {
            modelBuilder.Entity<Appointment>()
                .HasQueryFilter(p => p.MedicalProfileId == appContextProvider.CurrentMedicalProfileId && !p.IsHidden && !p.IsDeleted);
        }
        else
        {
            modelBuilder.Entity<Appointment>()
                .HasQueryFilter(p => p.MedicalProfileId == appContextProvider.CurrentMedicalProfileId && !p.IsDeleted);
        }
        modelBuilder.Entity<Patient>()
            .HasQueryFilter(p => p.MedicalProfileId == appContextProvider.CurrentMedicalProfileId && !p.IsDeleted);
        modelBuilder.Entity<PatientPair>()
            .HasQueryFilter(p => p.MedicalProfileId == appContextProvider.CurrentMedicalProfileId && !p.IsDeleted);
    }

    public static void ConfigureBaseEntity(this ModelBuilder modelBuilder, DatabaseProviderInfo dbProviderInfo)
    {
        IEnumerable<IMutableEntityType> entityTypes = modelBuilder.Model.GetEntityTypes();

        foreach (IMutableEntityType entityType in entityTypes)
        {
            if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
            {
                EntityTypeBuilder entityBuilder = modelBuilder.Entity(entityType.ClrType);

                var hasMedicalProfileId = entityType.FindProperty("MedicalProfileId") != null;

                if (hasMedicalProfileId)
                {
                    entityBuilder.HasIndex("MedicalProfileId", "DateModified");
                    entityBuilder.HasIndex("MedicalProfileId", "IsDeleted");
                }

                entityBuilder.Property("IsDeleted")
                    .HasDefaultValue(false);

                var parameter = Expression.Parameter(entityType.ClrType, "e");
                var property = Expression.Property(parameter, "IsDeleted");

                var notDeleted = Expression.Equal(property, Expression.Constant(false));

                var filter = Expression.Lambda(notDeleted, parameter);
                entityBuilder.HasQueryFilter(filter);

                if (dbProviderInfo.IsSqlite())
                {
                    entityBuilder.Property("DateModified")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");
                    entityBuilder.Property("DateCreated")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");
                }
                if (dbProviderInfo.IsMySql())
                {
                    entityBuilder.Property("DateCreated")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    entityBuilder.Property("DateModified")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasComputedColumnSql("CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP");

                    entityBuilder.Property("DateModified")
                        .HasColumnType("DATETIME(3)");

                    entityBuilder.Property("DateCreated")
                        .HasColumnType("DATETIME(3)");
                }
                if (dbProviderInfo.IsPostgreSql())
                {
                    entityBuilder.Property("DateCreated")
                        .HasDefaultValueSql("now()");

                    entityBuilder.Property("DateModified")
                        .HasDefaultValueSql("now()");

                    entityBuilder.Property("DateModified")
                        .HasColumnType("TIMESTAMP(3) WITHOUT TIME ZONE");

                    entityBuilder.Property("DateCreated")
                        .HasColumnType("TIMESTAMP(3) WITHOUT TIME ZONE");
                }


            }
        }
    }
}


