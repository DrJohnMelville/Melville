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
      var viewTypeName = ViewTypeNameFromModel(model);
      if (viewTypeName == null) return DisplayMessage($"{model.GetType()} does not contain \"ViewModel\".");
      var targetType = model.GetType().Assembly.GetType(viewTypeName);
      if (targetType == null) return DisplayMessage($"Could Not Find Type {viewTypeName}.");
      if (!typeof(UIElement).IsAssignableFrom(targetType)) return DisplayMessage($"{viewTypeName} is not a UIElement.");
      return CreateView(targetType, frame);
    }

    private static UIElement CreateView(Type targetType, DependencyObject frame)
    {
      return (UIElement) (DiIntegration.SearchForContainer(frame).Get(targetType) ??
        DisplayMessage("DI container failed to create type "+ targetType.Name));
    }

    private static TextBlock DisplayMessage(string message, Brush? foreground = null) =>
      new TextBlock {Text = message, Foreground = foreground ?? Brushes.Red};

    private static string? ViewTypeNameFromModel(object model)
    {
      var modelName = model.GetType().ToString();
      var nonGeneric = Regex.Match(modelName, @"^([A-Za-z\.0-9_\+]+)").Groups[1].Value;
      var viewTypeName = nonGeneric.Replace("ViewModel", "View");
      return modelName.Equals(viewTypeName, StringComparison.Ordinal) ? null : viewTypeName;
    }
  }
}