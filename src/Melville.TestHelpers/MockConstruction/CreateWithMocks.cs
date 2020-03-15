using  System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using Moq;

namespace Melville.TestHelpers.MockConstruction
{
  /// <summary>
  /// Create a business object using mocks for unspecified parameters.
  /// </summary>
  public static class CreateWithMocks
  {
    /// <summary>
    /// Construct an object, using the most numerous parameter constructor, substituting any interfaces
    /// with filled out mocks which recursively fill out.
    /// </summary>
    /// <typeparam name="T">The type to construct</typeparam>
    /// <param name="parameters">Optional group of literals you wish to pass as parameters</param>
    /// <returns>A CreateWithMockImpl representing the constructed type</returns>
    public static CreateMockImpl<T> Construct<T>(params object[] parameters) => new ConstructWithMocks<T>(parameters);

    /// <summary>
    /// Construct a filled out mock where all getters and methods that return interfaces return mocks of those
    /// interfaces
    /// </summary>
    /// <typeparam name="T">Tyype of mock needed</typeparam>
    /// <param name="parameters">Literal parameters for the mock/</param>
    /// <returns>A CreatWithMockImpl which returns the proper </returns>
    public static CreateMockImpl<Mock<T>> Mock<T>(params object[] parameters) where T : class =>
      new DeepMockInterface<T>(parameters);
    public static object Concrete<T>() => new DeclareLiteral<T>();


    private sealed class DeepMockInterface<TResult> : CreateMockImpl<Mock<TResult>> where TResult : class
    {
      public DeepMockInterface(object[] parameters) : base(parameters)
      {
      }

      public override Mock<TResult> Result() => (Mock<TResult>)CreateDeepMock(typeof(TResult));

    }

    private interface IDeclareLiteral
    {
      object Create(IAbstractConstructor impl);
    }

    private sealed class DeclareLiteral<T> : IDeclareLiteral
    {
      public object Create(IAbstractConstructor impl) => impl.CreateConcreteFactory<T>()()!;
    }

    private interface IAbstractConstructor
    {
      Func<T> CreateConcreteFactory<T>();
    }

    private sealed class ConstructWithMocks<TResult> : CreateMockImpl<TResult>
    {
      private readonly Func<TResult> factory;

      public ConstructWithMocks(object[] parameters) : base(parameters)
      {
        factory = CreateConcreteFactory<TResult>();
      }

      public override TResult Result() => factory();

    }

    public abstract class CreateMockImpl<TResult> : IAbstractConstructor
    {
      private readonly List<object?> values = new List<object?>();
      private readonly List<Mock> mocks = new List<Mock>();

      protected CreateMockImpl(object[] parameters)
      {
        foreach (var param in parameters)
        {
          switch (param)
          {
            case null: return;
            case Mock m:
              mocks.Add(m);
              break;
            case IDeclareLiteral literal:
              values.Add(literal.Create(this));
              break;
            default:
              values.Add(param);
              break;
          }
        }
      }

      public abstract TResult Result();

      public CreateMockImpl<TResult> Result(out TResult result)
      {
        result = Result();
        return this;
      }

      public CreateMockImpl<TResult> InputValue<T>(out T obj)
      {
        obj = InputValue<T>(false);
        return this;
      }

      public T InputValue<T>(bool tryCreate = false) => (T)InputValue(typeof(T), tryCreate)!;

      protected object? InputValue(Type targetType, bool tryCreate)
      {
        var candidates = CurrentCandidatesForType(targetType).ToList();
        switch (candidates.Count)
        {
          case 0 when tryCreate: return CreateValue(targetType);
          case 0: throw new InvalidOperationException($"No value of type {targetType.Name} found.");
          default: return candidates[0];
        }
      }

      private IEnumerable<object?> CurrentCandidatesForType(Type targetType)
      {
        return values.Concat(mocks.Select(i => i.Object)).Where(targetType.IsInstanceOfType);
      }

      private object? CreateValue(Type targetType)
      {
        if (targetType == typeof(void)) return null;
        object? literal = TryCreateLiteral(targetType);
        if (literal != null) return literal;

        if (targetType.IsInterface)
        {
          var mock = CreateDeepMock(targetType);
          return mock?.Object;
        }
        var ret = targetType.IsValueType ? Activator.CreateInstance(targetType) : null;
        values.Add(ret);
        return ret;
      }

      private static readonly HashSet<Type> LiteralTypes= new HashSet<Type>()
      {
        typeof(ArrayList),
      };
      private static readonly HashSet<Type> GenericCreators= new HashSet<Type>()
      {
        typeof(List<>),
        typeof(Collection<>),
        typeof(Stack<>),
        typeof(HashSet<>),
        typeof(Array),
        typeof(Dictionary<,>),
         typeof(SortedDictionary<,>),
      };
      private static readonly Dictionary<Type, Type> ReplacementCreators = new Dictionary<Type, Type>()
      {
        {typeof(IList<>), typeof(List<>)},
        {typeof(IEnumerable<>), typeof(List<>)},
        {typeof(ISet<>), typeof(HashSet<>)},
        {typeof(ICollection<>), typeof(Collection<>)},
        {typeof(IDictionary<,>), typeof(Dictionary<,>)},
      };
      private object? TryCreateLiteral(Type targetType)
      {
        if (targetType.IsValueType) return null;

        if (LiteralTypes.Contains(targetType))
        {
          return CreateObject(targetType);
        }

        if (targetType.IsGenericType && GenericCreators.Contains(targetType.GetGenericTypeDefinition()))
        {
          return CreateObject(targetType);
        }

        if (targetType.IsGenericType && ReplacementCreators.TryGetValue(targetType.GetGenericTypeDefinition(), out var creationType))
        {
          return CreateObject(creationType.MakeGenericType(targetType.GenericTypeArguments));
        }

        return null;
      }

      private static object CreateObject(Type targetType)
      {
        return targetType.GetConstructor(new Type[0]).Invoke(new object[0]);
      }


      protected Mock CreateDeepMock(Type targetType)
      {
          var mock = (Mock)Activator.CreateInstance(typeof(Mock<>).MakeGenericType(targetType));
          mocks.Add(mock);
          FillMock(mock, targetType);
          return mock;
      }

      private void FillMock(Mock mock, Type targetType)
      {
        foreach (var property in MockableProperties(targetType))
        {
          MockExtension.SetupGet(targetType, mock, property.PropertyType, property.Name, InputValue(property.PropertyType, true));
        }

        foreach (var method in MockableMethods(targetType))
        {
          MockExtension.SetupMethod(mock, targetType, method, InputValue(method.ReturnType, true));
        }
      }

      private IEnumerable<PropertyInfo> MockableProperties(Type targetType) =>
        targetType.GetProperties().Where(i => i.CanRead && ShouldFillInterfaceMember(i.PropertyType));
      private IEnumerable<MethodInfo> MockableMethods(Type targetType) =>
        targetType.GetMethods().Where(i => (!i.ContainsGenericParameters) && ShouldFillInterfaceMember(i.ReturnType));

      private bool ShouldFillInterfaceMember(Type t) => CanMockType(t) || AlreadyHaveTypeCandidate(t) || CreateValue(t) != null;
      private bool AlreadyHaveTypeCandidate(Type t) => CurrentCandidatesForType(t).Any();
      private static bool CanMockType(Type t) => t.IsInterface && !t.IsGenericTypeDefinition;

      public Mock<T> Mock<T>() where T : class => mocks.OfType<Mock<T>>().First();
      public CreateMockImpl<TResult> Mock<T>(out Mock<T> mock) where T : class
      {
        mock = Mock<T>();
        return this;
      }

      public Func<T> CreateConcreteFactory<T>()
      {
        var constructor = typeof(T).GetConstructors().OrderByDescending(i => i.GetParameters().Length).First();
        var constructorParams = constructor.GetParameters().Select(i => InputValue(i.ParameterType, true)).ToArray();
        return () => (T)constructor.Invoke(constructorParams);
      }

    }
  }
}
