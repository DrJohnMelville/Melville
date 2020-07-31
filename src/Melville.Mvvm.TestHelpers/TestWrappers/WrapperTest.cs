#nullable disable warnings
using  System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Melville.MVVM.CSharpHacks;
using Moq;
using Xunit;

namespace Melville.Mvvm.TestHelpers.TestWrappers
{
  /// <summary>
  /// This class verifices that a class is a wrapper to an an interface.
  /// A "wrapper" is a decorator with the following properties.
  /// 1. Implements the same interface as the wrapped interface.
  /// 2. Each call to the wrapper results in exactly one coll to the same method on the target,
  ///    with the same arguments.
  /// </summary>
  /// <typeparam name="T"></typeparam>
  public sealed class WrapperTest<T>
  {
    private T source;
    private TestProxy targetProxy;
    private readonly CreateDefaults defaults = new CreateDefaults();
    private readonly HashSet<MemberInfo> excludedMembers = new HashSet<MemberInfo>();
    
    public WrapperTest(Func<T, T> createWrapper)
    {
      T target = DispatchProxy.Create<T, TestProxy>();
      targetProxy = target as TestProxy;
      targetProxy.Defaults = defaults;
      source = createWrapper(target);
    }
    public void RegisterTypeCreator<TRegistered>(Func<TRegistered> func) => defaults.Register(func);

    public void ExcludeMember(Expression<Action<T>> memberToExclude) =>
      excludedMembers.Add(memberToExclude.ExtractMemberInfo());
    
    public void AssertAllMethodsForward()
    {
      foreach (var method in MethodsToTest())
      {
        TestSingleMethod(method);
      }
    }

    private IEnumerable<MethodInfo> MethodsToTest()
    {
      var stack = new Stack<Type>();
      stack.Push(typeof(T));
      while (stack.Any())
      {
        var definition = stack.Pop();
        foreach (var parent in definition.GetInterfaces())
        {
          stack.Push(parent);
        }

        foreach (var method in definition.GetMethods().Where((i=>!excludedMembers.Contains(i))))
        {
          yield return method;
        }
      }
    }

    private void TestSingleMethod(MethodInfo method)
    {
      targetProxy.Reset();
      var parameters = defaults.DefaultFor(method.GetParameters());
      var returnVal = method.Invoke(source, parameters);
      targetProxy.VerifyCall(method, parameters, returnVal);
    }
  }
  public class TestProxy:DispatchProxy
  {
    public CreateDefaults Defaults { get; set; }
    public List<(MethodInfo info,object[] arguments, object returnVal)> Calls { get; } = 
      new List<(MethodInfo info, object[] arguments, object returnVal)>();

    public void Reset() => Calls.Clear();

    public void VerifyCall(MethodInfo method, Object[] args, object returnVal)
    {
      Assert.True(Calls.Count() == 1, $"Call to {method.Name} resulted in {Calls.Count()} target calls.");
      VerifySingleCall(method, args, returnVal, Calls[0]);
    }

    private void VerifySingleCall(MethodInfo method, object[] args, object returnVal, 
      (MethodInfo info, object[] arguments, Object returnVal) call)
    {
      Assert.True(call.info == method, $"Call to {method.Name} resulted in call to {call.info.Name}");
      Assert.Equal(args.Length, call.arguments.Length);
      for (int i = 0; i < args.Length && i < call.arguments.Length; i++)
      { 
        Assert.True(object.Equals(args[i], call.arguments[i]), $"Method '{method.Name}' argument {i} mismatch.");
      } 
      Assert.True(object.Equals(returnVal, call.returnVal), $"Method '{method.Name}' return value mismatch.");
    }

    protected override object Invoke(MethodInfo targetMethod, object[] args)
    {
      var ret = Defaults.DefaultFor(targetMethod.ReturnType); 
      Calls.Add((targetMethod, args, ret));
      return ret;
    }
  }

  public class CreateDefaults
  {
    private Dictionary<Type, Func<object>> TypeCreator = new Dictionary<Type, Func<object>>();

    public CreateDefaults()
    {
      long nextlong = long.MinValue;
      TypeCreator[typeof(long)] = () => nextlong++;
      byte nextbyte = byte.MinValue;
      TypeCreator[typeof(byte)] = () => nextbyte++;
      int nextint = int.MinValue;
      TypeCreator[typeof(int)] = () => nextint++;
      double nextdouble = double.MinValue;
      TypeCreator[typeof(double)] = () => nextdouble++;
      TypeCreator[typeof(DateTime)] = () => DateTime.Now;
      TypeCreator[typeof(string)] = () => $"string: {nextint++}";
      TypeCreator[typeof(Task)] = ()=> Task.FromResult(1);
      TypeCreator[typeof(bool)] = () => true;
    }

    public void Register<T>(Func<T> func) => TypeCreator[typeof(T)] = ()=>func();

    public object[] DefaultFor(IEnumerable<ParameterInfo> parameters) =>
      DefaultFor(parameters.Select(i=>i.ParameterType));
    public object[] DefaultFor(IEnumerable<Type> types) => types.Select(DefaultFor).ToArray();

    public object DefaultFor(Type type) =>
      type switch
      {var t when t.FullName.Equals("System.Void", StringComparison.Ordinal) => null,
        var t when TypeCreator.TryGetValue(t, out var func) => func(),
        var t when t.IsEnum => Enum.ToObject(t, DefaultFor(typeof(int))),
        var t when IsGenericTask(t) => CreateDefaultCompletedTask(t.GenericTypeArguments[0]),
        var t when IsGeericEnumerable(t) => CreateArray(t.GenericTypeArguments[0]),
        var t when t.IsInterface => CreateMock(t),
        var t when t == typeof(ValueTask) => new ValueTask(),
        _ => throw new InvalidOperationException($"Could not create type {type.Name}")
      };

    private object CreateMock(Type interfaceType) =>
      typeof(Mock)
        .GetMethods().Where(i=>i.Name.Equals("Of", StringComparison.Ordinal))
        .First(i => i.GetParameters().Length == 0)
        .MakeGenericMethod(interfaceType)
        .Invoke(null, new object[0]);
    
  private bool IsGeericEnumerable(Type type)
  {
    return type.IsConstructedGenericType && GenericLists.Contains(type.GetGenericTypeDefinition());
  }

  private object CreateArray(Type basisType) =>
    Activator.CreateInstance(basisType.MakeArrayType(), new object[]{0});
  private object CreateList(Type basisType) =>
    Activator.CreateInstance(typeof(List<>).MakeGenericType(basisType));


  private static readonly Type[] GenericLists =
  {
    typeof(IEnumerable<>), typeof(ICollection<>), typeof(IList<>),
     typeof(Collection<>), typeof(List<>)
  };

    private object CreateDefaultCompletedTask(Type t)
    {
      return typeof(Task).GetMethod("FromResult").MakeGenericMethod(t).Invoke(null, 
        new []{DefaultFor(t)});
    }


    private static bool IsGenericTask(Type t) => 
      t.IsConstructedGenericType && t.GetGenericTypeDefinition()==typeof(Task<>);
  }
}