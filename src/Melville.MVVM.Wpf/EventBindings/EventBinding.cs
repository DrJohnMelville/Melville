using  System;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;
using Melville.MVVM.Wpf.EventBindings.SearchTree;

namespace Melville.MVVM.Wpf.EventBindings
{
  public class EventBinding : MarkupExtension
  {
    public EventBinding()
    {
    }

    /// <summary>
    /// Bind an event or a command to a method call.  Will search tree for an appropriate method
    /// </summary>
    /// <param name="methodName">Method name to call</param>
    public EventBinding(string methodName):this()
    {
      MethodName = methodName;
    }
    /// <summary>
    /// Bind an event or a command to a method call.  Will search tree for an appropriate method
    /// </summary>
    /// <param name="methodName">Method name to call</param>
    /// <param name="parameters">A comma delimited list of dot expressions rooted to either $this or $arg, which should be passed to the IOC mechanism</param>
    public EventBinding(string methodName, string parameters):this(methodName)
    {
      Parameters = parameters;
    }

    /// <summary>
    /// Name of the method to find
    /// </summary>
    public string MethodName { get; set; } = "";
    /// <summary>
    /// A comma delimited list of dot expressions rooted to either $this or $arg, which should be passed to the IOC mechanism
    /// </summary>
    public string Parameters { get; set; } = "";
    /// <summary>
    /// Maximum number of times this method should be called
    /// </summary>
    public int MaxCalls { get; set; } = int.MaxValue;

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
      var targetInfo = ProvideValueTargetInfo(serviceProvider);
      if (targetInfo == null) return DependencyProperty.UnsetValue;
      if (targetInfo.TargetObject is Setter) return this;
      return targetInfo.TargetProperty switch
      {
        EventInfo e => (object) new SearchTreeEventTarget(MethodName, Parameters, MaxCalls).CreateDelegate(e.EventHandlerType),
        // This may not be a really happen, it may just be an issue with the wpf 4.5 prerelease code.
        // it  doesn't hurt anything, so leave it in just in case it rarely happens
       // http://www.jonathanantoine.com/2011/09/23/wpf-4-5s-markupextension-invoke-a-method-on-the-viewmodel-datacontext-when-an-event-is-raised/
        
        MethodInfo m => new SearchTreeEventTarget(MethodName, Parameters, MaxCalls).CreateDelegate(m.GetParameters()[1]
          .ParameterType),
        
        PropertyInfo pr when pr.PropertyType.IsAssignableFrom(typeof(ICommand)) => 
          new SearchTreeCommandTarget((DependencyObject)targetInfo.TargetObject, MethodName, Parameters, MaxCalls),

        DependencyProperty dp when dp.PropertyType.IsAssignableFrom(typeof(ICommand)) => 
          new SearchTreeCommandTarget((DependencyObject)targetInfo.TargetObject, MethodName, Parameters, MaxCalls),
        _ => throw new InvalidDataException("Must bind to a routedEvent or command property")
      };
    }

    private static IProvideValueTarget? ProvideValueTargetInfo(IServiceProvider serviceProvider)
    {
      return (IProvideValueTarget?)serviceProvider.GetService(typeof(IProvideValueTarget));
    }
  }
}