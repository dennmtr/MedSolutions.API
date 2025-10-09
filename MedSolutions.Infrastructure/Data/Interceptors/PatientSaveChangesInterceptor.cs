using System.Linq.Expressions;
using MedSolutions.Domain.Models;
using MedSolutions.Shared.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace MedSolutions.Infrastructure.Data.Interceptors;

public class PatientSaveChangesInterceptor : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        ProcessPatients(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        ProcessPatients(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private static void ProcessPatients(DbContext context)
    {
        foreach (EntityEntry<Patient> entry in context.ChangeTracker.Entries<Patient>())
        {
            if (entry.State is EntityState.Added or EntityState.Modified)
            {
                Patient patient = entry.Entity;

                UpdateLatinIfModified(entry, p => p.FirstName, p => p.FirstNameLatin);
                UpdateLatinIfModified(entry, p => p.LastName, p => p.LastNameLatin);
                UpdateLatinIfModified(entry, p => p.City, p => p.CityLatin);
            }
        }
    }

    private static void UpdateLatinIfModified(
        EntityEntry<Patient> entry,
        Expression<Func<Patient, string?>> sourceProp,
        Expression<Func<Patient, string?>> targetProp)
    {
        PropertyEntry<Patient, string?> source = entry.Property(sourceProp);
        if (entry.State != EntityState.Added && !source.IsModified)
        {
            return;
        }

        string? sourceValue = source.CurrentValue;

        PropertyEntry<Patient, string?> target = entry.Property(targetProp);

        target.CurrentValue = sourceValue?.ToLatin().ToLowerInvariant();
    }
}
