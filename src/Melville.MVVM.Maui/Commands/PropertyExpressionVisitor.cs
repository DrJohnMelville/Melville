using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Melville.MVVM.Maui.Commands;

internal class PropertyExpressionVisitor(Type parentType): ExpressionVisitor
{
    public static HashSet<string> ScanForNames(Expression e, Type sourceType)
    {
        var visitor = new PropertyExpressionVisitor(sourceType);
        visitor.Visit(e);
        return visitor.Properties;
    }

    public HashSet<string> Properties { get; } = new();
    protected override Expression VisitMember(MemberExpression node)
    {
        if (node.Expression?.Type == parentType)
            Properties.Add(node.Member.Name);
        return base.VisitMember(node);
    }
}