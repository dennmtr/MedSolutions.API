using MedSolutions.Domain.Models;

namespace MedSolutions.App.Extensions;
public static class PatientOrderedQueryExtensions
{
    public static IOrderedQueryable<Patient> OrderByDateModified(
        this IQueryable<Patient> query,
        bool descending = false)
    {
        return descending
            ? query
                .OrderByDescending(p => p.DateModified)
            : query
                .OrderBy(p => p.DateModified)
                ;
    }
    public static IOrderedQueryable<Patient> OrderByDateOfBirth(
        this IQueryable<Patient> query,
        bool descending = false)
    {
        return descending
            ? query
                .OrderBy(p => p.BirthDate == null)
                .ThenByDescending(p => p.BirthDate)
                .ThenBy(p => p.LastName)
                .ThenBy(p => p.FirstName)
            : query
                .OrderBy(p => p.BirthDate == null)
                .ThenBy(p => p.BirthDate)
                .ThenBy(p => p.LastName)
                .ThenBy(p => p.FirstName)
                ;
    }
    public static IOrderedQueryable<Patient> OrderByLastName(
        this IQueryable<Patient> query,
        bool descending = false)
    {
        return descending
            ? query
                .OrderByDescending(p => p.LastName)
                .ThenBy(p => p.FirstName)
            : query
                .OrderBy(p => p.LastName)
                .ThenBy(p => p.FirstName)
                ;
    }
    public static IOrderedQueryable<Patient> OrderByCity(
        this IQueryable<Patient> query,
        bool descending = false)
    {
        return descending
            ? query
                .OrderByDescending(p => p.City)
                .ThenBy(p => p.LastName)
                .ThenBy(p => p.FirstName)
            : query
                .OrderBy(p => p.City)
                .ThenBy(p => p.LastName)
                .ThenBy(p => p.FirstName)
                ;
    }
    public static IOrderedQueryable<Patient> OrderByAppointments(
        this IQueryable<Patient> query,
        bool descending = false)
    {
        var now = DateTime.UtcNow;

        return descending
            ? query.OrderByDescending(p => p.Appointments
                    .Max(a => (DateTime?)a.Date) ?? DateTime.MinValue)
                .ThenByDescending(p => p.Appointments
                    .Min(a => (DateTime?)a.Date) ?? DateTime.MinValue)
            : query
            .OrderBy(p => p.Appointments
                .Max(a => (DateTime?)a.Date) ?? DateTime.MaxValue)
            .ThenBy(p => p.Appointments
                .Min(a => (DateTime?)a.Date) ?? DateTime.MaxValue);
    }

}
