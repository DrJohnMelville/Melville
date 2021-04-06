using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Melville.Hacks
{
  public static class GetPropertyNames
  {
    public static IEnumerable<string> FromExpression(Expression e)
    {
      var ret = new HashSet<String>();
      new PropertyNameVisitor(ret).Visit(e);
      return ret;
    }
    
    private sealed class PropertyNameVisitor : ExpressionVisitor
    {
      private ICollection<string> fieldNames;

      public PropertyNameVisitor(ICollection<string> fieldNames)
      {
        this.fieldNames = fieldNames;
      }

      protected override Expression VisitMember(MemberExpression node)
      {
        if (node.Member is PropertyInfo)
        {
          fieldNames.Add(node.Member.Name);
        }
        return base.VisitMember(node);
      }
    }
  }
}