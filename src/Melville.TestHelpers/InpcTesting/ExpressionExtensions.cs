using System;
using  System.Linq.Expressions;
using System.Reflection;

namespace Melville.TestHelpers.InpcTesting
{
    public static class ExpressionExtensions
    {
        public static string GetAccessedMemberName(this Expression body) =>
          GetAccessedMemberInfo(body).Name;

        public static MemberInfo GetAccessedMemberInfo(this Expression body) =>
            body.NodeType switch
            {
                ExpressionType.Convert => GetAccessedMemberInfo(((UnaryExpression) body).Operand),
                ExpressionType.Lambda => GetAccessedMemberInfo(((LambdaExpression) body).Body),
                ExpressionType.MemberAccess => ((MemberExpression) body).Member,
                _ => throw new InvalidOperationException("Cannot find member name.")
            };
    }
}