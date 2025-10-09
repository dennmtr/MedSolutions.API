using Microsoft.AspNetCore.Mvc;

namespace MedSolutions.App.Common.Models;

public class QueryPagination
{
    [FromQuery(Name = "page")]
    public int? PageNumber { get; set; } = 1;
    [FromQuery(Name = "pageSize")]
    public int? PageSize { get; set; } = 10;
    public QueryPagination() { }
    public QueryPagination(int pageNumber, int pageSize)
    {
        PageNumber = pageNumber < 1 ? 1 : pageNumber;
        PageSize = pageSize > 500 ? 500 : pageSize;
    }
    public IQueryable<T> ApplyTo<T>(IQueryable<T> query)
    {
        var skip = (PageNumber!.Value - 1) * PageSize!.Value;
        return query.Skip(skip).Take(PageSize!.Value);
    }

    public PaginationViewModel<T> ToViewModel<T>(List<T> data, long totalRecords)
    {
        return new PaginationViewModel<T>() {
            PageNumber = PageNumber!.Value,
            PageSize = PageSize!.Value,
            TotalRecords = totalRecords,
            Data = data
        };
    }

}
