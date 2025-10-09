using Microsoft.AspNetCore.Mvc;

namespace MedSolutions.App.Common.Models;

public abstract class QueryFilter<T>
{
    [FromQuery(Name = "filter")]
    public IDictionary<string, IDictionary<string, string>>? Filters { get; init; } = new Dictionary<string, IDictionary<string, string>>();

    public abstract IQueryable<T> ApplyTo(IQueryable<T> query);
}
