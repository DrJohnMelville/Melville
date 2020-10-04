using  System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Melville.MVVM.Wpf.DiParameterSources;

namespace Melville.MVVM.Wpf.ViewFrames
{
  public sealed class ViewFrame: Decorator
  {
    public object Content
    {
      get { return GetValue(ContentProperty); }
      set { SetValue(ContentProperty, value); }
    }

    // Using a DependencyProperty as the backing store for Content.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ContentProperty =
        DependencyProperty.Register("Content", typeof(object), typeof(ViewFrame), 
          new FrameworkPropertyMetadata(null, ContentChanged));



    private static void ContentChanged(DependencyObject target, DependencyPropertyChangedEventArgs e)
    {
      ((ViewFrame) target).Child = GenerateChild(e.NewValue, target);
    }


    public ViewFrame()
    {
#if DEBUG
      Child = DisplayMessage("Content Never Set");
#endif
    }

    private static UIElement GenerateChild(object value, DependencyObject frame)
    {
      var ret = CreateViewObject(value, frame);
      if (ret is FrameworkElement fe) { fe.DataContext = value; }
      RunMethodsFromOnDisplayedAttributes(frame, value);
      return ret;
    }

    private static void RunMethodsFromOnDisplayedAttributes(DependencyObject frame, object viewModel)
    {
      if (viewModel == null) return;
      foreach (var initializer in viewModel.GetType().GetCustomAttributes().OfType<OnDisplayedAttribute>())
      {
        initializer.DoCall(viewModel, frame);
      }
    }

    private static UIElement CreateViewObject(object model, DependencyObject frame) =>
      model switch
      {
        string s => DisplayMessage(s, SystemColors.WindowTextBrush),
        null => DisplayMessage("<Null>"),
        ICreateView icv => icv.View(),
        _ => CreateFromConvention(model, frame)
      };

    private static UIElement CreateFromConvention(object model, DependencyObject frame)
    {
      var container = DiIntegration.SearchForContainer(frame);
      return (GetMappingConvention(container) is {} convention)?
        convention.ViewFromViewModel(model, container) :
        DisplayMessage("Could not find view mapping convention");
    }

    private static IViewMappingConvention? GetMappingConvention(IDIIntegration container) => 
      container.Get(typeof(IViewMappingConvention)) as IViewMappingConvention;


    private static TextBlock DisplayMessage(string message, Brush? foreground = null) =>
      new TextBlock {Text = message, Foreground = foreground ?? Brushes.Red};
  }
}