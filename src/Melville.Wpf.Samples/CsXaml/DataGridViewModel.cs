using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Media;
using Melville.Linq.Statistics.Functional;
using Melville.MVVM.BusinessObjects;
using Melville.Mvvm.CsXaml;
using Melville.Mvvm.CsXaml.ValueSource;
using Melville.Mvvm.CsXaml.XamlBuilders;
using HorizontalAlignment = System.Windows.HorizontalAlignment;
using UserControl = System.Windows.Controls.UserControl;

namespace Melville.Wpf.Samples.CsXaml
{
    public class RowData: NotifyBase
    {
        private string name;
        public string Name
        {
            get => name;
            set => AssignAndNotify(ref name, value);
        }

        private int sortOrder;
        public int SortOrder
        {
            get => sortOrder;
            set
            {
                IsOdd = value % 2 == 1;
                AssignAndNotify(ref sortOrder, value);
            }
        }
        
        public int[] Choices => Enumerable.Range(0,4).Select(i=>i + SortOrder).ToArray();

        private bool isOdd;
        public bool IsOdd
        {
            get => isOdd;
            set => AssignAndNotify(ref isOdd, value);
        }

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
                .WithChild(Create.DataGrid(dc.BindList(i=>i.Data),
                    DataGridRows))
            );
        }

        private IEnumerable<DataGridColumn> DataGridRows(DataGridColumnGenerator<RowData> rgen)
        {  
            return new DataGridColumn[]
            {
                rgen.Text(rgen.Bind(i => i.Name), "TextBox", true), 
                rgen.Text("Foo", "Constant", true).WithForeground(Brushes.Blue), 
                rgen.ComboBox<int>(rgen.Bind(i => i.SortOrder),
                rgen.BindList(i => i.Choices).WithTracing(), "ComboBoxes"), 
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