using  System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Melville.WpfControls.Panels;

public sealed class FixedColumnFlowPanel: TestablePanel
{


  public int Columns
  {
    get { return (int)GetValue(ColumnsProperty); }
    set { SetValue(ColumnsProperty, value); }
  }

  // Using a DependencyProperty as the backing store for Columns.  This enables animation, styling, binding, etc...
  public static readonly DependencyProperty ColumnsProperty =
    DependencyProperty.Register("Columns", typeof(int), typeof(FixedColumnFlowPanel), 
      new FrameworkPropertyMetadata(1, FrameworkPropertyMetadataOptions.AffectsArrange |
                                       FrameworkPropertyMetadataOptions.AffectsMeasure));

  public class MeasurementRowHeight
  {
    public IList<int> Items = new List<int>();
    public Double RowHeight { get; private set; }

    public void AddItem(int item, double itemHeight)
    {
      RowHeight = Math.Max(itemHeight, RowHeight);
      Items.Add(item);
    }
  }

  private readonly IList<MeasurementRowHeight> measurements = new List<MeasurementRowHeight>();

  public override Size MeasureOverride(IPanelAdapter adapter, Size availaibleSize)
  {
    measurements.Clear();
    var cellSize = new Size();
    for (int i = 0; i < adapter.ChildrenCount; i++)
    {
      if (i % Columns == 0)
      {
        cellSize = new Size(availaibleSize.Width / Columns, 
          availaibleSize.Height - measurements.Sum(j=>j.RowHeight));
        measurements.Add(new MeasurementRowHeight());
      }
      adapter.Measure(i, cellSize);
      measurements.Last().AddItem(i,adapter.GetDesiredSize(i).Height);
    }
      
    return new Size(availaibleSize.Width, measurements.Sum(i=>i.RowHeight));
  }

  public override Size ArrangeOverride(IPanelAdapter adapter, Size finalSize)
  {
    double usedHeight = 0;
    var colWidth = finalSize.Width / Columns;
    foreach (var row in measurements)
    {
      for (int i = 0; i < row.Items.Count; i++)
      {
        adapter.Arrange(row.Items[i], i * colWidth, usedHeight, colWidth, row.RowHeight);
      }
      usedHeight += row.RowHeight;
    }
    return new Size(finalSize.Width, usedHeight);
  }

}