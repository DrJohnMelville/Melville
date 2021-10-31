using  System.Windows;

namespace Melville.MVVM.Wpf.MvvmDialogs;

public interface IMvvmDialog
{
  bool? ShowModalDialog(object viewModel, double width, double height, string title);
}

public sealed class MvvmDialog : IMvvmDialog
{
  private readonly Window parent;

  public MvvmDialog(Window parent)
  {
    this.parent = parent;
  }

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

}