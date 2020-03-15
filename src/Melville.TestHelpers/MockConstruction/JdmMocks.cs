using  System;
using System.Linq.Expressions;
using Moq;
using Moq.Language;

namespace Melville.TestHelpers.MockConstruction
{
  public static class JdmMocks
  {
    public static T SingleGetMock<T, U>(Expression<Func<T, U>> selector, U value) where T : class
    {
      var mock = new Mock<T>();
      mock.SetupGet(selector).Returns(value);
      return mock.Object;
    }

    public static object ReturnsMock<TMock, TProperty>(this IReturnsGetter<TMock, TProperty> item)
      where TMock : class where TProperty: class =>item.Returns(Mock.Of<TProperty>());
    public static object ReturnsMock<TMock, TProperty>(this IReturns<TMock, TProperty> item)
      where TMock : class where TProperty: class =>item.Returns(Mock.Of<TProperty>());
  }
}