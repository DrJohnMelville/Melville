using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Melville.WpfControls.CalendarControls;

public interface IClickToDayComputer
{
    Rect RectangleFromPoint(Point point);
    DateTime DateFromPoint(Point point);
}

public class ClickToDayComputer : IClickToDayComputer
{
    private readonly DateTime firstDayOnCalendar;
    private readonly IList<double> rowHeights;
    private readonly ColumnMeasure columnMeasure;

    public ClickToDayComputer(DateTime firstDayOnCalendar, double width, IList<double> rowHeights)
    {
        columnMeasure = new ColumnMeasure(new Size(width, 20));
        this.firstDayOnCalendar = firstDayOnCalendar;
        this.rowHeights = rowHeights;
    }

    public Rect RectangleFromPoint(Point point)
    {
        var (col,row) = PointToGridPosition(point);
        return new Rect(columnMeasure.ColumnOffset(col), RowOffset(row),
            columnMeasure.MultiColWidth(1), rowHeights[row]);
    }

    private double RowOffset(int row) => 
        rowHeights.Take(row).Aggregate(0.0, (i, j) => i + j);

    public DateTime DateFromPoint(Point point) => 
        firstDayOnCalendar.AddDays(DaysAfterFirstDate(point));

    private int DaysAfterFirstDate(Point point)
    {
        var (col, row) = PointToGridPosition(point);
        return col + (7 * row);
    }

    private (int, int) PointToGridPosition(Point point) =>
        (columnMeasure.ColumnFromOffset(point.X), RowFromY(point.Y));

    private int RowFromY(double y)
    {
        var hextRowYPosition = 0.0;
        for (var i = 0; i < rowHeights.Count; i++)
        {
            hextRowYPosition += rowHeights[i];
            if (hextRowYPosition > y) return i;
        }
        return rowHeights.Count - 1;
    }
}