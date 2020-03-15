#nullable disable warnings
using  System.Linq;
using Melville.MVVM.Https;
using Xunit;

namespace Melville.Mvvm.Test.Https
{
  public sealed class UrlFormatterTest
  {
    [Fact]
    public void KeyValuePairTest()
    {
      var kvp = UrlFormatter.ToKeyValuePair(new {a="bar", b="baz"}).ToArray();
      Assert.Equal("a", kvp[0].Key);
      Assert.Equal("bar", kvp[0].Value);
      Assert.Equal("b", kvp[1].Key);
      Assert.Equal("baz", kvp[1].Value);
    }
    [Fact]
    public void SimpleTest()
    {
      Assert.Equal("foo?a=bar&b=baz", UrlFormatter.AssembleUrl("foo", new {a="bar", b="baz"}));
    }
    [Fact]
    public void SingleTest()
    {
      Assert.Equal("foo?a=bar", UrlFormatter.AssembleUrl("foo", new {a="bar"}));
    }
    [Fact]
    public void FormattingTest()
    {
      Assert.Equal("foo?a=hello%20%3F%2B%3A%26%20world", UrlFormatter.AssembleUrl("foo", new {a="hello ?+:& world"}));
    }

    [Fact]
    public void FormatArrayTest()
    {
      Assert.Equal("foo?a=1&a=2&a=3", UrlFormatter.AssembleUrl("foo", new {a = new[] {1,2,3}}));
    }
    [Fact]
    public void NullTest()
    {
      Assert.Equal("foo", UrlFormatter.AssembleUrl("foo", null));
    }
    [Fact]
    public void Empty()
    {
      Assert.Equal("foo", UrlFormatter.AssembleUrl("foo", new {}));
    }

    [Fact]
    public void SimpleMakeFolderList()
    {
      Assert.Equal("a/b/c/d/", UrlFormatter.MakeFolderList("a","b","c","d"));
    }
    [Fact]
    public void EncodeMakeFolderList()
    {
      Assert.Equal("a/b%40%26/c/d/", UrlFormatter.MakeFolderList("a","b@&","c","d"));
    }
  }
}