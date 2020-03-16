using System.Diagnostics.CodeAnalysis;
using  System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Melville.WpfControls.DataGrids
{
  public class SingleClickDataGrid : DataGrid
  {
    // this solution comes from user2134678's response at 
    // http://stackoverflow.com/a/15218130/50088
    public SingleClickDataGrid()
    {
      GotFocus += DataGrid_CellGotFocus;
    }


    private void DataGrid_CellGotFocus(object sender, RoutedEventArgs e)
    {
      // Hack within a hack to make shift clicking work
      // https://stackoverflow.com/questions/29072008/wpf-datagrid-shift-tab-reverse-tabbing-no-longer-working-after-enabled-si
      if (IsShiftDown()) return;


      // Lookup for the source to be DataGridCell
      if (e.OriginalSource is DataGridCell dgc)
      {
        // Starts the Edit on the row;
        DataGrid grd = (DataGrid)sender;
        grd.BeginEdit(e);
        GetFirstChildByType(dgc)?.Focus();
      }
    }

    private static bool IsShiftDown() => Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift);

    private Control? GetFirstChildByType(DependencyObject prop)
    {
      for (int i = 0; i < VisualTreeHelper.GetChildrenCount(prop); i++)
      {
        DependencyObject child = VisualTreeHelper.GetChild((prop), i);
     
        if (child is Control childAsT) return childAsT;
  
        var castedProp = GetFirstChildByType(child);
        if (castedProp != null) return castedProp;
      }
      return null!;
    }
  }
}