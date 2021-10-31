using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Melville.Hacks;

public static class ExpressionExtensions
{
  public static string ExtractMemberName(this Expression body) =>
    ExtractMemberInfo(body)?.Name??"";

  public static MemberInfo ExtractMemberInfo(this Expression body) =>
    body switch
    {
      UnaryExpression ue => ExtractMemberInfo(ue.Operand),
      LambdaExpression l => ExtractMemberInfo(l.Body),
      MemberExpression m => m.Member,
      MethodCallExpression c => c.Method,
      _ => throw new InvalidOperationException("Cannot extract member name.")
    };
}