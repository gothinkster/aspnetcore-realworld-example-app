using System.Text.RegularExpressions;

namespace Conduit.Infrastructure
{
    // https://stackoverflow.com/questions/2920744/url-slugify-algorithm-in-c
    public static class Slug
    {
        public static string? GenerateSlug(this string? phrase)
        {
            if (phrase is null)
            {
                return null;
            }
            string str = phrase.ToLowerInvariant();
            // invalid chars
            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
            // convert multiple spaces into one space
            str = Regex.Replace(str, @"\s+", " ").Trim();
            // cut and trim
            str = str.Substring(0, str.Length <= 45 ? str.Length : 45).Trim();
            str = Regex.Replace(str, @"\s", "-"); // hyphens
            return str;
        }
    }
}
