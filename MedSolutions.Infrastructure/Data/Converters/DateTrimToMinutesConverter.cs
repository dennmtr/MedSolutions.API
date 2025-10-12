using MedSolutions.Shared.Extensions;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MedSolutions.Infrastructure.Data.Converters;
public class DateTrimToMinutesConverter : ValueConverter<DateTime, DateTime>
{
    public static readonly DateTrimToMinutesConverter Instance = new();

    public DateTrimToMinutesConverter()
        : base(x => x.TrimToMinutes(), x => x) { }
}
