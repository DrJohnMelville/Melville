using  System;
using System.Linq.Expressions;
using Moq;

namespace Melville.TestHelpers.MockConstruction;

public static class MakeMock
{
  public static T Of<T>(Action<Mock<T>> initalize) where T:class
  {
    var ret = new Mock<T>();
    initalize(ret);
    return ret.Object;
  }

  public static T WithProp<T, TProperty>(Expression<Func<T, TProperty>> expression, TProperty value) where T : class => 
    Of<T>(i => i.SetupGet(expression).Returns(value));
  public static T WithProp<T, TProperty>(Expression<Func<T, TProperty>> expression, Func<TProperty> value) where T : class => 
    Of<T>(i => i.SetupGet(expression).Returns(value));
}