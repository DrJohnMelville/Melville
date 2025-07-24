using System;
using Melville.Generators.INPC.GenerationTools.AstUtilities;
using Melville.Generators.INPC.GenerationTools.CodeWriters;
using Melville.Generators.INPC.GenerationTools.DocumentationCopiers;
using Microsoft.CodeAnalysis;

namespace Melville.Generators.INPC.ProductionGenerators.INPC;

public interface IPropertySourceSymbol
{
    string FieldName { get; }
    string PropertyName { get; }
    ITypeSymbol Type { get; }
    ISymbol DocumentationSource { get; }
    SyntaxNode SourceSyntax { get; }
    string PropertyAccessLevel { get; }
    void TryWriteFieldDeclaration(CodeWriter target);
    string GetterAccess { get; }
    string SetterAccess { get; }
}

public readonly struct PropertyGenerator
{
    private readonly CodeWriter target;
    private readonly IPropertySourceSymbol field;
    private readonly ITypeSymbol parentClass;
    private readonly string changeFuncName;
    public readonly INotifyImplementationStategy notifyStrategy;
    private readonly PropertyDependencyGraph dependencies;
    
    public PropertyGenerator(CodeWriter target, IPropertySourceSymbol field, ITypeSymbol parentClass,
        INotifyImplementationStategy notifyStrategy, PropertyDependencyGraph dependencies)
    {
        this.target = target;
        this.field = field;
        this.parentClass = parentClass;
        this.notifyStrategy = notifyStrategy;
        this.dependencies = dependencies;
        changeFuncName = $"On{field.PropertyName}Changed";
    }

    public void RenderSingleField()
    {
        field.TryWriteFieldDeclaration(target);

        new AttributeCopier(target, "property").CopyAttributesFrom(field.SourceSyntax);
        new DocumentationCopier(target).Copy(field.DocumentationSource);
        target.AppendLine($"{field.PropertyAccessLevel} {field.Type.FullyQualifiedName()} {field.PropertyName}");
        using var _ = target.CurlyBlock();
        GemetateGetBlock();
        GenerateSetBlock();
    }

    private void GemetateGetBlock() => 
        target.AppendLine($"{field.GetterAccess}get => {TryIncludeFilter("GetFilter", "this."+field.FieldName)};");

    private string TryIncludeFilter(string filterType, string source)
    {
        var getFilterName = field.PropertyName+filterType;
        if (parentClass.HasMethod(field.Type, getFilterName, field.Type))
            return $"this.{getFilterName}({source})";
        return source;
    }
    private void GenerateSetBlock()
    {
        target.AppendLine($"{field.SetterAccess}set");
        using var _ = target.CurlyBlock();
        target.AppendLine(
            TryWrapTwoParameterChangeCall($"this.{field.FieldName} = {TryIncludeFilter("SetFilter", "value")}"));
        TryWriteOneParameterChangeCall();
        TryWriteZeroParameterChangeCall();
        WritePropertyNotifications();
    }

    private void TryWriteZeroParameterChangeCall()
    {
        if (parentClass.HasMethod(null, changeFuncName, Array.Empty<ITypeSymbol>()))
            target.AppendLine($"this.{changeFuncName}();");
    }

    private void TryWriteOneParameterChangeCall()
    {
        if (parentClass.HasMethod(null, changeFuncName, field.Type))
            target.AppendLine($"this.{changeFuncName}(this.{field.FieldName});");
    }

    private string TryWrapTwoParameterChangeCall(string assignmentStatement)
    {
        if (parentClass.HasMethod(null, changeFuncName, field.Type, field.Type))
            return $"this.{changeFuncName}(this.{field.FieldName}, {assignmentStatement});";
        return assignmentStatement + ";";
    }

    private void WritePropertyNotifications()
    {
        foreach (var dependency in dependencies.AllDependantProperties(field.PropertyName))
        {
            notifyStrategy.RenderPropertyNotification(target, dependency );
        }
    }
}