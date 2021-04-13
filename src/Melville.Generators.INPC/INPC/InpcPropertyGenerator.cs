using System;
using System.Text.RegularExpressions;
using Melville.Generators.INPC.AstUtilities;
using Melville.Generators.INPC.CodeWriters;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Melville.Generators.INPC.INPC
{
    public readonly struct InpcPropertyGenerator
    {
        private readonly FieldDeclarationSyntax fieldToWrap;
        private readonly IFieldSymbol fieldToWrapSymbol;
        private readonly CodeWriter codeWriter;
        private readonly ClassToImplement target;
        private readonly INotifyImplementationStategy notifyStrategy;
        private readonly VariableDeclaratorSyntax variable;
        private readonly string fieldName;
        private readonly string propertyName;

        public InpcPropertyGenerator(FieldDeclarationSyntax fieldToWrap,
            CodeWriter codeWriter,
            ClassToImplement target,
            INotifyImplementationStategy notifyStrategy, 
            VariableDeclaratorSyntax variable)
        {
            this.fieldToWrap = fieldToWrap;
            fieldToWrapSymbol =  
                (target.SemanticModel.GetDeclaredSymbol(fieldToWrap.Declaration.Variables.First())
                    as IFieldSymbol) ??
                    throw new InvalidProgramException("This should be an IFieldSymbol");;
            this.codeWriter = codeWriter;
            this.target = target;
            this.notifyStrategy = notifyStrategy;
            this.variable = variable;
            fieldName = this.variable.Identifier.ToString();
            propertyName = ComputePropertyName(fieldName);
        }

        public void DeclareWrappingProperty()
        {
            WritePropertyDecl();
            WritePartialChangeDeclaration();
        }

        private static readonly Regex propertyNameRegex = new("_*(.)(.*)");
        private static string ComputePropertyName(string fieldName)
        {
            var match = propertyNameRegex.Match(fieldName);
            return match.Groups[1].Value.ToUpper() + match.Groups[2].Value;
        }
        
        private void WritePropertyDecl()
        {
            WriteAttributes();
            WritePropertyLine();
            using var propIndent = codeWriter.CurlyBlock();
            WriteGetter();
            WriteSetter();
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
        private  bool HasPeerFilterHasMethod(string methodName) =>
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
            StoreOldValue();
            GenerateFieldAsignment();
            CallWhenChangesMethod();
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

        private void CallWhenChangesMethod()
        {
            codeWriter.AppendLine($"When{propertyName}Changes(___LocalOld, this.{fieldName});");
        }

        private void StoreOldValue()
        {
            codeWriter.AppendLine($"var ___LocalOld = this.{fieldName};");
        }

        private void GenerateFieldAsignment()
        {
            codeWriter.Append($"this.{fieldName} = ");
            ComputeSetterFilter();
            codeWriter.AppendLine($";");
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
        
        private void WriteAttributes()
        {
            codeWriter.CopyAttributes(fieldToWrap.AttributeLists, "AutoNotify");
        }

        
        private void WritePartialChangeDeclaration()
        {
            codeWriter.Append($"partial void When{propertyName}Changes(");
            codeWriter.WriteTypeSymbolName(fieldToWrapSymbol.Type);
            codeWriter.Append(" oldValue, ");
            codeWriter.WriteTypeSymbolName(fieldToWrapSymbol.Type);
            codeWriter.AppendLine(" newValue);");
        }

    }
}