using System;
using System.Linq.Expressions;
using System.Text.RegularExpressions;

namespace Melville.Linq.Statistics.Functional
{
  public sealed class ExpressionPrinter
  {
    public static string StripObjectName(object o)
    {
      var str = o.ToString();
      return str.Substring(str.IndexOf(".") + 1);
    }
    public static string Print<TSource,TDest>(Expression<Func<TSource, TDest>> func)
    {
      var str = InnerStringOnjectName(func);
      var match = Regex.Match(str, @"^\((.+)\)$");
      return match.Success?match.Groups[1].Value:str;
    }

    private static string InnerStringOnjectName<TSource, TDest>(Expression<Func<TSource, TDest>> func)
    {
      Expression working = func.Body;
      while (true)
      {
        switch (working)
        {
          case MethodCallExpression mce:
            return StripObjectName(mce);
          case MemberExpression mem:
            return StripObjectName(mem);
          case UnaryExpression uex:
            if (uex.NodeType == ExpressionType.Convert || uex.NodeType == ExpressionType.ConvertChecked)
            {
              working = uex.Operand;
            }
            else
            {
              goto default;
            }
            break;
          case BinaryExpression bex:
            if (bex.NodeType == ExpressionType.Coalesce)
            {
              working = bex.Left;
            }
            else
            {
              goto default;
            }
            break;
          default: return working.ToString();
        }
      }
    }
  }
}