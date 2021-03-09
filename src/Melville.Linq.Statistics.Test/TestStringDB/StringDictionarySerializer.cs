using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Xunit;

namespace Test.TestStringDB
{
  /// <summary>
  /// Memory for unit tests
  /// </summary>
  public abstract class StringTestDatabase : IDisposable
  {
    private readonly IDictionary<string, string> data;
    private readonly string dbFile;

    /// <summary>
    /// Create the database.
    /// </summary>
    /// <param name="databaseName">Leave this parameter blank.  CSharp will fill in the name of the calling file, which is used to find the database file.</param>
    protected StringTestDatabase([CallerFilePath] string databaseName = null)
    {
      dbFile = GetDatabaseFile(databaseName);
      data = File.Exists(dbFile)?StringDictionarySerializer.Deserialize(File.ReadAllText(dbFile)):
      new Dictionary<string, string>();
    }

    private static string GetDatabaseFile(string databaseName) => databaseName + ".txt";

    private string LookupResult(string key) => 
      data.TryGetValue(key, out string? ret) ? ret : "<No value defined.>";

    private bool dirty = false;

    /// <summary>
    /// Assert a result against the database, or possibly remember the result
    /// </summary>
    /// <param name="result">Result to test or remember</param>
    /// <param name="member">Usually leave this blank.  If your have more than one assert per test method, give each a unique name here</param>
    /// <param name="file">Leave blank.  C# will fill in the calling file name</param>
    public void AssertDatabase(string? result, [CallerMemberName] string? member = null,
      [CallerFilePath] string? file = null)
    {
      if (result == null) throw new InvalidOperationException("Cannot test for null in StringDictionary");
      var datumKey = $"{Path.GetFileName(file)}_{member}";
      string expected = LookupResult(datumKey);
      if (expected.Equals(result)) return; // quit early on success

      // otherwsie we failed
      if (!(CapsLockDown() && ControlDown())) // if we are not in writing mode
      {
        Assert.Equal(expected, result); // fail with nice UI showing where
        Debug.Assert(false, "Should never run because the assert sheould have failed.");
      }

      // update the database
      dirty = true;
      data[datumKey] = result;
    }

    private static bool CapsLockDown() => KeyDown(0x14);
    private static bool ControlDown() => KeyDown(0x11);
    private static bool KeyDown(int keyCode) => (NativeMethods.GetKeyState(keyCode) & 0xffff) != 0;

    private static class NativeMethods
    {
      [DllImport("user32.dll", CharSet = CharSet.Auto,
        ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
      public static extern short GetKeyState(int keyCode);
    }

    /// <summary>
    /// If we have a new value, write it out to disk before we go away.
    /// </summary>
    public void Dispose()
    {
      if (!dirty) return;
      dirty = false;
      File.WriteAllText(dbFile, StringDictionarySerializer.Serialize(data));
    }

    public static class StringDictionarySerializer
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
        foreach (Match match in Regex.Matches(serialized, @"^(.+)\|([^\r\n]*)[\r\n]*$", RegexOptions.Multiline))
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
}