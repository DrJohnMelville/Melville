using  System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Melville.INPC;
using Melville.MVVM.Wpf.DiParameterSources;

namespace Melville.MVVM.Wpf.ViewFrames
{
  public sealed partial class ViewFrame: Decorator
  {

    [GenerateDP]
    private void OnContentChanged(object? newValue)
    {
      Child = GenerateChild(newValue);
      RunMethodsFromOnDisplayedAttributes(newValue);
    }


    public ViewFrame()
    {
#if DEBUG
      Child = DisplayMessage("Content Never Set");
#endif
    }

    private UIElement GenerateChild(object? value)
    {
      var ret = CreateViewObject(value);
      if (ret is FrameworkElement fe) { fe.DataContext = value; }
      return ret;
    }

    private void RunMethodsFromOnDisplayedAttributes(object? viewModel)
    {
      if (viewModel == null) return;
      foreach (var initializer in viewModel.GetType().GetCustomAttributes().OfType<OnDisplayedAttribute>())
      {
        initializer.DoCall(viewModel, this);
      }
    }

    private UIElement CreateViewObject(object? model) =>
      model switch
      {
        string s => DisplayMessage(s, SystemColors.WindowTextBrush),
        null => DisplayMessage("<Null>"),
        ICreateView icv => icv.View(),
        _ => CreateFromConvention(model)
      };

    private UIElement CreateFromConvention(object model)
    {
      var container = DiIntegration.SearchForContainer(this);
      return (GetMappingConvention(container) is {} convention)?
        convention.ViewFromViewModel(model, container) :
        DisplayMessage("Could not find view mapping convention");
    }

    private  IViewMappingConvention? GetMappingConvention(IDIIntegration container) => 
      container.Get(typeof(IViewMappingConvention)) as IViewMappingConvention;


    private TextBlock DisplayMessage(string message, Brush? foreground = null) =>
      new() {Text = message, Foreground = foreground ?? Brushes.Red};
  }
}