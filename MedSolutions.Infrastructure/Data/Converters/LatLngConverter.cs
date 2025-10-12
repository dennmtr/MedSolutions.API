using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MedSolutions.Infrastructure.Data.Converters;
public class LatLngConverter : ValueConverter<decimal?, decimal?>
{
    public static readonly LatLngConverter Instance = new();

    public LatLngConverter()
        : base(x => x == null ? null : Math.Round(x.Value, 6), x => x) { }
}
