using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices.ProductionGenerators.Constructors;
using Melville.Generators.INPC.GenerationTools.AstUtilities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Melville.Generators.INPC.ProductionGenerators.Constructors;

public readonly struct ConstructorCodeGeneratorFactory
{
    private readonly INamedTypeSymbol rootSymbol;
    private readonly IList<MemberData[]> constructors = new List<MemberData[]>();

    public ConstructorCodeGeneratorFactory(INamedTypeSymbol rootSymbol)
    {
        this.rootSymbol = rootSymbol;
        FindParentConstructors(rootSymbol);        
    }
    public ConstructorsCodeGenerator Create(TypeDeclarationSyntax syntax)
    {
        return new(syntax, ImplicitlyConstructedMembers(rootSymbol), constructors);
    }

    private void FindParentConstructors(INamedTypeSymbol target) => 
        FindDeclaredConstructors(target.BaseType, Array.Empty<MemberData>());

    private IEnumerable<MemberData[]> ReadConstructors(INamedTypeSymbol symbol, bool excludeDefalutCons) =>
        symbol.InstanceConstructors
            .Where(i => !excludeDefalutCons || i.Parameters.Length > 0)
            .Select(static i => i.Parameters.Select(j=>new MemberData(j.Type.FullyQualifiedName(), j.Name)).ToArray());
    
    private void FindDeclaredConstructors(INamedTypeSymbol? baseType, MemberData[] inheritedParams)
    {
        if (baseType == null)
        {
            constructors.Add(inheritedParams);
            return;
        }
        var implicitVars = ImplicitlyConstructedMembers(baseType).ToList();
        foreach (var constructor in ReadConstructors(baseType, implicitVars.Count > 0))
        {
            constructors.Add(constructor.Concat(inheritedParams).ToArray());
        }

        if (implicitVars.Count > 0 || HasFromConstructorAttribute(baseType))
        {
            FindDeclaredConstructors(baseType.BaseType, implicitVars.Concat(inheritedParams).ToArray());
        }
    }
    
    private MemberData[] ImplicitlyConstructedMembers(INamedTypeSymbol baseType) =>
        baseType
            .GetMembers()
            .Where(ShouldGenerateAssignment)
            .Select(MemberDataForSymbol)
            .ToArray();

    private MemberData MemberDataForSymbol(ISymbol arg) => arg switch
    {
        IFieldSymbol fs => new MemberData(fs.Type.FullyQualifiedName(), fs.Name),
        IPropertySymbol ps => new MemberData(ps.Type.FullyQualifiedName(), ps.Name),
        _ => throw new InvalidDataException("Only fields and properties should be attributed with [FromConstructor]")
    };

    private bool ShouldGenerateAssignment(ISymbol i) => IsSettableMemberKind(i) && HasFromConstructorAttribute(i);

    private static bool IsSettableMemberKind(ISymbol i) => i is IFieldSymbol or IPropertySymbol;

    private static bool HasFromConstructorAttribute(ISymbol i) =>
        i.GetAttributes().Any(IsFromConstructorAttribute);

    private static bool IsFromConstructorAttribute(AttributeData j) => 
        j.AttributeClass?.Name.Equals("FromConstructorAttribute") ?? false;
}