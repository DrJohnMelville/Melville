using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using Melville.Hacks;
using Melville.Linq;

namespace Melville.WpfControls.EasyGrids
{
    public static class GridOperations
    {
        public static readonly DependencyProperty ColsAndRowsProperty = DependencyProperty.RegisterAttached(
            "ColsAndRows", typeof(string), typeof(GridOperations),
            new FrameworkPropertyMetadata(null, SetColsAndRows));

        public static string GetColsAndRows(DependencyObject obj) => (string) obj.GetValue(ColsAndRowsProperty);

        public static void SetColsAndRows(DependencyObject obj, string value) =>
            obj.SetValue(ColsAndRowsProperty, value);

        private static void SetColsAndRows(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Grid g && e.NewValue is string s)
                AssignColsAndRows(g, s);
        }

        private static void AssignColsAndRows(Grid grid, string data)
        {
            var split = data.Split('/');
            if (split.Length > 0)
                grid.ColumnDefinitions.AddRange(GridDimensionParser.ParseGridDimensions(split[0])
                    .Select(i=>new ColumnDefinition{Width = i}));
            if (split.Length > 1)
                grid.RowDefinitions.AddRange(GridDimensionParser.ParseGridDimensions(split[1])
                    .Select(i=>new RowDefinition{Height = i}));
        }
    }
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