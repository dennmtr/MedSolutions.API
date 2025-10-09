using MedSolutions.App.Common.Models;

namespace MedSolutions.App.Common.Extensions;
public static class QuerySortExtension
{
    public static IQueryable<T> ApplySorting<T>(
        this IQueryable<T> query,
        QuerySorting<T>? sortingProps) => sortingProps?.ApplyTo(query) ?? query;

}
