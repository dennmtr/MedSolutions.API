using Microsoft.AspNetCore.Mvc;

namespace MedSolutions.App.Common.Models;

public abstract class QueryGlobalSearch<T>
{
    [FromQuery(Name = "query")]
    public string? Query { get; set; }
    public abstract IQueryable<T> ApplyTo(IQueryable<T> query);
}
