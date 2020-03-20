#nullable disable warnings
using  System;
using System.Windows;
using Melville.MVVM.Wpf.DiParameterSources;
using Melville.MVVM.Wpf.EventBindings;
using Melville.MVVM.Wpf.EventBindings.ParameterResolution;
using Moq;
using Xunit;

namespace Melville.MVVM.WPF.Test.EventBindings
{
  public sealed class FactoryTest
  {
    [StaFact]
    public void InvokeFactoryTest()
    {
      var fact = new Mock<IFactory>();
      var inputParams = new object[]{new EventArgs()};
      var par = new FrameworkElement();
      par.DataContext = fact.Object;
      fact.SetupGet(i => i.TargetType).Returns(typeof(string));
      fact.Setup(i => i.Create(Mock.Of<IDIIntegration>(),par, inputParams)).Returns("FooBar");
      
      Assert.True(ParameterResolver.ResolveParameter(typeof(string), par, inputParams, out var result));
      Assert.Equal("FooBar", result.ToString());
    }

    [Fact]
    public void UniqueFactoryTest()
    {
      var uf = Factory.Unique((a, b) => "Hello"+1+"2");
      var obj1 = uf.Create(Mock.Of<IDIIntegration>(),null, null);
      var obj2 = uf.Create(Mock.Of<IDIIntegration>(),null, null);
      Assert.Equal("Hello12", obj1);
      Assert.NotSame(obj1, obj2);
    }

  }
}