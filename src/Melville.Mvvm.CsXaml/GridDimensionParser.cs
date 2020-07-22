using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;

namespace Melville.Mvvm.CsXaml
{
    public static class GridDimensionParser
    {
        private static readonly Regex GridLengthFinder = new Regex(
            @"auto|\*|((?:\d+(?:\.\d+)?))\*?", RegexOptions.Compiled|RegexOptions.IgnoreCase);
        public static IEnumerable<GridLength> ParseGridDimensions(string declarations) => 
            GridLengthFinder.Matches(declarations).Select(ParseSingleDimension);

        private static GridLength ParseSingleDimension(Match lengthDecl) => 
            IsAuto(lengthDecl.Value[0]) ? new GridLength() :  ParseNumericalGridLength(lengthDecl);

        private static GridLength ParseNumericalGridLength(Match lengthDecl)
        {
            return new GridLength(ParseNumberValue(lengthDecl.Groups[1].Value),
                IsProportional(lengthDecl)?GridUnitType.Star:GridUnitType.Pixel);
        }

        private static double ParseNumberValue(string num) => 
            string.IsNullOrWhiteSpace(num)?1.0:double.Parse(num);

        private static bool IsProportional(Match lengthDecl)
        {
            return lengthDecl.Value.EndsWith('*');
        }

        private static bool IsAuto(char character) => character =='a' || character == 'A';
        
    }
}