﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Melville.Generators.INPC.AstUtilities;
using Melville.Generators.INPC.CodeWriters;
using Microsoft.CodeAnalysis;

namespace Melville.Generators.INPC.DelegateToGen
{
    public interface IDelegatedMethodGenerator
    {
        string? InheritFrom();
        void GenerateForwardingMethods(ITypeSymbol parentClass, CodeWriter cw);
    }
    
    public abstract class DelegatedMethodGenerator : IDelegatedMethodGenerator
    {
        protected readonly ITypeSymbol TargetType;
        private readonly string MethodPrefix;

        public static IDelegatedMethodGenerator Create(
            ITypeSymbol targetType, string methodPrefix, SyntaxNode location) =>
            targetType.TypeKind switch
            {
                TypeKind.Interface => new InterfaceMethodGenerator(targetType, methodPrefix),
                TypeKind.Class => new BaseClassMethodGenerator(targetType, methodPrefix),
                _ => new InvalidParentMethodGenerator(targetType, location)
            };

        protected abstract string MemberDeclarationPrefix();
        protected abstract IEnumerable<ISymbol> MembersThatCouldBeForwarded(ITypeSymbol parentClass);
 
        protected DelegatedMethodGenerator(ITypeSymbol targetType, string methodPrefix)
        {
            this.TargetType = targetType;
            this.MethodPrefix = methodPrefix;
        }

        public string InheritFrom() => TargetType.FullyQualifiedName();

        public void GenerateForwardingMethods(ITypeSymbol parentClass, CodeWriter cw)
        {
            foreach (var member in MembersToForward(parentClass))
            {
                    GenerateForwardingMember(cw, member);
            }
        }

        private IEnumerable<ISymbol> MembersToForward(ITypeSymbol parentClass) =>
            MembersThatCouldBeForwarded(parentClass)
                .Where(i=>ImplementationMissing(parentClass, i));

        private IEnumerable<ITypeSymbol> TargetTypeAndParents() => 
            TargetType.AllInterfaces.Cast<ITypeSymbol>().Append(TargetType);

        protected abstract bool ImplementationMissing(ITypeSymbol parentClass, ISymbol i); 

        private void GenerateForwardingMember(CodeWriter cw, ISymbol member)
        {
            switch (member)
            {
                case IPropertySymbol {IsIndexer: true} ps:
                    GenerateIndexer(ps, cw);
                    break;
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

        private void GenerateIndexer(IPropertySymbol ps, CodeWriter cw)
        {
            MemberPrefix(cw, ps.Type.FullyQualifiedName(), "this" );
            ParameterList(cw, ps.Parameters, i => $"{i.Type.FullyQualifiedName()} {i.Name}", "[", "]");
            cw.AppendLine();
            PropertyBlock(ps, cw, $"[{string.Join(", ", ps.Parameters.Select(i=>i.Name))}]");
        }
        
        private void GenerateProperty(IPropertySymbol ps, CodeWriter cw)
        {
            MemberPrefix(cw, ps.Type.FullyQualifiedName(), ps.Name);
            cw.AppendLine();
            PropertyBlock(ps, cw, "."+ps.Name);
        }

        private void PropertyBlock(IPropertySymbol ps, CodeWriter cw, string propertyCall)
        {
            using (cw.CurlyBlock())
            {
                if (ps.GetMethod != null)
                {
                    cw.AppendLine($"get => {MethodPrefix}{propertyCall};");
                }

                if (ps.SetMethod is { } setMethod)
                {
                    cw.AppendLine($"{setMethodKeyword(setMethod)} => {MethodPrefix}{propertyCall} = value;");
                }
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
                    cw.AppendLine($"add => {MethodPrefix}.{es.Name} += value;");
                }
                if (es.RemoveMethod != null)
                {
                    cw.AppendLine($"remove => {MethodPrefix}.{es.Name} -= value;");
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
            ParameterList(cw, ms.Parameters, i => $"{i.Type.FullyQualifiedName()} {i.Name}", "(", ")");
            cw.Append(" => ");
            cw.Append(MethodPrefix);
            cw.Append(".");
            cw.Append(ms.Name);
            AppendTypeParamList(cw, ms.TypeParameters);
            ParameterList(cw, ms.Parameters, i => i.Name, "(", ")");
            cw.AppendLine(";");
        }

        private void ParameterList(
            CodeWriter cw, ImmutableArray<IParameterSymbol> parameters, 
            Func<IParameterSymbol, string> display, string open, string close)
        {
            cw.Append(open);
            AppendArgumentList(cw, parameters, display);
            cw.Append(close);
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