using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Melville.Linq.Statistics.Tables
{
  public sealed class ObjectFormatter
  {
    private readonly Dictionary<Type, Func<object,string>> formatters =
      new Dictionary<Type, Func<object, string>>();

    public string Format(object obj)
    {
      if (obj == null) return "<null>";
      var paramType = obj.GetType();
      foreach (var pair in formatters)
      {
        if (pair.Key.IsAssignableFrom(paramType))
        {
          return pair.Value(obj);
        }
      }
      return obj.ToString();
    }

    public void AddFormatter<T>(Func<T, string> func)
    {
      formatters[typeof(T)] = (object o) => func((T) o);
    }
  }
}