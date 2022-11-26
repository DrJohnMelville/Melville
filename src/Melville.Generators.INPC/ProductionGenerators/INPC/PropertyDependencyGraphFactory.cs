using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Melville.Generators.INPC.ProductionGenerators.INPC;

public readonly struct PropertyDependencyGraphFactory
{
    private readonly Dictionary<string, HashSet<string>> isReferencedBy = new();

    public PropertyDependencyGraphFactory()
    {
    }

    public PropertyDependencyGraph CreateFromClass(ITypeSymbol classDecl)
    {
        foreach (var member in classDecl.GetMembers())
        {
            if (member is IPropertySymbol propSymbol)
                AddProperty(propSymbol.DeclaringSyntaxReferences);
        }

        return new(isReferencedBy);
    }

    private void AddProperty(ImmutableArray<SyntaxReference> declarations)
    {
        foreach (var declaration in declarations)
        {
            if (declaration.GetSyntax() is PropertyDeclarationSyntax pds)
                AddProperty(pds);
        }
    }

    public void AddProperty(PropertyDeclarationSyntax property)
    {
        foreach (var node in property.DescendantNodes()
                     .Where(IsGetterDeclaration))
        {
            AddPropertyBody(node, property.Identifier.ToString());
        }
    }

    private bool IsGetterDeclaration(SyntaxNode i)
    {
        var ret = (i is ArrowExpressionClauseSyntax || 
                   (i is AccessorDeclarationSyntax ads && ads.Kind() == SyntaxKind.GetAccessorDeclaration));
        return ret;
    }

    private void AddPropertyBody(SyntaxNode node, string targetPropertyName)
    {
        foreach (var implementationNode in node.DescendantNodes())
        {
            if (!(implementationNode is IdentifierNameSyntax invocation && ValidInvocation(invocation))) continue;
            AddMapping(targetPropertyName, invocation.ToString());
        }
    }

    private bool ValidInvocation(SyntaxNode invocation) =>
        invocation.Parent switch
        {
            MemberAccessExpressionSyntax mae when mae.Expression == invocation => true,
            MemberAccessExpressionSyntax mae when !(
                mae.Expression.ToString().Equals("this") &&
                mae.Name == invocation) => false,
            MemberBindingExpressionSyntax _ => false,
            _=> true
        };

    private void AddMapping(string targetPropertyName, string sourcePropertyName)
    {
        var mappedItem = GetCollection(sourcePropertyName);
        mappedItem.Add(targetPropertyName);
    }

    private HashSet<string> GetCollection(string invocation)
    {
        if (isReferencedBy.TryGetValue(invocation, out var item)) return item;
        var ret = new HashSet<string>();
        isReferencedBy.Add(invocation, ret);
        return ret;
    }
}