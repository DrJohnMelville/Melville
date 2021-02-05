using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Melville.Generators.INPC.INPC
{
    public class ClassFieldRecord{
        public List<FieldDeclarationSyntax> FieldsToWrap { get; } = new List<FieldDeclarationSyntax>();
        public List<PropertyDeclarationSyntax> ProperteisToMap { get; } = new List<PropertyDeclarationSyntax>();
        public TypeDeclarationSyntax ClassDeclaration { get; }
        private SemanticModel? semanticModel;

        public SemanticModel SemanticModel
        {
            get => semanticModel ??
                   throw new InvalidOperationException("Semantic model is not yet initialized;");
            set
            {
                semanticModel = value;
                var def = semanticModel.GetTypeInfo(ClassDeclaration);
            }
        }

        public ClassFieldRecord(TypeDeclarationSyntax classDeclaration)
        {
            this.ClassDeclaration = classDeclaration;
        }

        public void AddField(FieldDeclarationSyntax field)
        {
            FieldsToWrap.Add(field);
        }
        public void AddProperty(PropertyDeclarationSyntax prop)
        {
            ProperteisToMap.Add(prop);
        }

        public ClassToImplement ElaborateSemanticInfo(Compilation compilation)
        {
            return new ClassToImplement(FieldsToWrap, ClassDeclaration,
                compilation.GetSemanticModel(ClassDeclaration.SyntaxTree), 
                MapPropertyDependencies());
        }

        private PropertyDependencyChecker MapPropertyDependencies()
        {
            var pc = new PropertyDependencyChecker();
            foreach (var propertyDeclarationSyntax in ProperteisToMap)
            {
                pc.AddProperty(propertyDeclarationSyntax);
            }

            return pc;
        }
    }
}