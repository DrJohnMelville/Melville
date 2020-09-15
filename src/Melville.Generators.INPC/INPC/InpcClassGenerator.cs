﻿using System;
using System.Text.RegularExpressions;
using Melville.Generators.INPC.Common.AstUtilities;
using Melville.Generators.INPC.Common.CodeWriters;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Melville.Generators.INPC.INPC
{
    public class InpcClassGenerator
    {
        protected ClassToImplement Target { get; }
        private readonly INotifyImplementationStategy notifyStrategy;
        protected CodeWriter CodeWriter { get; }

        public InpcClassGenerator(
            ClassToImplement target, 
            INotifyImplementationStategy notifyStrategy, 
            SourceGeneratorContext context)
        {
            Target = target;
            this.notifyStrategy = notifyStrategy;
            CodeWriter = new CodeWriter(context);
        }

        public void Generate()
        {
            GenerateCodeForClass();
            CodeWriter.PublishCodeInFile($"{ClassName()}.GeneratedINPC.cs");
        }
        private string ClassName() => Target.ClassDeclaration.Identifier.ToString();

        private void GenerateCodeForClass()
        {
            using var ns = CodeWriter.GenerateEnclosingNamespaces(Target.ClassDeclaration);
            using var classes = GenerateClassDeclaration();
            notifyStrategy.DeclareMethod(CodeWriter);
            GenerateAllPropertyDeclarations();
        }


        private IDisposable GenerateClassDeclaration()
        {
            return CodeWriter.GenerateEnclosingClasses(Target.ClassDeclaration, 
                notifyStrategy.DeclareInterface());
        }

        private void GenerateAllPropertyDeclarations()
        {
            foreach (var fieldToWrap in Target.FieldsToWrap)
            { 
                GenerateProperty(fieldToWrap);
            }
        }

        private void GenerateProperty(FieldDeclarationSyntax fieldToWrap)
        {
            var decl = fieldToWrap.Declaration;
            var typeName= (Target.SemanticModel.GetDeclaredSymbol(decl.Variables.First()) as IFieldSymbol) ??
                          throw new InvalidProgramException("This should be an IFieldSymbol");
            foreach (var variable in decl.Variables)
            {
                DeclareWrappingProperty(typeName, variable);
            }
        }

        private void DeclareWrappingProperty(IFieldSymbol type, VariableDeclaratorSyntax variable)
        {
            var fieldName = variable.Identifier.ToString();
            var propertyName = ComputePropertyName(fieldName);
            WritePropertyDecl(type, propertyName, fieldName);
            WritePartialChangeDeclaration(type, propertyName);
        }

        private void WritePartialChangeDeclaration(IFieldSymbol type, string propertyName)
        {
            CodeWriter.Append($"partial void When{propertyName}Changes(");
            CodeWriter.WriteTypeSymbolName(type.Type);
            CodeWriter.Append(" oldValue, ");
            CodeWriter.WriteTypeSymbolName(type.Type);
            CodeWriter.AppendLine(" newValue);");
        }

        private void WritePropertyDecl(IFieldSymbol type, string propertyName, string fieldName)
        {
            WritePropertyLine(type, propertyName);
            using var propIndent = CodeWriter.CurlyBlock();
            WriteGetter(fieldName);
            CodeWriter.AppendLine("set");
            using var setIndent = CodeWriter.CurlyBlock();
            WriteSetterCode(propertyName, fieldName);
        }

        private void WriteSetterCode(string propertyName, string fieldName)
        {
            CodeWriter.AppendLine($"var ___LocalOld = this.{fieldName};");
            CodeWriter.AppendLine($"this.{fieldName} = value;");
            CodeWriter.AppendLine($"When{propertyName}Changes(___LocalOld, value);");
            foreach (var changedProperty in 
                Target.PropertyDependencies.AllDependantProperties(propertyName))
            {
                notifyStrategy.PropertyChangeCallPrefix(CodeWriter);
                CodeWriter.Append("OnPropertyChanged(\"");
                CodeWriter.Append(changedProperty);
                CodeWriter.AppendLine("\");");
            }
        }

        private void WriteGetter(string fieldName)
        {
            CodeWriter.Append("get => this.");
            CodeWriter.Append(fieldName);
            CodeWriter.AppendLine(";");
        }

        private void WritePropertyLine(IFieldSymbol type, string propertyName)
        {
            CodeWriter.Append("public ");
            CodeWriter.WriteTypeSymbolName(type.Type);
            CodeWriter.Append(" ");
            CodeWriter.AppendLine(propertyName);
        }

        private string ComputePropertyName(string fieldName)
        {
            var match = Regex.Match(fieldName, "_*(.)(.*)");
            return match.Groups[1].Value.ToUpper() + match.Groups[2].Value;
        }
    }
}