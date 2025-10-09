using MedSolutions.App.Common.Models;

namespace MedSolutions.App.Common.Extensions;
public static class QueryPaginationExtension
{
    public static IQueryable<T> Paginate<T>(
        this IQueryable<T> query,
        QueryPagination? paginationProps) => paginationProps?.ApplyTo(query) ?? query;

}
