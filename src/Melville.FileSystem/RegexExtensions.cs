using System.Text.RegularExpressions;

namespace Melville.FileSystem;

public class RegexExtensions
{
  public static string GlobToRegex(string glob) => 
      $"{Regex.Escape(glob)
          .Replace(@"\*", ".*")
          .Replace(@"\?", ".")}";

}