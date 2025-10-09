using MedSolutions.App.Common.Models;

namespace MedSolutions.App.Common.Extensions;
public static class QueryFilterExtension
{
    public static IQueryable<T> ApplyFilter<T>(
        this IQueryable<T> query,
        QueryFilter<T>? filterProps) => filterProps?.ApplyTo(query) ?? query;

}
