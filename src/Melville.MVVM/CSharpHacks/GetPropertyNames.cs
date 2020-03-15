using  System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Melville.MVVM.CSharpHacks
{
  public sealed class GetPropertyNames : ExpressionVisitor
  {
    private GetPropertyNames()
    {
    }

    private List<string> fieldNames = new List<string>();

    protected override Expression VisitMember(MemberExpression node)
    {
      if (node.Member is PropertyInfo)
      {
        var memberName = node.Member.Name;
        if (!fieldNames.Contains(memberName))
        {
          fieldNames.Add(memberName);
        }
      }

      return base.VisitMember(node);
    }

    public static IList<string> FromExpression(Expression e)
    {
      var visitor = new GetPropertyNames();
      visitor.Visit(e);
      return visitor.fieldNames;
    }
  }
}