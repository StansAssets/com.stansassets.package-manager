using System.Text.RegularExpressions;

namespace StansAssets.PackageManager
{
    static class NameConventionBuilder
    {
        internal static string BuildName(string name, NamingConvention convention)
        {
            var result = FormatTextByConvention(name, convention.ConventionType);
            return $"{convention.Prefix}.{result}.{convention.Postfix}";
        }

        static string FormatTextByConvention(string text, NameConventionType convention)
        {
            switch (convention)
            {
                case NameConventionType.CamelCase:
                {
                    var output = Regex
                        .Replace(text, @"\b(\w)", match => match.Value.ToUpper())
                        .Replace(" ", "");
                    
                    return char.ToLower(output[0]) + output.Substring(1);
                }
                case NameConventionType.SnakeCase:
                {
                    var output = Regex.Replace(text, @"(\p{Ll})(\p{Lu})|(\W)+", "$1_$2");
                    return output.ToLower();
                }
                case NameConventionType.KebabkCase:
                {
                    return Regex.Replace(text, @"(\p{Ll})(\p{Lu})|(\W)+", "$1-$2").ToLower();
                }
                case NameConventionType.PascalCase:
                {
                    var output = Regex.Replace(text, @"(?:^|\s)(\w)", match => match.Value.ToUpper());
                    return output.Replace(" ", "");
                }
                case NameConventionType.None:
                default:
                {
                    return text;
                }
            }
        }
    }
}