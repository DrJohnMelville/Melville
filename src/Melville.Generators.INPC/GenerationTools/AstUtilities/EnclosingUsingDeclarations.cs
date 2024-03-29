﻿using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Melville.Generators.INPC.GenerationTools.AstUtilities;

public static class EnclosingUsingDeclarations
{
    public static IEnumerable<UsingDirectiveSyntax> UsingDeclarationsInScope(this SyntaxNode context) =>
        context.UsingDeclarationListsForSurroundingScopes().SelectMany(i => i);
        
    public static IEnumerable<SyntaxList<UsingDirectiveSyntax>> UsingDeclarationListsForSurroundingScopes(
        this SyntaxNode context)
    { 
        var currentNode = context.Parent;
        while (currentNode != null)
        {
            switch (currentNode)
            {
                case NamespaceDeclarationSyntax nds:
                    yield return nds.Usings;
                    break;
                case CompilationUnitSyntax cus:
                    yield return cus.Usings;
                    break;
            }

            currentNode = currentNode.Parent;
        }
    }
        
}