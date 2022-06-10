using System;
using Melville.Generators.INPC.CodeWriters;

namespace Melville.Generators.INPC.INPC.CodeGen;

public sealed class InpcClassGenerator
{
    private readonly InpcSemanticModel target;
    private readonly INotifyImplementationStategy notifyStrategy;
    private readonly CodeWriter codeWriter;

    public InpcClassGenerator(
        InpcSemanticModel target, 
        INotifyImplementationStategy notifyStrategy, 
        CodeWriter writer)
    {
        this.target = target;
        this.notifyStrategy = notifyStrategy;
        codeWriter = writer;
    }

    public void WriteToCodeWriter()
    {
        codeWriter.AppendLine("#nullable enable");
        GenerateCodeForClass();
    }

    private void GenerateCodeForClass()
    {
        using var ns = codeWriter.GeneratePartialClassContext(target.ClassDeclaration);
        using var classes = GenerateClassDeclaration();
        notifyStrategy.DeclareMethod(codeWriter);
        GenerateAllPropertyDeclarations();
    }


    private IDisposable GenerateClassDeclaration()
    {
        return codeWriter.GenerateEnclosingClasses(target.ClassDeclaration, 
            notifyStrategy.DeclareInterface());
    }

    private void GenerateAllPropertyDeclarations()
    {
        foreach (var fieldToWrap in target.FieldsToWrap)
        foreach (var variable in fieldToWrap.Declaration.Variables)
        {
            new InpcPropertyGenerator(fieldToWrap, codeWriter, target, notifyStrategy, variable)
                .DeclareWrappingProperty();
                
        }    
    }
}