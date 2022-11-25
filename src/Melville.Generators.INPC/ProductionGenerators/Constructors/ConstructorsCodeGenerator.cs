using System;
using System.Collections.Generic;
using System.Linq;
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
        cw.Append($"public {className}(");
        GenerateParameters(cw, constructorParams.Concat(FieldsAndProperties));
        cw.Append(")");
        if (constructorParams.Count > 0)
        {
            GenerateBaseCall(cw, constructorParams);
        }
        cw.AppendLine("");
        GenerateConstructorBody(cw);
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