using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Melville.Linq.Statistics.FileWriter
{
  public static class ObjectTableFormatter
  {
    public static IEnumerable<IList> Dump<T>(IEnumerable<T> objects)
    {
      var fields = typeof(T).GetProperties().ToList();
      yield return fields.Select(i => UnPascalCase(i.Name)).ToList();
      foreach (var row in objects)
      {
        yield return fields.Select(i => ProcessField(i.GetMethod.Invoke(row, null))).ToList();
      }
    }

    private static object ProcessField(object field)
    {
      switch (field)
      {
        case int Int:
          return Int;
        case double d:
          return d;
        case DateTime d:
          return d.ToShortDateString();
        case null:
          return null;
        default:
          return ProcessString(field.ToString());
      }
    }

    private static object ProcessString(string field)
    {
      if (int.TryParse(field, out var intValue)) return intValue;
      if (double.TryParse(field, out var doubleValue)) return doubleValue;
      return field;
    }

    private static string UnPascalCase(string input) =>
      Regex.Replace(input, "([a-z])([A-Z])", "$1 $2");
  }
}