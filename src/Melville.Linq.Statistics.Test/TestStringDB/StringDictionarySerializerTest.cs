using System.Collections.Generic;
using Xunit;

namespace Test.TestStringDB
{
  public sealed class StringDictionarySerializerTest
  {
    private void CheckSerialization(Dictionary<string, string> dictionary)
    {
      var serialized = StringTestDatabase.StringDictionarySerializer.Serialize(dictionary);
      var d2 = StringTestDatabase.StringDictionarySerializer.Deserialize(serialized);
      Assert.Equal(dictionary.Count, d2.Count);
      foreach (var entry in dictionary)
      {
        var actual = d2[entry.Key];
        Assert.Equal(entry.Value, actual);
      }
    }

    [Fact]
    public void EmptySerialize()
    {
      CheckSerialization(new Dictionary<string, string>());
    }
    [Fact]
    public void SimpleSerialize()
    {
      CheckSerialization(new Dictionary<string, string>
      {
        {"One", "1"},
        {"two", "2"}
      });
    }
    [Fact]
    public void ConmplexSerialize()
    {
      CheckSerialization(new Dictionary<string, string>
      {
        {"On|e", "1"},
        {"t\\\r\nwo", "2"}
      });
    }
  }
}