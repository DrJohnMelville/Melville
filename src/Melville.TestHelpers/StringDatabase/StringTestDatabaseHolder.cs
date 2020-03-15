using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Melville.TestHelpers.StringDatabase
{
  public sealed class StringTestDatabaseHolder: IDisposable
  {
    private IDictionary<string, string> data;
    private string dbFile;
    private bool dirty;

    public StringTestDatabaseHolder(string sourceFileName)
    {
      dbFile = sourceFileName + ".txt";
      data = File.Exists(dbFile)
        ? StringDictionarySerializer.Deserialize(File.ReadAllText(dbFile))
        : new Dictionary<string, string>();
    }
    
    public void Dispose()
    {
      if (!dirty) return;
      dirty = false;
      File.WriteAllText(dbFile, StringDictionarySerializer.Serialize(data));
    }

    public string LookupResult(string key) => data.TryGetValue(key, out var ret) ? ret : "<No value defined.>";

    public void WriteResult(string key, string value)
    {
      dirty = true;
      data[key] = value;
    }

  }
  
  internal static class StringDictionarySerializer
  {
    public static string Serialize(IDictionary<string, string> dictionary) => string.Join(Environment.NewLine,
      dictionary.Select(i => $"{Escape(i.Key)}|{Escape(i.Value)}"));

    private static string Escape(string input)
    {
      return input.Replace("\\", "\\s").Replace("|", "\\b").Replace("\r", "\\r").Replace("\n", "\\n");
    }

    public static IDictionary<string, string> Deserialize(string serialized)
    {
      var ret = new Dictionary<string, string>();
      foreach (Match? match in Regex.Matches(serialized, @"^(.+)\|([^\r\n]*)[\r\n]*$", RegexOptions.Multiline))
      {
        ret.Add(Unescape(match.Groups[1].Value), Unescape(match.Groups[2].Value));
      }
      return ret;
    }

    private static string Unescape(string value)
    {
      return value.Replace("\\n", "\n").Replace("\\r", "\r").Replace("\\b", "|").Replace("\\s", "\\");
    }
  }

}