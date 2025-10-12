using MedSolutions.Domain.Common.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace MedSolutions.Infrastructure.Data.Interceptors;

public class SoftDeleteInterceptor : ISaveChangesInterceptor
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
        IEnumerable<EntityEntry<BaseEntity>> entries = context.ChangeTracker.Entries<BaseEntity>()
            .Where(e => e.State == EntityState.Deleted);

        foreach (EntityEntry<BaseEntity>? entry in entries)
        {
            entry.State = EntityState.Modified;
            entry.Entity.IsDeleted = true;
        }
    }
}
