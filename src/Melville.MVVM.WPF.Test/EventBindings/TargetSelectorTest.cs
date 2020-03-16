#nullable disable warnings
using  System.Linq;
using System.Windows;
using Melville.MVVM.Wpf.EventBindings;
using Moq;
using Xunit;

namespace Melville.MVVM.WPF.Test.EventBindings
{
  public sealed class TargetSelectorTest
  {
    [StaFact]
    public void EmptyFallThrough()
    {
      var elt = new FrameworkElement();
      var resolvedTargets = TargetSelector.ResolveTarget(elt).ToList();
      Assert.Single(resolvedTargets); // just the element
      Assert.Equal(elt, resolvedTargets.First());
      
    }
    [StaFact]
    public void ExplicitTargetTest()
    {
      object target = new object();
      var elt = new FrameworkElement();
      TargetSelector.SetTargetProperty(elt, target);
      Assert.Equal(target, TargetSelector.ResolveTarget(elt).First());
    }
    [StaFact]
    public void ImplicitTargetTest()
    {
      object target = new object();
      var elt = new FrameworkElement();
      elt.DataContext = target;
      Assert.Equal(target, TargetSelector.ResolveTarget(elt).First());
    }

    [StaFact]
    public void AdditionalTargetTest()
    {
      var target = new Mock<IAdditionlTargets>();
      target.Setup(i => i.Targets()).Returns(new object[] {3});
      var elt = new FrameworkElement();
      elt.DataContext = target.Object;

      var targets = elt.AllSources().ToList();

      Assert.Equal(3, targets.Count);
      Assert.Equal(3, targets.First());
      
      
    }


  }
}