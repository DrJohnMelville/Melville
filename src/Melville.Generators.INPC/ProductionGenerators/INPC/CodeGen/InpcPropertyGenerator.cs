using System;
using System.Text.RegularExpressions;
using Melville.Generators.INPC.GenerationTools.AstUtilities;
using Melville.Generators.INPC.GenerationTools.CodeWriters;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Melville.Generators.INPC.ProductionGenerators.INPC.CodeGen;

public readonly struct InpcPropertyGenerator
{
    private readonly FieldDeclarationSyntax fieldToWrap;
    private readonly IFieldSymbol fieldToWrapSymbol;
    private readonly CodeWriter codeWriter;
    private readonly InpcSemanticModel target;
    private readonly INotifyImplementationStategy notifyStrategy;
    private readonly VariableDeclaratorSyntax variable;
    private readonly string fieldName;
    private readonly string propertyName;

    public InpcPropertyGenerator(FieldDeclarationSyntax fieldToWrap,
        CodeWriter codeWriter,
        InpcSemanticModel target,
        INotifyImplementationStategy notifyStrategy,
        VariableDeclaratorSyntax variable)
    {
        this.fieldToWrap = fieldToWrap;
        fieldToWrapSymbol =
            (target.SemanticModel.GetDeclaredSymbol(fieldToWrap.Declaration.Variables.First())
                as IFieldSymbol) ??
            throw new InvalidProgramException("This should be an IFieldSymbol");
        ;
        this.codeWriter = codeWriter;
        this.target = target;
        this.notifyStrategy = notifyStrategy;
        this.variable = variable;
        fieldName = this.variable.Identifier.ToString();
        propertyName = ComputePropertyName(fieldName);
    }

    public void DeclareWrappingProperty()
    {
        WriteAttributes();
        WritePropertyLine();
        using var propIndent = codeWriter.CurlyBlock();
        WriteGetter();
        WriteSetter();
    }

    private static readonly Regex propertyNameRegex = new("_*(.)(.*)");
    private static string ComputePropertyName(string fieldName)
    {
        var match = propertyNameRegex.Match(fieldName);
        return match.Groups[1].Value.ToUpper() + match.Groups[2].Value;
    }

    private static SearchForAttribute
        autoNotifyAttributeSearcher = new("Melville.INPC.AutoNotifyAttribute");

    private void WriteAttributes()
    {
        foreach (var attribute in autoNotifyAttributeSearcher.FindAllAttributes(fieldToWrap))
        {
            foreach (var (name, value) in attribute.ToArguments())
            {
                if (name?.Equals("Attributes", StringComparison.Ordinal) ?? false)
                {
                    codeWriter.AppendLine(value);
                }
            }
        }
    }

    private void WritePropertyLine()
    {
        codeWriter.Append("public ");
        codeWriter.WriteTypeSymbolName(fieldToWrapSymbol.Type);
        codeWriter.Append(" ");
        codeWriter.AppendLine(propertyName);
    }

    private void WriteGetter()
    {
        codeWriter.Append("get => this.");
        var getterFilterName = propertyName + "GetFilter";
        if (HasPeerFilterHasMethod(getterFilterName))
        {
            codeWriter.Append(getterFilterName);
            codeWriter.Append("(this.");
            codeWriter.Append(fieldName);
            codeWriter.Append(")");
        }
        else
        {
            codeWriter.Append(fieldName);
        }

        codeWriter.AppendLine(";");
    }

    private bool HasPeerFilterHasMethod(string methodName) =>
        fieldToWrapSymbol.ContainingSymbol is ITypeSymbol its &&
        its.HasMethod(fieldToWrapSymbol.Type, methodName, fieldToWrapSymbol.Type);

    private void WriteSetter()
    {
        codeWriter.AppendLine("set");
        using var setIndent = codeWriter.CurlyBlock();
        WriteSetterCode();
    }

    private void WriteSetterCode()
    {
        CallOnChangedMethod();
        GeneratePropertyChangeCalls();
    }

    private void GeneratePropertyChangeCalls()
    {
        foreach (var changedProperty in
                 target.PropertyDependencies.AllDependantProperties(propertyName))
        {
            notifyStrategy.PropertyChangeCallPrefix(codeWriter);
            codeWriter.Append("OnPropertyChanged(\"");
            codeWriter.Append(changedProperty);
            codeWriter.AppendLine("\");");
        }
    }

    private void CallOnChangedMethod()
    {
        switch (ParametersOfOnChangeMethod())
        {
            case 0:
                GenerateFieldAsignment();
                codeWriter.AppendLine($";");
                codeWriter.AppendLine($"this.On{propertyName}Changed();");
                break;
            case 1:
                codeWriter.Append($"this.On{propertyName}Changed(");
                GenerateFieldAsignment();
                codeWriter.AppendLine($");");
                break;
            case 2:
                codeWriter.Append($"this.On{propertyName}Changed(this.{fieldName}, ");
                GenerateFieldAsignment();
                codeWriter.AppendLine($");");
                break;
            default:
                GenerateFieldAsignment();
                codeWriter.AppendLine($";");
                break;
        }
    }

    private int ParametersOfOnChangeMethod()
    {
        var changeMethodName = $"On{propertyName}Changed";
        var targetType = fieldToWrapSymbol.Type;
        return true switch
        {
            true when
                target.TypeInfo.HasMethod(null, changeMethodName, targetType, targetType) => 2,
            true when
                target.TypeInfo.HasMethod(null, changeMethodName, targetType) => 1,
            true when
                target.TypeInfo.HasMethod(null, changeMethodName, Array.Empty<ITypeSymbol>()) => 0,
            _ => -1
        };
    }

    private void GenerateFieldAsignment()
    {
        codeWriter.Append($"this.{fieldName} = ");
        ComputeSetterFilter();
    }

    private void ComputeSetterFilter()
    {
        var setterFilterName = propertyName + "SetFilter";
        if (HasPeerFilterHasMethod(setterFilterName))
        {
            codeWriter.Append($"this.{setterFilterName}(value)");
        }
        else
        {
            codeWriter.Append("value");
        }
    }
}