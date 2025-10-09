
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc;

namespace MedSolutions.App.Common.Models;

public abstract class QuerySorting<T>()
{
    [FromQuery(Name = "sortBy")]
    public string? SortBy { get; set; }

    [FromQuery(Name = "sort")]
    public ListSortDirection? SortDirection { get; init; } = ListSortDirection.Ascending;
    protected IQueryable<T> Query { get; private set; } = default!;

    public IOrderedQueryable<T> ApplyTo(IQueryable<T> data)
    {
        Query = data;
        return GetOrderedQuery(SortBy?.Trim().ToLowerInvariant() ?? string.Empty);
    }

    protected abstract IOrderedQueryable<T> GetOrderedQuery(string key);
}
