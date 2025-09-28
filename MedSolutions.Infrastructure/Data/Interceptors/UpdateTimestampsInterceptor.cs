using MedSolutions.Domain.Models;
using MedSolutions.Shared.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace MedSolutions.Infrastructure.Data.Interceptors;

public class UpdateTimestampsInterceptor : ISaveChangesInterceptor
{
    public InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        OnSavingChanges(eventData.Context!);
        return result;
    }

    public ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        OnSavingChanges(eventData.Context!);
        return new ValueTask<InterceptionResult<int>>(result);
    }

    public static void OnSavingChanges(DbContext context)
    {
        DbProviderInfo dbProviderInfo = new DbProviderInfo(context.Database.ProviderName);

        IEnumerable<Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<BaseEntity>> entries = context.ChangeTracker.Entries<BaseEntity>()
            .Where(e => e.State is EntityState.Deleted or EntityState.Modified or EntityState.Added);
        foreach (Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<BaseEntity>? entry in entries)
        {
            DateTime dateTime = DateTime.UtcNow;

            if (dbProviderInfo.IsSqlite())
            {
                dateTime = dateTime.TrimToSeconds();
            }

            entry.Entity.DateModified = dateTime;

            if (entry.State == EntityState.Added)
            {
                entry.Entity.DateCreated = dateTime;

            }
        }
    }
}
