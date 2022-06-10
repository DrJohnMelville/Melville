using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Melville.Generators.INPC.ProductionGenerators.INPC.CodeGen;

public class PropertyDependencyChecker
{
    private readonly Dictionary<string, HashSet<string>> mappings = new();

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
        foreach (var invocation in node.DescendantNodes().Where(
                     i=> i is IdentifierNameSyntax ))
        {
            if (!ValidInvocation(invocation)) continue;
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
        if (mappings.TryGetValue(invocation, out var item)) return item;
        var ret = new HashSet<string>();
        mappings.Add(invocation, ret);
        return ret;
    }

    public IEnumerable<string> AllDependantProperties(string initialProperty)
    {
        var candidates = new Stack<string>();
        var outputItems = new HashSet<string>();
        candidates.Push(initialProperty);
        while (candidates.Any())
        {
            var item = candidates.Pop();
            if (outputItems.Contains(item)) continue;
            outputItems.Add(item);
            yield return item;
            if (mappings.TryGetValue(item, out var mappedProperties))
            {
                foreach (var prop in mappedProperties)
                {
                    candidates.Push(prop);
                }
            }
        }
    }
}