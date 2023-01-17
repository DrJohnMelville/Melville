using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Melville.Generators.INPC.GenerationTools.AstUtilities;
using Melville.Generators.INPC.GenerationTools.CodeWriters;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Melville.Generators.INPC.ProductionGenerators.Constructors;

internal readonly struct WriteConstructorsForSymbol
{
    private readonly ITypeSymbol classToGenerate;
    private readonly SourceProductionCodeWriter writer;

    public WriteConstructorsForSymbol(ITypeSymbol classToGenerate, SourceProductionCodeWriter writer)
    {
        this.classToGenerate = classToGenerate;
        this.writer = writer;
    }

    public void Genarate()
    {
        var fieldsToGenerate = ComputeFieldsToGenerate(classToGenerate);
        new ConstructorsCodeGenerator(classToGenerate.Name, fieldsToGenerate, ConstructorsFor(classToGenerate.BaseType))
            .GenerateCode(writer);
    }

    private static List<MemberData> ComputeFieldsToGenerate(ITypeSymbol target) =>
        target.MembersLabeledWith(ConstructorGenerator.AttributeName)
            .Select(CreateMemberData)
            .OfType<MemberData>()
            .ToList();

    private static MemberData? CreateMemberData(ISymbol i) =>
        i switch
        {
            IFieldSymbol field => new MemberData(field.Type.FullyQualifiedName(), field.Name),
            IPropertySymbol prop => new MemberData(prop.Type.FullyQualifiedName(), prop.Name),
            _=> null
        };

    private static IList<MemberData[]> ConstructorsFor(INamedTypeSymbol? classSymbol)
    {
        if (classSymbol is null) return Array.Empty<MemberData[]>();
        return ConstructorsExplicitlyDefinedOn(classSymbol)
            .Concat(ConstructorsThatWillBeSynthesizedForClass(classSymbol))
            .DefaultIfEmpty(Array.Empty<MemberData>())
            .ToList();
    }

    private static IEnumerable<MemberData[]> ConstructorsExplicitlyDefinedOn(INamedTypeSymbol classSymbol) =>
        classSymbol.InstanceConstructors
        .Where(i=>!i.IsImplicitlyDeclared)
        .Select(
            i => i.Parameters.Select(j => new MemberData(j.Type.FullyQualifiedName(), j.Name))
                .ToArray());

    private static IEnumerable<MemberData[]> ConstructorsThatWillBeSynthesizedForClass(INamedTypeSymbol classSymbol)
    {
        if (!GeneratorWillAddConstructorsFor(classSymbol)) return Array.Empty<MemberData[]>();
        var localFields = ComputeFieldsToGenerate(classSymbol);
        return ConstructorsFor(classSymbol.BaseType)
            .DefaultIfEmpty(Array.Empty<MemberData>())
            .Select(i => i.Concat(localFields).ToArray())
            .Where(i => i.Length > 0);
    }

    private static bool GeneratorWillAddConstructorsFor(INamedTypeSymbol classSymbol) =>
        classSymbol.GetAttributes().Concat(
            classSymbol.GetMembers()
                .Where(i=> i is IFieldSymbol or IPropertySymbol)
                .SelectMany(i => i.GetAttributes())
        ).FilterToAttributeType(ConstructorGenerator.AttributeName).Any();
}