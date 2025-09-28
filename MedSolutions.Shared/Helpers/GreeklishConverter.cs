using System.Text.RegularExpressions;

namespace MedSolutions.Shared.Helpers;

public class GreeklishConverter
{
    private static readonly Dictionary<string, string> _chars = new()
    {
        {"α", "a"}, {"ά", "a"}, {"Α", "a"}, {"Ά", "a"},
        {"β", "v"}, {"Β", "v"},
        {"γ", "g"}, {"Γ", "g"},
        {"δ", "d"}, {"Δ", "d"},
        {"ε", "e"}, {"έ", "e"}, {"Ε", "e"}, {"Έ", "e"},
        {"ζ", "z"}, {"Ζ", "z"},
        {"η", "i"}, {"ή", "i"}, {"Η", "i"}, {"Ή", "i"},
        {"θ", "th"}, {"Θ", "th"},
        {"ι", "i"}, {"ί", "i"}, {"ϊ", "i"}, {"ΐ", "i"}, {"Ι", "i"}, {"Ί", "i"}, {"Ϊ", "i"},
        {"κ", "k"}, {"Κ", "k"},
        {"λ", "l"}, {"Λ", "l"},
        {"μ", "m"}, {"Μ", "m"},
        {"ν", "n"}, {"Ν", "n"},
        {"ξ", "ks"}, {"Ξ", "ks"},
        {"ο", "o"}, {"ό", "o"}, {"Ο", "o"}, {"Ό", "o"},
        {"π", "p"}, {"Π", "p"},
        {"ρ", "r"}, {"Ρ", "r"},
        {"σ", "s"}, {"ς", "s"}, {"Σ", "s"},
        {"τ", "t"}, {"Τ", "t"},
        {"υ", "y"}, {"ύ", "y"}, {"ϋ", "y"}, {"ΰ", "y"}, {"Υ", "y"}, {"Ύ", "y"}, {"Ϋ", "y"},
        {"φ", "f"}, {"Φ", "f"},
        {"χ", "ch"}, {"Χ", "ch"},
        {"ψ", "ps"}, {"Ψ", "ps"},
        {"ω", "o"}, {"ώ", "o"},
        {"΅", ""}
    };

    private static readonly Regex _regex = new(
        string.Join("|", _chars.Keys),
        RegexOptions.Compiled
    );

    public static string? ToGreeklish(string? source)
    {
        if (string.IsNullOrEmpty(source))
        {
            return source;
        }

        string result = _regex.Replace(source, match => _chars[match.Value]);

        result = result.Replace("yi", "vi")
                       .Replace("ye", "ve")
                       .Replace("yo", "vo")
                       .Replace("ya", "va")
                       .Replace("ey", "ef")
                       .Replace("ay", "af")
                       .Replace("ey", "v")
                       .Replace("ay", "v")
                       .Replace("oy", "ou")
                       .Replace("iy", "v")
                       .Replace("ei", "i")
                       .Replace("y", "i");

        return result;
    }
}
