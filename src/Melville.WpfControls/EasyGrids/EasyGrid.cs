using  System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Melville.WpfControls.EasyGrids
{
  public enum EasyGridFlow
  {
    None = 0,
    LeftToRight = 1,
    RightToLeft = 2
  }

  public sealed class EasyGrid : Grid
  {
    public static readonly DependencyProperty ForceRowProperty =
      DependencyProperty.RegisterAttached("ForceRow", typeof(int), typeof(EasyGrid),
        new PropertyMetadata(-1));

    public static int GetForceRow(DependencyObject obj)
    {
      return (int)obj.GetValue(ForceRowProperty);
    }

    public static void SetForceRow(DependencyObject obj, int value)
    {
      obj.SetValue(ForceRowProperty, value);
    }

    public static readonly DependencyProperty ForceColumnProperty =
      DependencyProperty.RegisterAttached("ForceColumn", typeof(int), typeof(EasyGrid),
        new PropertyMetadata(-1));

    public static int GetForceColumn(DependencyObject obj)
    {
      return (int)obj.GetValue(ForceColumnProperty);
    }

    public static void SetForceColumn(DependencyObject obj, int value)
    {
      obj.SetValue(ForceColumnProperty, value);
    }



    public static EasyGridFlow GetFlow(DependencyObject obj)
    {
      return (EasyGridFlow)obj.GetValue(FlowProperty);
    }

    public static void SetFlow(DependencyObject obj, EasyGridFlow value)
    {
      obj.SetValue(FlowProperty, value);
    }

    // Using a DependencyProperty as the backing store for Flow.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty FlowProperty =
        DependencyProperty.RegisterAttached("Flow", typeof(EasyGridFlow), typeof(EasyGrid), new PropertyMetadata(EasyGridFlow.None));



    public Thickness CellMargins
    {
      get { return (Thickness)GetValue(CellMarginsProperty); }
      set { SetValue(CellMarginsProperty, value); }
    }

    // Using a DependencyProperty as the backing store for CellMargins.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty CellMarginsProperty =
        DependencyProperty.Register("CellMargins", typeof(Thickness), typeof(EasyGrid), new PropertyMetadata(new Thickness()));

    public override void EndInit()
    {
      LayoutGrid();
      base.EndInit();
    }

    private void LayoutGrid()
    {
      var excluded = new HashSet<Tuple<int, int>>();

      var nextRow = 0;
      var nextCol = 0;
      var direction = EasyGridFlow.LeftToRight;

      foreach (UIElement? child in InternalChildren)
      {
        if (child == null) continue;
        var newFlow = GetFlow(child);
        direction = newFlow == EasyGridFlow.None ? direction : newFlow;

        nextRow = TryForceValue(nextRow, GetForceRow(child));
        nextCol = TryForceValue(nextCol, GetForceColumn(child));


        SetRow(child, nextRow);
        SetColumn(child, nextCol);

        SetCellMargins(child);

        var rowSpan = GetRowSpan(child);
        for (int i = 1; i < rowSpan; i++)
        {
          excluded.Add(Tuple.Create(i + nextRow, nextCol));
        }

        nextCol += Math.Max(GetColumnSpan(child), 1) - 1;
        do
        {
          if (direction == EasyGridFlow.LeftToRight)
          {
            nextCol++;
            if (nextCol >= ColumnDefinitions.Count)
            {
              nextCol = 0;
              nextRow = (nextRow + 1) % MaxRows();
            }
          }
          else
          {
            nextCol--;
            if (nextCol < 0)
            {
              nextCol = ColumnDefinitions.Count - 1;
              nextRow = (nextRow + 1) % MaxRows();
            }
          }
        } while (excluded.Contains(Tuple.Create(nextRow, nextCol)));
      }

      EnsureAdequateRows(nextRow, nextCol);
    }

    private static int TryForceValue(int oldValue, int newValue)
    {
      return newValue >= 0 ? newValue : oldValue;
    }

    private int MaxRows()
    {
      return RowDefinitions.Count > 0 ? RowDefinitions.Count : int.MaxValue;
    }

    private void EnsureAdequateRows(int nextRow, int nextCol)
    {
      if (RowDefinitions.Count == 0)
      {
        if (nextCol == 0)
        {
          nextRow--;
        }

        nextRow++;
        for (int i = 0; i < nextRow; i++)
        {
          RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
        }
      }
    }

    private void SetCellMargins(UIElement child)
    {
      var existMargin = child.GetValue(MarginProperty) as Thickness?;
      if (existMargin.HasValue &&
          existMargin.Value.Left < 0.1 &&
          existMargin.Value.Right < 0.1 &&
          existMargin.Value.Top < 0.1 &&
          existMargin.Value.Bottom < 0.1)
      {
        child.SetValue(MarginProperty, CellMargins);
      }
    }
  }
}