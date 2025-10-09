
using System.ComponentModel;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;

namespace MedSolutions.App.Common.Models;

public abstract class QuerySorting<T>()
{
    [FromQuery(Name = "sortBy")]
    public string? SortBy { get; set; }

    [FromQuery(Name = "sort")]
    public ListSortDirection? SortDirection { get; set; } = ListSortDirection.Ascending;
    private IQueryable<T> _query = default!;

    public QuerySorting<T> OrderBy(Expression<Func<T, object>> expression, ListSortDirection? directionOverride = null)
    {
        var direction = directionOverride ?? SortDirection;

        _query = direction == ListSortDirection.Descending
            ? _query.OrderByDescending(expression)
            : _query.OrderBy(expression);

        return this;
    }

    public QuerySorting<T> ThenBy(Expression<Func<T, object>> expression, ListSortDirection? directionOverride = null)
    {

        if (_query is not IOrderedQueryable<T> ordered)
        {
            throw new InvalidOperationException("ThenBy requires an ordered query.");
        }

        var direction = directionOverride ?? SortDirection;

        _query = direction == ListSortDirection.Descending
            ? ordered.ThenByDescending(expression)
            : ordered.ThenBy(expression);

        return this;
    }

    public IOrderedQueryable<T> ApplyTo(IQueryable<T> data)
    {
        _query = data;
        Sort(SortBy?.Trim() ?? string.Empty);
        return (IOrderedQueryable<T>)_query;
    }

    protected abstract void Sort(string key);
}
