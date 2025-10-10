using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MedSolutions.Infrastructure.Data.Converters;
public class NullableGuidConverter : ValueConverter<Guid?, byte[]?>
{
    public static readonly NullableGuidConverter Instance = new();

    public NullableGuidConverter()
        : base(
            x => x != null ? x.Value.ToByteArray() : null,
            x => x != null ? new Guid(x) : null)
    {
    }
}
