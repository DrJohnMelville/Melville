using System;
using System.Text.RegularExpressions;

namespace Melville.ExcelDataContext.DataContextBases
{
    public abstract class DataTableBase
    {
        protected static bool IsNullOrWhiteSpace(string s) => String.IsNullOrWhiteSpace(s);
        protected static int ParseInt(string s) => int.Parse(s);
        protected static double  ParseDouble(string s) => Double.Parse(s);
        protected static DateTime  ParseDateTime(string s) => DateTime.Parse(s);
        protected static bool ParseBoolean(string s) => TrueDetector.Match(s).Success;
        private static readonly Regex TrueDetector = new Regex(@"^\s*(Y|Yes|T|True)\s*$", RegexOptions.IgnoreCase);
    }
}