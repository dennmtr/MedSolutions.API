using MedSolutions.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace MedSolutions.Infrastructure.Data.Interceptors;

public class PatientPairNormalizeInterceptor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData,
                                                          InterceptionResult<int> result)
    {
        NormalizePatientPairs(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        NormalizePatientPairs(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private static void NormalizePatientPairs(DbContext? context)
    {
        if (context is null)
        {
            return;
        }

        foreach (Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<PatientPair>? entry in context.ChangeTracker.Entries<PatientPair>()
            .Where(e => e.State is EntityState.Added or EntityState.Modified))
        {
            entry.Entity.Normalize();
        }
    }
}
