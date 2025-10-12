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
    public static DateTime TrimToMinutes(this DateTime value)
    {
        return new DateTime(
            value.Year,
            value.Month,
            value.Day,
            value.Hour,
            value.Minute,
            0,
            value.Kind
        );
    }
    public static DateTime TrimToMilliseconds(this DateTime value)
    {
        long ticksPerMillisecond = TimeSpan.TicksPerMillisecond;
        long trimmedTicks = value.Ticks - (value.Ticks % ticksPerMillisecond);
        return new DateTime(trimmedTicks, value.Kind);
    }

}
