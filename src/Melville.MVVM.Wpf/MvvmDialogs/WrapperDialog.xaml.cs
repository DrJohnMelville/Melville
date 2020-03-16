using  System.Collections.Generic;
using System.Windows;
using Melville.MVVM.Wpf.EventBindings;

namespace Melville.MVVM.Wpf.MvvmDialogs
{
  /// <summary>
  /// Interaction logic for WrapperDialog.xaml
  /// </summary>
  public partial class WrapperDialog : Window, IAdditionlTargets
  {
    public WrapperDialog()
    {
      InitializeComponent();
    }

    private void OkClicked(object sender, RoutedEventArgs e)
    {
      DialogResult = true;
      Close();
    }

    public IEnumerable<object> Targets() =>
      Owner is IAdditionlTargets iat ? iat.Targets() : new object[0];

  }
}
