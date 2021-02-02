using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Melville.INPC;
using Melville.MVVM.BusinessObjects;
using Melville.Mvvm.CsXaml;
using Melville.Mvvm.CsXaml.XamlBuilders;
using Melville.MVVM.Functional;
using Melville.WpfControls.FilteredDataGrids;

namespace Melville.Wpf.Samples.CsXaml
{
    public partial class RowData
    {
        [AutoNotify]private string name;

        [AutoNotify]private int sortOrder;

        partial void WhenSortOrderChanges(int oldValue, int newValue)
        {
            IsOdd = newValue % 2 == 1;
        }
        
        public int[] Choices => Enumerable.Range(0,4).Select(i=>i + SortOrder).ToArray();

        [AutoNotify] private bool isOdd;
        public RowData(string name, int sortOrder)
        {
            this.name = name;
            SortOrder = sortOrder;
        }
    }
    public class DataGridViewModel
    {
        public IList<RowData> Data { get; } = new List<RowData>()
        {
            new RowData("Apple", 1),
            new RowData("Baby", 2),
            new RowData("Chucky", 3)
        };
    }

    public class DataGridView : UserControl
    {
        public DataGridView()
        {
            var dc = new BindingContext<DataGridViewModel>();
            AddChild(new StackPanel()
                .WithChild(Create.TextBlock("Data Grid Demo", 18, HorizontalAlignment.Center).WithMargin(20))
                .WithChild(Create.DataGrid(dc.BindList(i=>i.Data),DataGridRows)
                    .Fix(i=>FilteredDataGrid.SetUseFilter(i, true))));
        }

        private IEnumerable<DataGridColumn> DataGridRows(DataGridColumnGenerator<RowData> rgen)
        {  
            return new DataGridColumn[]
            {
                rgen.Text(rgen.Bind(i => i.Name), "TextBox", true), 
                rgen.Text("Foo", "Constant", true).WithForeground(Brushes.Blue), 
                rgen.ComboBox<int>(rgen.Bind(i => i.SortOrder),
                    rgen.BindList(i => i.Choices), "ComboBoxes")
                    .WithWidth(new DataGridLength(1.0, DataGridLengthUnitType.Star)), 
                rgen.CheckBox(rgen.Bind(i => i.IsOdd), "Checboxes"), 
                rgen.Template("Template", DisplayTemplate, i=>Create.TextBox(i.Bind(j=>j.Name)))
            };  
        }

        private object DisplayTemplate(BindingContext<RowData> i)
        {
            return Create.TextBlock(i.Bind(j => j.Name))
                .WithStyle(Create.Style<TextBlock>().WithFontSize(23))
                .WithLayoutTransform(new RotateTransform().WithAngle(i.Bind(j=>j.SortOrder, j=>j*20.0)))
                .WithToolTip(i.Bind(j=>j.SortOrder));
        }
    }
}