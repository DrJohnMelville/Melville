using System.Text.RegularExpressions;

namespace Melville.FileSystem
{
  public class RegexExtensions
  {
    public static string GlobToRexex(string glob) => $"{Regex.Escape(glob).Replace(@"\*", ".*").Replace(@"\?", ".")}";

  }
}