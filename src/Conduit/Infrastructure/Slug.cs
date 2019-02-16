using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Conduit.Infrastructure
{
    //https://stackoverflow.com/questions/2920744/url-slugify-algorithm-in-c
    //https://stackoverflow.com/questions/249087/how-do-i-remove-diacritics-accents-from-a-string-in-net
    public static class Slug
    {
        public class Punycode
        {
            /* Punycode parameters */
            static int TMIN = 1;
            static int TMAX = 26;
            static int BASE = 36;
            static int INITIAL_N = 128;
            static int INITIAL_BIAS = 72;
            static int DAMP = 700;
            static int SKEW = 38;
            static char DELIMITER = '-';
            public static string EncodingDomain(string domain)
            {
                domain = domain.Replace("。", ".");
                string[] domainArray = domain.Split(new string[] { "." }, StringSplitOptions.None);
                string result = "";
                foreach (string item in domainArray)
                {
                    if (item == "")
                    {
                        result += ".";
                        continue;
                    }
                    result += "xn--" + Encode(item) + ".";
                }
                if (result[result.Length - 1] == '.') result = result.Substring(0, result.Length - 1);
                return result;
            }
            public static string DecodingDomain(string domain)
            {
                domain = domain.Replace("。", ".");
                string[] domainArray = domain.Split(new string[] { "." }, StringSplitOptions.None);
                string result = "";
                foreach (string item in domainArray)
                {
                    if (item == "")
                    {
                        result += ".";
                        continue;
                    }
                    string item2 = item;
                    if (item2.Length > 4 && item2.Substring(0, 4) == "xn--")
                    {
                        result += Decode(item2.Substring(4, item2.Length - 4)) + ".";
                    }
                }
                if (result[result.Length - 1] == '.') result = result.Substring(0, result.Length - 1);
                return result;
            }
            public static string Encode(string input)
            {
                int n = INITIAL_N;
                int delta = 0;
                int bias = INITIAL_BIAS;
                StringBuilder output = new StringBuilder();
                // Copy all basic code points to the output
                int b = 0;
                for (int i = 0; i < input.Length; i++)
                {
                    char c = input[i];
                    if (isBasic(c))
                    {
                        output.Append(c);
                        b++;
                    }
                }
                // Append delimiter
                if (b > 0)
                {
                    output.Append(DELIMITER);
                }
                int h = b;
                while (h < input.Length)
                {
                    int m = int.MaxValue;
                    // Find the minimum code point >= n
                    for (int i = 0; i < input.Length; i++)
                    {
                        int c = input[i];
                        if (c >= n && c < m)
                        {
                            m = c;
                        }
                    }
                    if (m - n > (int.MaxValue - delta) / (h + 1))
                    {
                        throw new Exception("OVERFLOW");
                    }
                    delta = delta + (m - n) * (h + 1);
                    n = m;
                    for (int j = 0; j < input.Length; j++)
                    {
                        int c = input[j];
                        if (c < n)
                        {
                            delta++;
                            if (0 == delta)
                            {
                                throw new Exception("OVERFLOW");
                            }
                        }
                        if (c == n)
                        {
                            int q = delta;
                            for (int k = BASE; ; k += BASE)
                            {
                                int t;
                                if (k <= bias)
                                {
                                    t = TMIN;
                                }
                                else if (k >= bias + TMAX)
                                {
                                    t = TMAX;
                                }
                                else
                                {
                                    t = k - bias;
                                }
                                if (q < t)
                                {
                                    break;
                                }
                                output.Append((char)digit2codepoint(t + (q - t) % (BASE - t)));
                                q = (q - t) / (BASE - t);
                            }
                            output.Append((char)digit2codepoint(q));
                            bias = adapt(delta, h + 1, h == b);
                            delta = 0;
                            h++;
                        }
                    }
                    delta++;
                    n++;
                }
                return output.ToString();
            }
            public static string Decode(string input)
            {
                int n = INITIAL_N;
                int i = 0;
                int bias = INITIAL_BIAS;
                StringBuilder output = new StringBuilder();
                int d = input.LastIndexOf(DELIMITER);
                if (d > 0)
                {
                    for (int j = 0; j < d; j++)
                    {
                        char c = input[j];
                        if (!isBasic(c))
                        {
                            throw new Exception("BAD_INPUT");
                        }
                        output.Append(c);
                    }
                    d++;
                }
                else
                {
                    d = 0;
                }
                while (d < input.Length)
                {
                    int oldi = i;
                    int w = 1;
                    for (int k = BASE; ; k += BASE)
                    {
                        if (d == input.Length)
                        {
                            throw new Exception("BAD_INPUT");
                        }
                        int c = input[d++];
                        int digit = codepoint2digit(c);
                        if (digit > (int.MaxValue - i) / w)
                        {
                            throw new Exception("OVERFLOW");
                        }
                        i = i + digit * w;
                        int t;
                        if (k <= bias)
                        {
                            t = TMIN;
                        }
                        else if (k >= bias + TMAX)
                        {
                            t = TMAX;
                        }
                        else
                        {
                            t = k - bias;
                        }
                        if (digit < t)
                        {
                            break;
                        }
                        w = w * (BASE - t);
                    }
                    bias = adapt(i - oldi, output.Length + 1, oldi == 0);
                    if (i / (output.Length + 1) > int.MaxValue - n)
                    {
                        throw new Exception("OVERFLOW");
                    }
                    n = n + i / (output.Length + 1);
                    i = i % (output.Length + 1);
                    output.Insert(i, (char)n);
                    i++;
                }
                return output.ToString();
            }
            private static int adapt(int delta, int numpoints, bool first)
            {
                if (first)
                {
                    delta = delta / DAMP;
                }
                else
                {
                    delta = delta / 2;
                }
                delta = delta + (delta / numpoints);
                int k = 0;
                while (delta > ((BASE - TMIN) * TMAX) / 2)
                {
                    delta = delta / (BASE - TMIN);
                    k = k + BASE;
                }
                return k + ((BASE - TMIN + 1) * delta) / (delta + SKEW);
            }
            private static bool isBasic(char c)
            {
                return c < 0x80;
            }
            private static int digit2codepoint(int d)
            {
                if (d < 26)
                {
                    // 0..25 : 'a'..'z'
                    return d + 'a';
                }
                else if (d < 36)
                {
                    // 26..35 : '0'..'9';
                    return d - 26 + '0';
                }
                else
                {
                    throw new Exception("BAD_INPUT");
                }
            }
            private static int codepoint2digit(int c)
            {
                if (c - '0' < 10)
                {
                    // '0'..'9' : 26..35
                    return c - '0' + 26;
                }
                else if (c - 'a' < 26)
                {
                    // 'a'..'z' : 0..25
                    return c - 'a';
                }
                else
                {
                    throw new Exception("BAD_INPUT");
                }
            }
        }

        public static string GenerateSlug(this string phrase)
        {
            string str = phrase.RemoveDiacritics().ToLower();
            // invalid chars           
            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
            // convert multiple spaces into one space   
            str = Regex.Replace(str, @"\s+", " ").Trim();
            // cut and trim 
            str = str.Substring(0, str.Length <= 45 ? str.Length : 45).Trim();
            str = Regex.Replace(str, @"\s", "-"); // hyphens   
            return str;
        }

        public static string RemoveDiacritics(this string text)
        {
            var s = new string(text.Normalize(NormalizationForm.FormD)
                .Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                .ToArray());

            return s.Normalize(NormalizationForm.FormC);
        }
    }
}
