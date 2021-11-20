using System;
using System.Linq;
using Melville.Generators.INPC.CodeWriters;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Melville.Generators.INPC.PartialTypeGenerators;

public abstract class PartialTypeGenerator<T> : ISourceGenerator
{
    protected abstract bool GlobalDeclarations(CodeWriter cw);
    protected abstract T PreProcess(
        IGrouping<TypeDeclarationSyntax, MemberDeclarationSyntax> input,
        GeneratorExecutionContext context);
    protected abstract bool GenerateClassContents(T input, CodeWriter cw);
    protected virtual string ClassSuffix(T input) => "";

    private readonly string[] targetAttributes;
    private readonly string suffix;
    private readonly GeneratedFileUniqueNamer namer;

    protected PartialTypeGenerator(string suffix, params string[] targetAttributes)
    {
        this.suffix = suffix;
        this.targetAttributes = targetAttributes;
        namer = new(suffix);
    }

    public void Initialize(GeneratorInitializationContext context) => 
        context.RegisterForSyntaxNotifications(()=>new PartialTypeReceiver(targetAttributes));

    public void Execute(GeneratorExecutionContext context)
    {
        namer.Clear();
        if (!(context.SyntaxReceiver is PartialTypeReceiver ptr)) return;
        try
        {
            GenerateCodeForAllClasses(context, ptr);
        }
        catch (Exception e)
        {
            ReportExceptionAsCompilerError(context, e);
        }
    }

    private static void ReportExceptionAsCompilerError(GeneratorExecutionContext context, Exception e)
    {
        context.ReportDiagnostic(Diagnostic.Create(new DiagnosticDescriptor("INPCGen0003",
                "Exception Thrown", e.Message + e.StackTrace, "Fatal", DiagnosticSeverity.Error, true),
            Location.None));
    }

    private void GenerateCodeForAllClasses(GeneratorExecutionContext context, PartialTypeReceiver ptr)
    {
        TryGenerateCode($"{suffix}Attributes", context, InnerGlobalDeclarations);
        foreach (var (group, intermed) in ptr.ItemsByType().Select(i=>(i, PreProcess(i,context))).ToList())
        {
            TryGeneratePartialClass(group.Key, context, intermed);
        }
    }

    private bool InnerGlobalDeclarations(CodeWriter cw)
    {
        cw.AppendLine($"namespace Melville.{suffix}");
        using var ns = cw.CurlyBlock();
        return GlobalDeclarations(cw);
    }

    private void TryGeneratePartialClass(
        TypeDeclarationSyntax parent, GeneratorExecutionContext context, T input)
    {
        TryGenerateCode(parent.Identifier.ToString(), context, cw =>
        {
            using (cw.GeneratePartialClassContext(parent))
            using (cw.GenerateEnclosingClasses(parent, ClassSuffix(input)))
            {
                return GenerateClassContents(input, cw);
            }
        });
    }
    private void TryGenerateCode(
        string proposedNamePrefix,
        GeneratorExecutionContext context, 
        Func<CodeWriter, bool> contentFunc)
    {
        var codeWriter = (CodeWriter)new GeneratorContextCodeWriter(context);
        if (!contentFunc(codeWriter)) return; // no code to generate
        codeWriter.PublishCodeInFile(namer.CreateFileName(proposedNamePrefix));
    }
}
    
public abstract class PartialTypeGenerator:
    PartialTypeGenerator<IGrouping<TypeDeclarationSyntax, MemberDeclarationSyntax>>
{
    protected PartialTypeGenerator(string suffix, params string[] targetAttributes) : 
        base(suffix, targetAttributes)
    {
    }

    protected override IGrouping<TypeDeclarationSyntax, MemberDeclarationSyntax> 
        PreProcess(IGrouping<TypeDeclarationSyntax, MemberDeclarationSyntax> input,
            GeneratorExecutionContext context) => input;
}