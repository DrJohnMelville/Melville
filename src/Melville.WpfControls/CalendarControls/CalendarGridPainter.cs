using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Media;

namespace Melville.WpfControls.CalendarControls;

public ref struct CalendarGridPainter
{
    private DrawingContext dc;
    private DateTime firstDay;
    private int currentMonth;
    private Size renderSize;
    private readonly IList<double> rowHeights;
    private readonly Pen blackPen;
    private readonly double colWidth;
    private readonly double pixelsPerDip; 

    public CalendarGridPainter(
        DrawingContext dc, DateTime firstDay, int currentMonth, Size renderSize, double pixelsPerDip,
        IList<double> rowHeights)
    {
        this.dc = dc;
        this.firstDay = firstDay;
        this.currentMonth = currentMonth;
        this.renderSize = renderSize;
        this.pixelsPerDip = pixelsPerDip;
        this.rowHeights = rowHeights;
        blackPen = new Pen(Brushes.Black, 1);
        colWidth = renderSize.Width / 7;
    }

    public void PaintCalendar()
    {
        PaintGrid();
        PaintDayNumbers();
    }

    private void PaintDayNumbers()
    {
        var currentDate = firstDay;
        var currentPos = 0.0;
        for (int week = 0; week < rowHeights.Count; week++)
        {
            for (int day = 0; day < 7; day++)
            {
                var formattedText = CreateFormattedNumberText(currentDate);
                dc.DrawText(formattedText, new Point(day*colWidth, currentPos));
                currentDate = currentDate.AddDays(1);
            }

            currentPos += rowHeights[week];
        }
    }

    private static readonly Typeface numberTypeFace = 
        new Typeface(new FontFamily("Arial"), FontStyles.Normal,
            FontWeights.Normal,
            FontStretches.Normal);

    private FormattedText CreateFormattedNumberText(DateTime currentDate) =>
        new(currentDate.Day.ToString(),
            CultureInfo.CurrentCulture, FlowDirection.LeftToRight, 
            numberTypeFace, 20, DayNumberBrush(currentDate), new NumberSubstitution(),  
            TextFormattingMode.Display, pixelsPerDip);

    private SolidColorBrush DayNumberBrush(DateTime currentDate) => 
        IsCurrentlyActiveMonth(currentDate)? Brushes.Black : Brushes.DarkGray;

    private bool IsCurrentlyActiveMonth(DateTime currentDate) => currentDate.Month == currentMonth;

    private void PaintGrid()
    {
        PaintOuterBox();
        PaintColumnLines();
        PaintRowLines();
    }

    private void PaintRowLines()
    {
        double currentLine = 0.0;
        for (int i = 0; i < rowHeights.Count - 1; i++)
        {
            currentLine += rowHeights[i];
            dc.DrawLine(blackPen, new Point(0, currentLine), new Point(renderSize.Width, currentLine));
        }
    }

    private void PaintOuterBox()
    {
        dc.DrawRectangle(null, blackPen, new Rect(new Point(), renderSize));
    }

    private void PaintColumnLines()
    {
        for (int i = 1; i < 7; i++)
        {
            dc.DrawLine(blackPen, new Point(i * colWidth, 0), new Point(i * colWidth, renderSize.Height));
        }
    }
}