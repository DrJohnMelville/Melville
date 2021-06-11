using System.Windows;
using Serilog.Data;

namespace Melville.WpfControls.CalendarControls
{
    public readonly struct ColumnMeasure
    {
        private readonly double columnWidth;
        private readonly double height;

        public ColumnMeasure(Size targetSize)
        {
            columnWidth = SingleColumnWidth(targetSize.Width);
            height = targetSize.Height;
        }
        // 2 fore each of the left and right border and 3 for each of 6 internal lines
        private const double ConstantHorizontalSpace = 2 + 2 + (6 * 3);
        private static double SingleColumnWidth(double totalWidth) => 
            (totalWidth - ConstantHorizontalSpace) / 7.0;

        public double ColumnOffset(int column) => (column *(columnWidth + 3)) + 2;
        public int ColumnFromOffset(double offset) => (int)((offset -2)/(columnWidth + 3));
        public double MultiColWidth(int width) => (width * (columnWidth + 3)) - 3;

        public double MeasureElement(FrameworkElement fe, int column, int widthCols, double yOffset)
        {
            fe.Measure(new Size(MultiColWidth(widthCols), height));
            return fe.DesiredSize.Height;
        }
    }
}