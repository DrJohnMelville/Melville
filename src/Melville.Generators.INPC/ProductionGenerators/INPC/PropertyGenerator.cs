using System;
using Melville.Generators.INPC.GenerationTools.AstUtilities;
using Melville.Generators.INPC.GenerationTools.CodeWriters;
using Microsoft.CodeAnalysis;

namespace Melville.Generators.INPC.ProductionGenerators.INPC;

public readonly struct PropertyGenerator
{
    private readonly CodeWriter target;
    private readonly IFieldSymbol field;
    private readonly ITypeSymbol parentClass;
    private readonly string propertyName;
    private readonly string changeFuncName;
    public readonly INotifyImplementationStategy notifyStrategy;
    private readonly PropertyDependencyGraph dependencies;
    private readonly string propertyModifiers;


    public PropertyGenerator(CodeWriter target, IFieldSymbol field, ITypeSymbol parentClass,
        INotifyImplementationStategy notifyStrategy, PropertyDependencyGraph dependencies, 
        string propertyModifiers)
    {
        this.target = target;
        this.field = field;
        this.parentClass = parentClass;
        this.notifyStrategy = notifyStrategy;
        this.dependencies = dependencies;
        this.propertyModifiers = propertyModifiers;
        propertyName = field.Name.ComputePropertyName();
        changeFuncName = $"On{propertyName}Changed";
    }

    public void RenderSingleField()
    {
        new DocumentationCopier(target).Copy(field);
        target.AppendLine($"{propertyModifiers} {field.Type.FullyQualifiedName()} {propertyName}");
        using var _ = target.CurlyBlock();
        GemetateGetBlock();
        GenerateSetBlock(field);
    }

    private void GemetateGetBlock() => 
        target.AppendLine($"get => {TryIncludeFilter("GetFilter", "this."+field.Name)};");

    private string TryIncludeFilter(string filterType, string source)
    {
        var getFilterName = propertyName+filterType;
        if (parentClass.HasMethod(field.Type, getFilterName, field.Type))
            return $"this.{getFilterName}({source})";
        return source;
    }
    private void GenerateSetBlock(IFieldSymbol fieldSymbol)
    {
        target.AppendLine("set");
        using var _ = target.CurlyBlock();
        target.AppendLine(
            TryWrapTwoParameterChangeCall($"this.{field.Name} = {TryIncludeFilter("SetFilter", "value")}"));
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
            target.AppendLine($"this.{changeFuncName}(this.{field.Name});");
    }

    private string TryWrapTwoParameterChangeCall(string assignmentStatement)
    {
        if (parentClass.HasMethod(null, changeFuncName, field.Type, field.Type))
            return $"this.{changeFuncName}(this.{field.Name}, {assignmentStatement});";
        return assignmentStatement + ";";
    }

    private void WritePropertyNotifications()
    {
        foreach (var dependency in dependencies.AllDependantProperties(propertyName))
        {
            notifyStrategy.RenderPropertyNotification(target, dependency );
        }
    }
}