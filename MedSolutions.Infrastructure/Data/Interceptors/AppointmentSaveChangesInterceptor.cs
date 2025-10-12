using MedSolutions.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace MedSolutions.Infrastructure.Data.Interceptors;

public class AppointmentSaveChangesInterceptor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        OnSavingChanges(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        OnSavingChanges(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private static void OnSavingChanges(DbContext context)
    {
        foreach (EntityEntry<Appointment> entry in context.ChangeTracker.Entries<Appointment>())
        {
            if (entry.State is EntityState.Added or EntityState.Modified)
            {
                _ = entry.Entity;
            }
        }
    }
}
