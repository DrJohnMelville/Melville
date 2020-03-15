using  System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Melville.MVVM.Functional;

namespace Melville.MVVM.CSharpHacks
{
  public static class ReflectionHelper
  {
    #region MemberInfoSelectors

    private static IEnumerable<Type> AllBaseClasses(object target) =>
      FunctionalMethods.Sequence<Type?>(target.GetType(), i => i?.BaseType).OfType<Type>();

    private const BindingFlags DefaultBindingFlags =
      BindingFlags.Public | BindingFlags.Instance;

    private static BindingFlags SelectFlags(bool publicOnly) =>
      publicOnly ? DefaultBindingFlags : DefaultBindingFlags | BindingFlags.NonPublic;

    private static FieldInfo? Field(object target, string name, bool publicOnly = false)
    {
      return AllBaseClasses(target)
        .Select(i => i.GetField(name,  SelectFlags(publicOnly)| BindingFlags.GetField))
        .LastOrDefault(i => i != null);
    }

    private static PropertyInfo? Property(object target, string name, bool publicOnly = false)
    {
      return AllBaseClasses(target)
        .Select(i => i.GetProperty(name, SelectFlags(publicOnly) | BindingFlags.GetProperty))
        .LastOrDefault(i => i != null);
    }

    private static MethodInfo? Method(object target, string name, Type[] types, bool publicOnly = false)
    {
      return target.GetType()
        .GetMethod(name, SelectFlags(publicOnly) | BindingFlags.InvokeMethod, null, 
          CallingConventions.Any, types, null);
    }

    public static IEnumerable<MethodInfo> Methods(object target, string name, bool publicOnly = false)
    {
      var methodInfos = target.GetType().GetMethods(SelectFlags(publicOnly) | BindingFlags.InvokeMethod);
      return methodInfos.Where(i => i.Name.Equals(name, StringComparison.Ordinal));
    }
    #endregion

    #region Getters and Setters

    

    
    public static void SetField(this object target, string property, object value) => 
      Field(target, property)?.SetValue(target, value);

    public static object? GetField(this object target, string name) => 
      Field(target, name)?.GetValue(target);

    public static object? GetProperty(this object target, string name) => 
      Property(target, name)?.GetValue(target);

    public static void SetProperty(this object target, string property, object value) => 
      Property(target, property)?.SetValue(target, value);

    public static object? Call(this object target, string methodName, params object[] paramenters) => 
      Method(target, methodName, paramenters.Select(i => i.GetType()).ToArray())?.Invoke(target, paramenters);
    #endregion

    #region PathTraversal

    public static object? GetPath(this object source, string path, bool publicOnly = false) => 
      FollowPathComponents(source, SplitPathString(path), publicOnly);

    public static string[] SplitPathString(string path) => 
      path.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);

    public static object? FollowPathComponents(this object source, IEnumerable<string> components, 
      bool publicOnly = false)
    {
      object? ret = source;
      foreach (var field in components)
      {
        if (ret == null) return null;
        ret = Property(ret, field, publicOnly)?.GetValue(ret);
      }
      return ret;
    }


    #endregion

  }
}
