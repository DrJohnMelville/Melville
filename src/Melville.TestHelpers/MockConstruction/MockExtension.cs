using  System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Moq;

namespace Melville.TestHelpers.MockConstruction;

public static class MockExtension
{
  public static void SetupMethod(Mock mock, Type parentObjectType, MethodInfo method, object? value) => 
    InvokeReturnMethod(method.ReturnType, SetMethodCall(mock, parentObjectType, method), value);

  private static object? SetMethodCall(Mock mock, Type parentObjectType, MethodInfo method)
  {
    var call = MethodCallLambda(parentObjectType, method);
    var setupDecl = mock.GetType().GetMethods().Single(i => i.Name == "Setup" && i.IsGenericMethod);
    var specificDecl = setupDecl.MakeGenericMethod(method.ReturnType);
    return specificDecl.Invoke(mock, new object[] { call });
  }

  private static LambdaExpression MethodCallLambda(Type parentObjectType, MethodInfo method)
  {
    var args = method.GetParameters().Select(i => IsAnyCall(i.ParameterType)).ToArray();
    var lambdaParam = Expression.Parameter(parentObjectType, "i");
    var body = Expression.Call(lambdaParam, method, args);
    return Expression.Lambda(body, lambdaParam);
  }

  private static Expression IsAnyCall(Type target)
  {
    var genericMethod = typeof(It).GetMethod("IsAny");
    var concreteMethod = genericMethod.MakeGenericMethod(target);
    return Expression.Call(concreteMethod);
  }

  /// <summary>
  /// Set a property to return a value without knowing the generic type of the object
  /// </summary>
  /// <param name="mockedObjectType">Type of the object being mocked.</param>
  /// <param name="mock">The mock object</param>
  /// <param name="propertyType">Type of the property to be set</param>
  /// <param name="propertyName">Name of the property to be set/</param>
  /// <param name="propertyValue">Value of the property</param>
  public static void SetupGet(Type mockedObjectType, Mock mock, Type propertyType, string propertyName, object? propertyValue)
  {
    var setupGetResult = InvokeSetupGetMethod(mockedObjectType, mock, propertyType, propertyName);

    InvokeReturnMethod(propertyType, setupGetResult, propertyValue);
  }

  private static object InvokeSetupGetMethod(Type mockedObjectType, Mock mock, Type propertyType, string propertyName)
  {
    var lambda = PropertyGetLambda(mockedObjectType, propertyName);

    var type = mock.GetType();
    var genericMethod = type.GetMethod("SetupGet");
    var makeGenericMethod = genericMethod.MakeGenericMethod(propertyType);
    var setupGetResult =
      makeGenericMethod.Invoke(mock, new[] { lambda });
    return setupGetResult;
  }

  private static void InvokeReturnMethod(Type propertyType, object setupActionType, object? propertyValue)
  {
    GetMethodBySig(setupActionType.GetType(), "Returns", propertyType).Invoke(setupActionType, new[] { propertyValue });
  }

  /// <summary>
  /// Give a type, a method name, and a set of parameter types, find a methodinfo for a public method with that signature
  /// </summary>
  /// <param name="setupGetterType">Type of the object to search</param>
  /// <param name="methodName">Name of the method to be found.</param>
  /// <param name="parameterTypes">Types of the parameters to the method</param>
  /// <returns>A matching methodinfo, or null if there is no match/</returns>
  private static MethodInfo GetMethodBySig(Type setupGetterType, string methodName, params Type[] parameterTypes)
  {
    return setupGetterType.GetMethod(methodName, BindingFlags.Public | BindingFlags.InvokeMethod | BindingFlags.Instance,
      null, CallingConventions.Any, parameterTypes, null);
  }

  private static LambdaExpression PropertyGetLambda(Type parentObjectType, string propertyName)
  {
    var parameter = Expression.Parameter(parentObjectType);
    return Expression.Lambda(Expression.Property(parameter, propertyName), parameter);
  }

}