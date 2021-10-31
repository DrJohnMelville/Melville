#nullable disable warnings
using System;
using System.Collections.Generic;
using  System.Linq;
using System.Windows;
using Melville.MVVM.Wpf.EventBindings;
using Moq;
using Xunit;

namespace Melville.MVVM.WPF.Test.EventBindings;

public sealed class TargetSelectorTest
{

  private IList<object> Targets(DependencyObject root) =>
    TargetSelector.ResolveTarget(root, new(Array.Empty<ITargetListExpander>()));
    
  [StaFact]
  public void EmptyFallThrough()
  {
    var elt = new FrameworkElement();
    var resolvedTargets = Targets(elt);
    Assert.Single(resolvedTargets); // just the element
    Assert.Equal(elt, resolvedTargets.First());
      
  }
  [StaFact]
  public void ExplicitTargetTest()
  {
    object target = new object();
    var elt = new FrameworkElement();
    TargetSelector.SetTarget(elt, target);
    Assert.Equal(target, Targets(elt).First());
  }
  [StaFact]
  public void ImplicitTargetTest()
  {
    object target = new object();
    var elt = new FrameworkElement();
    elt.DataContext = target;
    Assert.Equal(target, Targets(elt).First());
  }

  [StaFact]
  public void AdditionalTargetTest()
  {
    var target = new Mock<IAdditionlTargets>();
    target.Setup(i => i.Targets()).Returns(new object[] {3});
    var elt = new FrameworkElement();
    elt.DataContext = target.Object;

    var targets = Targets(elt).ToList();

    Assert.Equal(3, targets.Count);
    Assert.Equal(3, targets.First());
  }
}