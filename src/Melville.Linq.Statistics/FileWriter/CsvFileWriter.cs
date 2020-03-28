using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Melville.Linq.Statistics.Functional;

namespace Melville.Linq.Statistics.FileWriter
{
  public static class CsvFileWriter
  {
    public static void Write<T>(string fileName, IEnumerable<T> cells)
    {
      using (var stream = File.Create(fileName))
      {
        Write(stream, cells);
      }
    }

    public static void Write(string fileName, IList<IList> cells)
    {
      using (var stream = File.Create(fileName))
      {
        Write(stream, cells);
      }
    }
    public static void Write<T>(Stream s, IEnumerable<T> cells) =>
      Write(s, ObjectTableFormatter.Dump(cells).ToList());
    public static void Write(Stream s, IList<IList> cells)
    {
      using (var writer = new StreamWriter(s))
      {
        foreach (var row in cells)
        {
          writer.WriteLine(string.Join(",", row.ObjectListWithNulls().Select(ProcessField)));
        }
      }
    }

    private static readonly char[] specialChars = {',', '"', '\r', '\n'};
    private static object ProcessField(object arg)
    {
      var str = arg?.ToString() ?? string.Empty;
      return str.IndexOfAny(specialChars) < 0 ? str : $"\"{str.Replace("\"", "\"\"")}\"";
    }
  }
}