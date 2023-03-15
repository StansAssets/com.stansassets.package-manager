using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace StansAssets.PackageManager
{
    static class NameConventionBuilder
    {
        internal static string BuildAssemblyName(string name, NamingConvention convention)
        {
            var conventionCopy = convention.Copy();
            conventionCopy.ConventionType = NameConventionType.PascalCase;

            return BuildName(name, conventionCopy);
        }

        internal static string BuildName(string name, NamingConvention convention)
        {
            var result = FormatTextByConvention(name, convention.ConventionType);
            return $"{result}";
        }

        internal static string FormatTextByConvention(string text, NameConventionType convention)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            text = text.Replace('-', ' ');
            text = text.Replace('_', ' ');

            RemoveChar(ref text, '\'');
            RemoveChar(ref text, '\"');

            RemoveDoubleSpaces(ref text);

            text = text.Trim();
            text = text.Trim('.');

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

        static void RemoveChar(ref string text, char c)
        {
            for (var i = text.Count(e => e.Equals(c)); i > 0; i--)
            {
                text = text.Remove(text.IndexOf(c), 1);
            }
        }

        static void RemoveDoubleSpaces(ref string text)
        {
            text = text.Trim();

            const string doubleSpace = "  ";
            while (text.Contains(doubleSpace))
            {
                var index = text.IndexOf(doubleSpace, StringComparison.Ordinal);
                text = text.Remove(index, 1);
            }
        }

        internal static string RemovePrefix(string name, string prefix, bool removeAbstractPrefix = false)
        {
            if (string.IsNullOrEmpty(name))
            {
                return string.Empty;
            }

            const char space = ' ';

            var originalLenght = prefix.Length;
            var changedLenght = originalLenght;

            if (name.Contains(space))
            {
                if (name.Length - 1 < originalLenght)
                {
                    changedLenght = name.IndexOf(space);

                    var severalSpaces = name.Count(s => s.Equals(' ')) > 1;
                    if (severalSpaces)
                    {
                        changedLenght = name.IndexOf(space, changedLenght + 1);
                    }
                }
                else
                {
                    changedLenght = name.IndexOf(space, originalLenght - 2);

                    if (changedLenght == -1)
                    {
                        changedLenght = name.IndexOf(space);
                    }
                }
            }
            else
            {
                if (name.Length < originalLenght)
                {
                    changedLenght = name.Length;
                }
            }

            var formattedPrefix = FormatTextByConvention(prefix, NameConventionType.PascalCase).ToLower();
            var formattedName = FormatTextByConvention(
                    name.Substring(0, changedLenght),
                    NameConventionType.PascalCase)
                .ToLower();

            if (removeAbstractPrefix || formattedName.Contains(formattedPrefix))
            {
                name = name.Remove(0, changedLenght);
            }

            name = name.Trim();
            name = name.Trim('.');

            return name;
        }
    }
}