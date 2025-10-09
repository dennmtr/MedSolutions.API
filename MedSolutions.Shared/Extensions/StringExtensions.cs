using MedSolutions.Shared.Helpers;

namespace MedSolutions.Shared.Extensions;

public static class StringExtensions
{
    public static string ToLatin(this string value) => GreeklishConverter.ContainsGreek(value) ? GreeklishConverter.ToGreeklish(value) : value;


}
