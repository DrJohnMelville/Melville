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
    // public void Initialize(GeneratorInitializationContext context)
    // {
    //     context.RegisterForSyntaxNotifications(() => new INPCReceiver());
    // }
    //
    // public void Execute(GeneratorExecutionContext context)
    // {
    //     try
    //     {
    //         InjectINPCProperties(context);
    //     }
    //     catch (Exception e)
    //     {
    //         context.ReportDiagnostic(Diagnostic.Create(new DiagnosticDescriptor("INPCGen0003",
    //                 "Exception Thrown", e.Message + e.StackTrace, "Fatal", DiagnosticSeverity.Error, true),
    //             Location.None));
    //     }
    // }
    //
    // private void InjectINPCProperties(GeneratorExecutionContext context)
    // {
    //     if (!(context.SyntaxReceiver is INPCReceiver receiver))
    //         throw new InvalidProgramException("Receiver is the wrong type");
    //
    //     var elaboratedClasses = receiver.ClassesToAugment.Values
    //         .Select(i =>
    //         {
    //             Compilation compilation = context.Compilation;
    //             return i.ElaborateSemanticInfo(compilation,
    //                 compilation.GetSemanticModel(i.ClassDeclaration.SyntaxTree));
    //         })
    //         .ToList();
    //
    //     var factory = new InpcClassGeneratorFactory(elaboratedClasses,
    //         context.Compilation.GetSpecialType(SpecialType.System_String));
    //
    //     var namer = new GeneratedFileUniqueNamer("INPC");
    //
    //
    //     foreach (var augmenter in elaboratedClasses)
    //     {
    //         factory.CreateGenerator(augmenter, context).Generate(namer);
    //     }
    // }

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
        var cw = new SourceProductionCodeWriter(context);
        InpcClassGeneratorFactory.CreateGenerator(classToImplement, cw).WriteToCodeWriter();
        cw.PublishCodeInFile(classToImplement.ClassDeclaration, "INPC");
    }

    private static TypeDeclarationSyntax ParentClassSelector(GeneratorSyntaxContext j) =>
        j.Node as TypeDeclarationSyntax ??
        j.Node.Parent as TypeDeclarationSyntax ??
        throw new InvalidDataException("target must be a type of a member of a type;");

    private bool TokenSelector(SyntaxNode sn, CancellationToken _) =>
        sn is MemberDeclarationSyntax mds && attrFinder.HasAttribute(mds);
}