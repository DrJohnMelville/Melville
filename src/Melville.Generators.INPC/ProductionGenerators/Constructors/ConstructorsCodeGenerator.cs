using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using Melville.Generators.INPC.GenerationTools.AbstractGenerators;
using Melville.Generators.INPC.GenerationTools.CodeWriters;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace System.Runtime.CompilerServices.ProductionGenerators.Constructors;

public class ConstructorsCodeGenerator : ILabeledMembersSemanticModel
{
    public TypeDeclarationSyntax ClassDeclaration { get; }
    public IList<MemberData> FieldsAndProperties { get; }
    public IList<MemberData[]> Constructors { get; }

    public ConstructorsCodeGenerator(
        TypeDeclarationSyntax classDeclaration, IList<MemberData> fieldsAndProperties, IList<MemberData[]> constructors)
    {
        FieldsAndProperties = fieldsAndProperties;
        Constructors = constructors;
        ClassDeclaration = classDeclaration;
    }
    
        public void GenerateCode(CodeWriter cw)
    {
        using var context = WriteCodeNear.Symbol(ClassDeclaration, cw);
        foreach (var constructor in Constructors.DefaultIfEmpty(Array.Empty<MemberData>()))
        {
            GenerateSingleConstructor(cw, constructor);
        }

        GeneratePartialMethodDeclaration(cw);
    }

    private static void GeneratePartialMethodDeclaration(CodeWriter cw)
    {
        cw.AppendLine("partial void OnConstructed();");
    }

    private void GenerateSingleConstructor(CodeWriter cw, IList<MemberData> constructorParams)
    {
        cw.Append("public ");
        cw.Append(ClassDeclaration.Identifier.ToString());
        cw.Append("(");
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

    private static void GenerateSingleParameter(CodeWriter cw, MemberData member)
    {
        cw.Append(member.Type);
        cw.Append(" ");
        cw.Append(member.LowerCaseName);
    }

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

    private static void GenerateFieldAssignent(CodeWriter cw, MemberData member)
    {
        cw.Append("this.");
        cw.Append(member.Name);
        cw.Append(" = ");
        cw.Append(member.LowerCaseName);
        cw.AppendLine(";");
    }
}