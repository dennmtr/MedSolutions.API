namespace MedSolutions.Shared.Extensions;

public static class DateTimeExtensions
{
    public static DateTime TrimToSeconds(this DateTime value)
    {
        return new DateTime(
            value.Year, value.Month, value.Day,
            value.Hour, value.Minute, value.Second,
            value.Kind
        );
    }
}
