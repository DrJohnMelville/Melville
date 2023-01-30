﻿using System.Linq;
using Melville.Generators.INPC.GenerationTools.AstUtilities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Melville.Generators.INPC.ProductionGenerators.DelegateToGen;

public readonly struct DelegationRequestParser
{
    private readonly bool useExplicit;

    public DelegationRequestParser(bool useExplicit)
    {
        this.useExplicit = useExplicit;
    }

    public IDelegatedMethodGenerator ParseFromMethod(IMethodSymbol symbol, SyntaxNode location) =>
        IsValidDelegatingMethod(symbol)
            ? Create(symbol.ReturnType, $"this.{symbol.Name}()", symbol)
            : new InvalidTargetMethodGenerator(location);

    private static bool IsValidDelegatingMethod(IMethodSymbol symbol) => 
        symbol.Parameters.Length == 0 && !symbol.ReturnsVoid;
    
    public IDelegatedMethodGenerator ParseFromProperty(IPropertySymbol ps) =>
        Create(ps.Type, $"this.{ps.Name}", ps);

    public IDelegatedMethodGenerator ParseFromField(IFieldSymbol symbol) => 
        Create(symbol.Type, $"this.{symbol.Name}", symbol);

    private IDelegatedMethodGenerator Create(
        ITypeSymbol typeToImplement, string methodPrefix, ISymbol targetSymbol) =>
        (typeToImplement.TypeKind, useExplicit, IsMixIn(targetSymbol.ContainingType, typeToImplement)) switch
        {
            (TypeKind.Class or TypeKind.Struct,false,true) => 
                new MixinClassGenerator(typeToImplement, methodPrefix, targetSymbol),
            (TypeKind.Interface,false,true) => 
                new InterfaceMixinGenerator(typeToImplement, methodPrefix, targetSymbol),
            (TypeKind.Interface, true, _) =>
                new ExplicitMethodGenerator(typeToImplement, methodPrefix,
                    typeToImplement.FullyQualifiedName() + ".", targetSymbol),
            (TypeKind.Interface, _, _) => new InterfaceMethodGenerator(typeToImplement, methodPrefix, targetSymbol),
            (TypeKind.Class, _, _) => new BaseClassMethodGenerator(typeToImplement, methodPrefix, targetSymbol),
            _ => new InvalidParentMethodGenerator(typeToImplement,
                targetSymbol.DeclaringSyntaxReferences.First().GetSyntax())
        };

    private bool IsMixIn(INamedTypeSymbol typeHostingMembers, ITypeSymbol typeToImplement) =>
        !typeHostingMembers.Interfaces
            .Concat(typeHostingMembers.AllBases())
            .Any(
                i => SymbolEqualityComparer.Default.Equals(i, typeToImplement));
} 