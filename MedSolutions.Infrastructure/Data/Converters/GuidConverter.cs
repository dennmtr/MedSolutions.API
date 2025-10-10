using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MedSolutions.Infrastructure.Data.Converters;
public class GuidConverter : ValueConverter<Guid, byte[]>
{
    public static readonly GuidConverter Instance = new();

    public GuidConverter()
        : base(x => x.ToByteArray(), x => new Guid(x))
    {
    }
}
