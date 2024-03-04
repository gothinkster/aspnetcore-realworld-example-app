using System.Text.RegularExpressions;

namespace Conduit.Infrastructure;

// https://stackoverflow.com/questions/2920744/url-slugify-algorithm-in-c
public static partial class Slug
{
    public static string? GenerateSlug(this string? phrase)
    {
        if (phrase is null)
        {
            return null;
        }
        var str = phrase.ToLowerInvariant();
        // invalid chars
        str = InvalidCharsRegex().Replace(str, "");
        // convert multiple spaces into one space
        str = MultipleSpacesRegex().Replace(str, " ").Trim();
        // cut and trim
        str = str[..(str.Length <= 45 ? str.Length : 45)].Trim();
        str = TrimRegex().Replace(str, "-"); // hyphens
        return str;
    }

    [GeneratedRegex("[^a-z0-9\\s-]")]
    private static partial Regex InvalidCharsRegex();

    [GeneratedRegex("\\s+")]
    private static partial Regex MultipleSpacesRegex();

    [GeneratedRegex("\\s")]
    private static partial Regex TrimRegex();
}
