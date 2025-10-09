namespace MedSolutions.App.Common.Extensions;
public static class QueryableOrderExtensions
{
    public static IOrderedQueryable<T> ThenApply<T>(
      this IOrderedQueryable<T> source,
      Func<IOrderedQueryable<T>, IOrderedQueryable<T>> next) => next(source);
}
