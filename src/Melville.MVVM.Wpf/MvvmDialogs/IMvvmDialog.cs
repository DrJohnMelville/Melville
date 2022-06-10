using  System.Windows;
using Melville.INPC;

namespace Melville.MVVM.Wpf.MvvmDialogs;

public interface IMvvmDialog
{
  bool? ShowModalDialog(object viewModel, double width, double height, string title);
  void ShowPopupWindow(object viewModel, double width, double height, string title);
}

public sealed partial class MvvmDialog : IMvvmDialog
{
  [FromConstructor]private readonly Window parent;

  public bool? ShowModalDialog(object viewModel, double width, double height, string title)
  {
    var window = new WrapperDialog
    {
      Owner = parent,
      Width = width,
      Height = height,
      Title = title,
      DataContext = viewModel
    };

    return window.ShowDialog();
  }

  public void ShowPopupWindow(object viewModel, double width, double height, string title)
  {
    var window = new WrapperWindow()
    {
      Owner = parent,
      Width = width,
      Height = height,
      Title = title,
      DataContext = viewModel
    };

    window.Show();
  }
}