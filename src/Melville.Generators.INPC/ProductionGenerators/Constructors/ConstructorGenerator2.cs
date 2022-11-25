#if false
#else
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Melville.Generators.INPC.GenerationTools.AstUtilities;
using Melville.Generators.INPC.GenerationTools.CodeWriters;
using Microsoft.CodeAnalysis;

namespace System.Runtime.CompilerServices.ProductionGenerators.Constructors;

internal readonly struct ConstructorGenerator2
{
    private readonly ITypeSymbol classToGenerate;
    private readonly SourceProductionCodeWriter writer;

    public ConstructorGenerator2(ITypeSymbol classToGenerate, SourceProductionCodeWriter writer)
    {
        this.classToGenerate = classToGenerate;
        this.writer = writer;
    }

    public void Genarate()
    {
        var fieldsToGenerate = ComputeFieldsToGenerate(classToGenerate);
        new ConstructorsCodeGenerator2(classToGenerate.Name, fieldsToGenerate, ConstructorsFor(classToGenerate.BaseType))
            .GenerateCode(writer);
    }

    private static List<MemberData> ComputeFieldsToGenerate(ITypeSymbol target) =>
        target.MembersLabeledWith(ConstructorGenerator.AttributeName)
            .Select(CreateMemberData)
            .ToList();

    private static MemberData CreateMemberData(ISymbol i) =>
        i switch
        {
            IFieldSymbol field => new MemberData(field.Type.FullyQualifiedName(), field.Name),
            IPropertySymbol prop => new MemberData(prop.Type.FullyQualifiedName(), prop.Name),
            _=> throw new InvalidDataException("Can only put FromConstructor on fields or properties")
        };

    private static IList<MemberData[]> ConstructorsFor(INamedTypeSymbol? classSymbol)
    {
        if (classSymbol is null) return Array.Empty<MemberData[]>();
        var localFields = ComputeFieldsToGenerate(classSymbol);
        var toBeGenerated = ConstructorsFor(classSymbol.BaseType)
            .DefaultIfEmpty(Array.Empty<MemberData>())
            .Select(i => i.Concat(localFields).ToArray())
            .Where(i=>i.Length > 0);
        return classSymbol.InstanceConstructors.Select(
            i => i.Parameters.Select(j => new MemberData(j.Type.FullyQualifiedName(), j.Name))
                .ToArray())
            .Concat(toBeGenerated)
            .ToList();
    }
}
#endif