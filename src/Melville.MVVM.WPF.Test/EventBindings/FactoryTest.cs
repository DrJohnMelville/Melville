#nullable disable warnings
using  System;
using System.Windows;
using Melville.MVVM.Wpf.EventBindings;
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
      var ea = new object[]{new EventArgs()};
      var par = new FrameworkElement();
      par.DataContext = fact.Object;
      fact.SetupGet(i => i.TargetType).Returns(typeof(string));
      fact.Setup(i => i.Create(par, ea)).Returns("FooBar");

      Assert.True(ParameterResolver.ResolveParameter(typeof(string), par, ea, out var result));
      Assert.Equal("FooBar", result.ToString());
    }

    [Fact]
    public void UniqueFactoryTest()
    {
      var uf = Factory.Unique((a, b) => "Hello"+1+"2");
      var obj1 = uf.Create(null, null);
      var obj2 = uf.Create(null, null);
      Assert.Equal("Hello12", obj1);
      Assert.NotSame(obj1, obj2);
    }

  }
}