using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MedSolutions.Infrastructure.Data.Converters;
public class NullableLowercaseGuidConverter : ValueConverter<Guid?, string?>
{
    public NullableLowercaseGuidConverter()
        : base(
            guid => guid.HasValue ? guid.Value.ToString("D").ToLower() : null,
            str => str != null ? Guid.Parse(str) : null
        )
    { }
}
