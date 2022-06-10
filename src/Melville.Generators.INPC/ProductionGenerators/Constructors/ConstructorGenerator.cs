using System.Collections.Generic;
using Melville.Generators.INPC.GenerationTools.AbstractGenerators;
using Melville.Generators.INPC.GenerationTools.AstUtilities;
using Melville.Generators.INPC.GenerationTools.CodeWriters;
using Melville.Generators.INPC.ProductionGenerators.INPC;
using Melville.INPC;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Win32.SafeHandles;

namespace System.Runtime.CompilerServices.ProductionGenerators.Constructors;

[Generator]
public class ConstructorGenerator: ClassWithLabeledMembersGenerator
{
        private static readonly SearchForAttribute attrFinder = new("Melville.INPC.FromConstructorAttribute");

        public ConstructorGenerator() : base(attrFinder)
        {
        }
        protected override ILabeledMembersSyntaxModel CreateMemberRecord(TypeDeclarationSyntax targetTypeDecl) => 
            new ConstructorClassModel(targetTypeDecl);
    }

public class ConstructorClassModel : ILabeledMembersSyntaxModel, ILabeledMembersSemanticModel
{
    public TypeDeclarationSyntax ClassDeclaration { get; }
    private readonly List<MemberData> members  = new();

    public ConstructorClassModel(TypeDeclarationSyntax type)
    {
        ClassDeclaration = type;
    }

    public void AddMember(SyntaxNode declSyntax)
    {
        switch (declSyntax)
        {
            case PropertyDeclarationSyntax pds :
                members.Add(new (pds.Type.ToString(), pds.Identifier.ToString()));
                break;
        }
    }

    public ILabeledMembersSemanticModel AddSemanticInfo(SemanticModel semanticModel) => this;

    public void GenerateCode(CodeWriter cw)
    {
        using var context = WriteCodeNear.Symbol(ClassDeclaration, cw);
        cw.Append("public ");
        cw.Append(ClassDeclaration.Identifier.ToString());
        cw.Append("(");
        int pos = 0;
        GenerateParameters(cw, pos);
        cw.AppendLine(")");
        GenerateConstructorBody(cw);
    }

    private void GenerateParameters(CodeWriter cw, int pos)
    {
        foreach (var member in members)
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
        }
    }

    private void GenerateFieldAssignments(CodeWriter cw)
    {
        foreach (var member in members)
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

public record struct MemberData(string Type, string Name, string LowerCaseName)
{
    public MemberData(string type, string name) : this(type, name, name.AsInstanceSymbol())
    {
    }
}