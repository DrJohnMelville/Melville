using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Melville.Mvvm.CsXaml
{
    public class DataGridComboBoxColumnWithBinding : DataGridComboBoxColumn
    {
        protected override FrameworkElement GenerateEditingElement(DataGridCell cell, object dataItem)
        {
            FrameworkElement element = base.GenerateEditingElement(cell, dataItem);
            CopyItemsSource(element);
            return element;
        }
 
        protected override FrameworkElement GenerateElement(DataGridCell cell, object dataItem)
        {
            FrameworkElement element = base.GenerateElement(cell, dataItem);
            CopyItemsSource(element);
            return element;
        }
 
        private void CopyItemsSource(FrameworkElement element)
        {
            BindingOperations.SetBinding(element, ComboBox.ItemsSourceProperty,
                BindingOperations.GetBinding(this, ComboBox.ItemsSourceProperty));
        }
    }
}