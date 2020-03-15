using  System.Text.RegularExpressions;

namespace Melville.MVVM.CSharpHacks
{
  public class RegexExtensions
  {
    public static string GlobToRexex(string glob) => $"{Regex.Escape(glob).Replace(@"\*", ".*").Replace(@"\?", ".")}";

  }
}