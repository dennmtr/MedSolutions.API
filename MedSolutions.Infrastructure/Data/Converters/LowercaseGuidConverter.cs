using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MedSolutions.Infrastructure.Data.Converters;
public class LowercaseGuidConverter : ValueConverter<Guid, string>
{
    public LowercaseGuidConverter()
        : base(
            guid => guid.ToString("D").ToLower(),
            str => Guid.Parse(str)
        )
    { }
}
