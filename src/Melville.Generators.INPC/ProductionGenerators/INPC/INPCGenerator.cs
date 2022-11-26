using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using Melville.Generators.INPC.GenerationTools.AbstractGenerators;
using Melville.Generators.INPC.GenerationTools.AstUtilities;
using Melville.Generators.INPC.GenerationTools.CodeWriters;
using Melville.Generators.INPC.ProductionGenerators.INPC.CodeGen;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Melville.Generators.INPC.ProductionGenerators.INPC;

[Generator]

#if false
public class INPCGenerator : ClassWithLabeledMembersGenerator
{
    private static readonly SearchForAttribute attrFinder = new("Melville.INPC.AutoNotifyAttribute");

    public INPCGenerator() : base(attrFinder)
    {
    }
    protected override ILabeledMembersSyntaxModel CreateMemberRecord(TypeDeclarationSyntax targetTypeDecl) => 
        new InpcSyntaxModel(targetTypeDecl);
}
#else
public class INPCGenerator: IIncrementalGenerator
{
    private const string attributeFullName = "Melville.INPC.AutoNotifyAttribute";
    
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterSourceOutput(
            context.SyntaxProvider.ForAttributeWithMetadataName(attributeFullName,
                (i,_)=> i is TypeDeclarationSyntax or VariableDeclaratorSyntax,
                (i,_)=> i)
                .Collect()
                .SelectMany((i,_) => i.GroupBy(j=>j.TargetSymbol.ThisOrContainingTypeSymbol()))
                .Select((j,_)=>j),
            Generate
        );
    }

    private void Generate(SourceProductionContext context, IGrouping<ITypeSymbol, GeneratorAttributeSyntaxContext> classToGenerate)
    {
        var cw = new SourceProductionCodeWriter(context);
        var notifyImplementation = InpcClassGeneratorFactory.StrategyForClass(classToGenerate.Key);
        var dependencies = new PropertyDependencyChecker();
        dependencies.AddClass(classToGenerate.Key);
        using (cw.GenerateInClassFile(classToGenerate.Key, "INPC", notifyImplementation.DeclareInterface()))
        {
            notifyImplementation.DeclareMethod(cw);
            foreach (var member in classToGenerate)
            {
                new AttributeCopier(cw, "property").CopyAttributesFrom(member.TargetNode);
                if (member.TargetSymbol is not IFieldSymbol field) continue;
                new FieldBlockGenerator(
                    cw, field, classToGenerate.Key, notifyImplementation,dependencies)
                    .RenderSingleField();
            }
        }
    }
}

public readonly struct AttributeCopier
{
    private readonly CodeWriter cw;
    private readonly string attributePrefix;

    public AttributeCopier(CodeWriter cw, string attributePrefix)
    {
        this.cw = cw;
        this.attributePrefix = attributePrefix;
    }

    public void CopyAttributesFrom(SyntaxNode sym)
    {
        if (sym.AncestorsAndSelf().OfType<MemberDeclarationSyntax>().FirstOrDefault() is {} mds)
        CopyAttributesFrom(mds);
    }

    public void CopyAttributesFrom(MemberDeclarationSyntax mds)
    {
        CopyAttributesFrom(mds.AttributeLists);
    }

    public void CopyAttributesFrom(IEnumerable<AttributeListSyntax> attrs)
    {
        foreach (var attributeListSyntax in attrs)
        {
            if (!IsDesiredAttributeTarget(attributeListSyntax)) continue;
            foreach (var attributeSyntax in attributeListSyntax.Attributes)
            {
                TryCopyAttributeFrom(attributeSyntax);
            }
        }
    }

    private bool IsDesiredAttributeTarget(AttributeListSyntax attributeListSyntax) =>
        attributePrefix.Equals(
            attributeListSyntax.Target?.Identifier.ValueText, StringComparison.Ordinal);

    private void TryCopyAttributeFrom(AttributeSyntax attributeSyntax)
    {
        cw.AppendLine($"[{attributeSyntax}]");
    }
}
#endif
