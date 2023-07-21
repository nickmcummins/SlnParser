using System;
using System.IO;
using System.Text;
using Enum = SlnParser.Common.Utilities.Enum;

namespace SlnParser.Common.Utilities
{
    public static class StringExtensions
    {
        public static readonly string NewLine = "\r\n";

        public static string Indent(this string s, int level = 1)
        {
            var indentation = new StringBuilder();
            for (var i = 0; i < level; i++) indentation.Append("    ");
            return $"{indentation}{s}";
        }

        public static string WithBraces(this string s) => string.Concat("{", s, "}");

        public static string RelativePathTo(this string filepath, string otherFilepath) => new Uri(filepath).MakeRelativeUri(new Uri(otherFilepath)).ToString().Replace('/', Path.DirectorySeparatorChar);

        public static string GetDescription<TEnumType>(this TEnumType enumValue) where TEnumType : System.Enum => Enum.OfType<TEnumType>().DescriptionsByEnumValue[enumValue];

        public static string ToUpper(this Guid e) => e.ToString().ToUpper();
    }
}
