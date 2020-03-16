using System.Windows;
using System.Windows.Controls;

namespace Melville.WpfControls.Hacks
{
  public static class GridExtensions
  {


    public static int GetAutoRows(DependencyObject obj)
    {
      return (int)obj.GetValue(AutoRowsProperty);
    }

    public static void SetAutoRows(DependencyObject obj, int value)
    {
      obj.SetValue(AutoRowsProperty, value);
    }

    // Using a DependencyProperty as the backing store for AutoRows.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty AutoRowsProperty =
        DependencyProperty.RegisterAttached("AutoRows", typeof(int), typeof(GridExtensions), 
          new FrameworkPropertyMetadata(0, AutoRowsChanged));

    private static void AutoRowsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      if (d is Grid grid)
      {
        grid.RowDefinitions.Clear();
        for (int i = 0; i < (int)e.NewValue; i++)
        {
          grid.RowDefinitions.Add(new RowDefinition() {Height = GridLength.Auto});
        }
      }
    }
  }
}
