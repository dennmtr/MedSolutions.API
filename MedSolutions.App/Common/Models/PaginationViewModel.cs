namespace MedSolutions.App.Common.Models;

public class PaginationViewModel<T>()
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public long TotalPages => Convert.ToInt64(Math.Ceiling((double)TotalRecords / PageSize));
    public long TotalRecords { get; set; }
    public List<T> Data { get; set; } = [];
}
