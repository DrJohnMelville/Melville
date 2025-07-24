#nullable disable warnings
using  System;
using System.Reflection;
using System.Windows;
using System.Windows.Markup;
using Melville.MVVM.Wpf.DiParameterSources;
using Melville.MVVM.Wpf.EventBindings;
using Moq;

namespace Melville.MVVM.WPF.Test.EventBindings;

public class TestWithServiceProvider
{
  protected readonly FrameworkElement Elt = new FrameworkElement {Tag = "Foo"};
  protected readonly RoutedEventArgs RoutedEventArgs =
    new RoutedEventArgs { RoutedEvent = FrameworkElement.LoadedEvent };

  protected IServiceProvider CreateServiceProvider()
  {
    var sp = new Mock<IServiceProvider>();
    var ipv = new Mock<IProvideValueTarget>();
    sp.Setup(i => i.GetService(It.IsAny<Type>())).Returns(ipv.Object);

    ipv.SetupGet(i => i.TargetObject).Returns(Elt);
    ipv.SetupGet(i => i.TargetProperty).Returns(Elt.GetType().GetEvent("Loaded"));
    var serviceProvider = sp.Object;
    return serviceProvider;
  }

  protected object FireEvent(EventBinding markup)
  {
    var del = markup.ProvideValue(CreateServiceProvider()) as Delegate;
    DiIntegration.SetContainer(Elt, new FakeDIBridge());
    return del.DynamicInvoke(Elt, RoutedEventArgs);
  }
}

public class FakeDIBridge : IDIIntegration
{
    /// <inheritdoc />
    public void Dispose()
    {
    }

    /// <inheritdoc />
    public IDIIntegration CreateScope() => this;

    /// <inheritdoc />
    public object? Get(ParameterInfo info) => null;

    /// <inheritdoc />
    public object? Get(Type type)
    {
        if (type == typeof(TargetListCompositeExpander))
            return new TargetListCompositeExpander(Array.Empty<ITargetListExpander>());
        if (type == typeof(ParameterListCompositeExpander))
            return new ParameterListCompositeExpander(Array.Empty<IParameterListExpander>());
        return null;
    }
}