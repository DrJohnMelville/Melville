using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Melville.Generators.INPC.GenerationTools.CodeWriters;

namespace Melville.Generators.INPC.ProductionGenerators.Constructors;

public readonly struct ConstructorsCodeGenerator
{
    private readonly string className;
    private IList<MemberData> FieldsAndProperties { get; }
    private IList<MemberData[]> Constructors { get; }

    public ConstructorsCodeGenerator(
        string className, IList<MemberData> fieldsAndProperties, IList<MemberData[]> constructors)
    {
        this.className = className;
        FieldsAndProperties = fieldsAndProperties;
        Constructors = constructors;
    }
    
    public void GenerateCode(CodeWriter cw)
    {
        foreach (var constructor in Constructors.DefaultIfEmpty(Array.Empty<MemberData>()))
        {
            GenerateSingleConstructor(cw, constructor);
        }
        GeneratePartialMethodDeclaration(cw);
    }

    private static void GeneratePartialMethodDeclaration(CodeWriter cw) => 
        cw.AppendLine("partial void OnConstructed();");

    private void GenerateSingleConstructor(CodeWriter cw, IList<MemberData> constructorParams)
    {
        var allParams = constructorParams.Concat(FieldsAndProperties).ToList();
        TryGenerateDocumentation(cw, allParams);
        cw.Append($"public {className}(");
        GenerateParameters(cw, allParams);
        cw.Append(")");
        if (constructorParams.Count > 0)
        {
            GenerateBaseCall(cw, constructorParams);
        }
        cw.AppendLine("");
        GenerateConstructorBody(cw);
    }

    private void TryGenerateDocumentation(CodeWriter cw, IList<MemberData> allParams)
    {
        if (allParams.All(i => string.IsNullOrWhiteSpace(i.XmlDocummentation))) return;
        cw.AppendLine("/// <summary>");
        cw.AppendLine($"/// Auto generated constructor for {className}");
        cw.AppendLine("/// </summary>");
        foreach (var param in allParams)
        {
            cw.CopyWithLinePrefix("/// ",
                $"""<param name="{param.LowerCaseName}">{StripSummaryTags(param.XmlDocummentation)}</param> """.AsSpan());
        }
    }

    private static readonly Regex SummaryFinder = new Regex(@"\s*</?(?:summary|member)[^>]*>\s*");
    private string StripSummaryTags(string? documentation)
    {
        return SummaryFinder.Replace(documentation??"", "");
    }

    private void GenerateBaseCall(CodeWriter cw, IList<MemberData> constructorParams)
    {
        cw.Append(": base(");
        int pos = 0;
        foreach (var argument in constructorParams)
        {
            if (pos++ > 0) cw.Append(", ");
            cw.Append(argument.LowerCaseName);
        }
        cw.Append(")");
    }

    private void GenerateParameters(CodeWriter cw, IEnumerable<MemberData> parameters)
    {
        int pos = 0;
        foreach (var member in parameters)
        {
            if (pos++ > 0) cw.Append(", ");
            GenerateSingleParameter(cw, member);
        }
    }

    private static void GenerateSingleParameter(CodeWriter cw, MemberData member) => 
        cw.Append($"{ member.Type} {member.LowerCaseName}");

    private void GenerateConstructorBody(CodeWriter cw)
    {
        using (cw.CurlyBlock())
        {
            GenerateFieldAssignments(cw);
            GeneratePartialMethodCall(cw);
        }
    }

    private static void GeneratePartialMethodCall(CodeWriter cw) => cw.AppendLine("OnConstructed();");

    private void GenerateFieldAssignments(CodeWriter cw)
    {
        foreach (var member in FieldsAndProperties)
        {
            GenerateFieldAssignent(cw, member);
        }
    }

    private static void GenerateFieldAssignent(CodeWriter cw, MemberData member) => 
        cw.AppendLine($"this.{member.Name} = {member.LowerCaseName};");
}