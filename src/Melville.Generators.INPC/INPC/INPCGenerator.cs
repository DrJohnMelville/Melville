using System;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Threading;
using Melville.Generators.INPC.AstUtilities;
using Melville.Generators.INPC.CodeWriters;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Melville.Generators.INPC.INPC;

[Generator]
public class INPCGenerator : IIncrementalGenerator
{
    private static readonly SearchForAttribute attrFinder = new("Melville.INPC.AutoNotifyAttribute");

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterSourceOutput(
            context.SyntaxProvider.CreateSyntaxProvider(
                    TokenSelector,
                    static (gsc, _) => gsc
                )
                .Collect()
                .SelectMany(static (i, _) => i
                    .GroupBy(ParentClassSelector)
                    .Select(static i =>
                    {
                        var ret = new ClassFieldRecord(i.Key);
                        foreach (var member in i)
                        {
                            switch (member.Node)
                            {
                                case FieldDeclarationSyntax fds:
                                    ret.AddField(fds);
                                    break;
                                case PropertyDeclarationSyntax pds:
                                    ret.AddProperty(pds);
                                    break;
                            }
                        }

                        return ret.ElaborateSemanticInfo(i.First().SemanticModel);
                    }))
            , GenerateClass);
    }

    private void GenerateClass(SourceProductionContext context, ClassToImplement classToImplement)
    {
        classToImplement.GenerateCode(context);
    }

    private static TypeDeclarationSyntax ParentClassSelector(GeneratorSyntaxContext j) =>
        j.Node as TypeDeclarationSyntax ??
        j.Node.Parent as TypeDeclarationSyntax ??
        throw new InvalidDataException("target must be a type of a member of a type;");

    private bool TokenSelector(SyntaxNode sn, CancellationToken _) =>
        sn is MemberDeclarationSyntax mds && attrFinder.HasAttribute(mds);
}