using System;
using System.Collections.Immutable;
using System.Linq;
using Melville.Generators.INPC.AstUtilities;
using Melville.Generators.INPC.CodeWriters;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Melville.Generators.INPC.DelegateToGen
{
    public class InterfaceMethodGenerator : DelegatedMethodGenerator
    {
        public InterfaceMethodGenerator(ITypeSymbol targetType, string methodPrefix) : 
            base(targetType, methodPrefix)
        {
        }
        public override string InheritFrom() => TargetType.FullyQualifiedName();
        private string MemberDeclarationPrefix() => "public ";

        public override void GenerateForwardingMethods(ITypeSymbol parentClass, CodeWriter cw)
        {
            foreach (var member in TargetType.GetMembers())
            {
                GenerateForwardingMember(cw, member);
            }
        }

        private void GenerateForwardingMember(CodeWriter cw, ISymbol member)
        {
            switch (member)
            {
                case IPropertySymbol ps :
                    GenerateProperty(ps, cw);
                    break;
                case IMethodSymbol ms :
                    TryGenerateMethod(ms, cw);
                    break;
                case IEventSymbol es :
                    GenerateEvent(es, cw);
                    break;
                default:
                    cw.AppendLine($"// call {member.Name} using : {MethodPrefix}");
                    break;                    
            }
        }

        private void GenerateEvent(IEventSymbol es, CodeWriter cw)
        {
            MemberPrefix(cw, "event "+es.Type.FullyQualifiedName(), es.Name);
            cw.AppendLine();
            using (cw.CurlyBlock())
            {
                if (es.AddMethod != null)
                {
                    cw.AppendLine($"add => {MethodPrefix}{es.Name} += value;");
                }
                if (es.RemoveMethod != null)
                {
                    cw.AppendLine($"remove => {MethodPrefix}{es.Name} -= value;");
                }
            }
        }

        private void TryGenerateMethod(IMethodSymbol ms, CodeWriter cw)
        {
            if (IsSpecialExcludedMethod(ms)) return;
            GenerateMethod(ms, cw);
        }

        private void GenerateMethod(IMethodSymbol ms, CodeWriter cw)
        {
            MemberPrefix(cw, ms.ReturnType.FullyQualifiedName(), ms.Name);
            AppendTypeParamList(cw, ms.TypeParameters);
            cw.Append("(");
            AppendArgumentList(cw, ms.Parameters, i => $"{i.Type.FullyQualifiedName()} {i.Name}");
            cw.Append(") => ");
            cw.Append(MethodPrefix);
            cw.Append(ms.Name);
            AppendTypeParamList(cw, ms.TypeParameters);
            cw.Append("(");
            AppendArgumentList(cw, ms.Parameters, i => i.Name);
            cw.AppendLine(");");
        }

        // we don't generate the component methods of properties, events, or indexers, because
        // we generate those using higher level constructs.
        private bool IsSpecialExcludedMethod(IMethodSymbol ms) => !ms.CanBeReferencedByName;

        private void AppendArgumentList(CodeWriter cw, ImmutableArray<IParameterSymbol> parameters, Func<IParameterSymbol, string> ParamPrinter) => 
            cw.Append(string.Join(", ", parameters.Select(ParamPrinter)));

        private void AppendTypeParamList(
            CodeWriter cw, ImmutableArray<ITypeParameterSymbol> parameters)
        {
            if (parameters.Length == 0) return;
            cw.Append("<");
            cw.Append(string.Join(",", parameters.Select(i=>i.Name)));
            cw.Append(">");
        }

        private void GenerateProperty(IPropertySymbol ps, CodeWriter cw)
        {
            MemberPrefix(cw, ps.Type.FullyQualifiedName(), ps.Name);
            cw.AppendLine();
            using (cw.CurlyBlock())
            {
                if (ps.GetMethod != null)
                {
                    cw.AppendLine($"get => {MethodPrefix}{ps.Name};");
                }
                if (ps.SetMethod is {} setMethod)
                {
                    cw.AppendLine($"{setMethodKeyword(setMethod)} => {MethodPrefix}{ps.Name} = value;");
                }
            }
        }

        private void MemberPrefix(CodeWriter cw, string typeName, string memberName)
        {
            cw.Append(MemberDeclarationPrefix());
            cw.Append(typeName);
            cw.Append(" ");
            cw.Append(memberName);
        }

        private string setMethodKeyword(IMethodSymbol ps) => ps.IsInitOnly ? "init" : "set";
    }
}