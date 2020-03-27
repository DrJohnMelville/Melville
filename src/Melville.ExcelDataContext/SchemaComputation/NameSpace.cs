using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Melville.ExcelDataContext.SchemaComputation
{
    /// <summary>
    /// Enforces the following rules
    /// - PascalCasing
    /// - May not contain nonword characters
    /// - May not begin with a number
    /// -- May note be whitespace or empty
    /// -- May not be a C# keyword
    /// -- must be unique.
    /// 
    /// This means column names can be c# variable names.
    /// </summary>
    public class NameSpace
    {
        private readonly List<string> oldNames = new List<string>();
        public string Rename(string source)
        {
            var ret = Unique(
                AvoidKeyWord(
                    EscapeInitialNumber(
                        PascalCase(
                            FillEmptyName(
                                WordCharactersOnly(
                                    source))))));
            oldNames.Add(ret);
            return ret;
        }

        private string WordCharactersOnly(string s)
        {
            return Regex.Replace(s ?? string.Empty, @"[^\w ]", "");
        }
        private string GetExcelColumnName(int columnNumber)
        {
            int dividend = columnNumber + 1;
            string columnName = String.Empty;
            int modulo;

            while (dividend > 0)
            {
                modulo = (dividend - 1) % 26;
                columnName = Convert.ToChar(65 + modulo).ToString() + columnName;
                dividend = (int)((dividend - modulo) / 26);
            }

            return columnName;
        }

        private string FillEmptyName(string source) =>
            String.IsNullOrWhiteSpace(source) ? CurrentColumnName() : source;

        private string CurrentColumnName() => GetExcelColumnName(oldNames.Count);

        private string EscapeInitialNumber(string source) => (Char.IsDigit(source[0])) ? "_" + source : source;

        private string PascalCase(string source)
        {
            var segments = Regex.Matches(source, @"\w+");
            if (segments.Count < 2)
            {
                return source;
            }
            var textInfo = CultureInfo.CurrentUICulture.TextInfo;
            return string.Join("", segments.OfType<Match>().Select(i => textInfo.ToTitleCase(i.Value)));
        }

        private static readonly string[] Keywords = {
            "abstract",
            "as",
            "base",
            "bool",
            "break",
            "byte",
            "case",
            "catch",
            "char",
            "checked",
            "class",
            "const",
            "continue",
            "decimal",
            "default",
            "delegate",
            "do",
            "double",
            "else",
            "enum",
            "event",
            "explicit",
            "extern",
            "false",
            "finally",
            "fixed",
            "float",
            "for",
            "foreach",
            "goto",
            "if",
            "implicit",
            "in",
            "int",
            "interface",
            "internal",
            "is",
            "lock",
            "long",
            "namespace",
            "new",
            "null",
            "object",
            "operator",
            "out",
            "override",
            "params",
            "private",
            "protected",
            "public",
            "readonly",
            "ref",
            "return",
            "sbyte",
            "sealed",
            "short",
            "sizeof",
            "stackalloc",
            "static",
            "string",
            "struct",
            "switch",
            "this",
            "throw",
            "true",
            "try",
            "typeof",
            "uint",
            "ulong",
            "unchecked",
            "unsafe",
            "ushort",
            "using",
            "var",
            "virtual",
            "void",
            "volatile",
            "while",
            "add",
            "alias",
            "get",
            "global",
            "partial",
            "remove",
            "set",
            "value",
            "where",
            "yield"
        };
        private string AvoidKeyWord(string s)
        {
            return Keywords.Contains(s, StringComparer.Ordinal) ? CultureInfo.CurrentUICulture.TextInfo.ToTitleCase(s) : s;
        }

        private string Unique(string source)
        {
            return oldNames.Contains(source, StringComparer.Ordinal)
                ? Unique(source + "_" + CurrentColumnName())
                : source;
        }
    }
}