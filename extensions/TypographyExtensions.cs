using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace CzomPack.Extensions
{
    public static class TypographyExtensions
    {
        public static string ToTitleCase(this string str) => str == null ? null : new CultureInfo("en-US", false).TextInfo.ToTitleCase(string.Join(" ", new Regex(@"[A-Z]{2,}(?=[A-Z][a-z]+[0-9]*|\b)|[A-Z]?[a-z]+[0-9]*|[A-Z]|[0-9]+").Matches(str)).ToLower());
        public static string ToCamelCase(this string str) => str == null ? null : $"{str[0].ToString().ToLower()}{str.ToPascalCase().Substring(1)}";
        public static string ToPascalCase(this string str) => str?.ToTitleCase().Replace(" ", "");
        public static string ToKebabCase(this string str) => str == null ? null : string.Join("-", new Regex(@"[A-Z]{2,}(?=[A-Z][a-z]+[0-9]*|\b)|[A-Z]?[a-z]+[0-9]*|[A-Z]|[0-9]+").Matches(str.ToCamelCase()).Cast<Match>().Select(m => m.Value)).ToLower();
        public static string ToSnakeCase(this string str) => str?.ToKebabCase().Replace("-", "_");
    }
}
