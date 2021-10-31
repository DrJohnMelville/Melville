#nullable disable warnings
using  System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Melville.FileSystem;
using Melville.Hacks;
using Xunit;

namespace Melville.Mvvm.TestHelpers.MockFiles;

public static class FileSystemHelperObjects
{
  public static Stream OpenReadSync(this IFile file)
  {
    return AsyncPump.Run<Stream>(() => file.OpenRead());
  }
  public static Stream CreateWriteSync(this IFile file)
  {
    return AsyncPump.Run<Stream>(() => file.CreateWrite());
  }

  public static void Create(this IFile file, string content)
  {
    using (var stream = file.CreateWriteSync())
    {
      AsyncPump.Run(() => stream.WriteStringAsync(content, Encoding.UTF8));
    }
  }

  public static void AssertContent(this IFile file, string content)
  {
    using (var stream = file.OpenReadSync())
    {
      stream.AssertContent(content);
    }
  }

  public static string Content(this IFile file)
  {
    using (var s = file.OpenReadSync())
    using (var r = new StreamReader(s))
    {
      return AsyncPump.Run(() => r.ReadToEndAsync());
    }
  }
  public static void AssertContentMatches(this IFile file, string content)
  {
    using (var stream = file.OpenReadSync())
    {
      stream.AssertContentMatches(content);
    }
  }

  public static void AssertContent(this Stream s, string content)
  {
    var s2 = new MemoryStream((int)s.Length);
    AsyncPump.Run(() => s.CopyToAsync(s2));
    s2.Seek(0, SeekOrigin.Begin);
    using (var reader = new StreamReader(s2))
    {
      Assert.Equal(content, reader.ReadToEnd());
    }
  }
  public static void AssertContentMatches(this Stream s, string content)
  {
    var s2 = new MemoryStream((int)s.Length);
    AsyncPump.Run(() => s.CopyToAsync(s2));
    s2.Seek(0, SeekOrigin.Begin);
    using (var reader = new StreamReader(s2))
    {
      var actualContent = reader.ReadToEnd();
      var regex = new Regex($"^{RegexExtensions.GlobToRexex(content)}$", RegexOptions.IgnoreCase);
      Assert.True(regex.IsMatch(actualContent),
        string.Format("\r\n{0} and \r\n{1} do not match.", actualContent, content));
    }
  }
}