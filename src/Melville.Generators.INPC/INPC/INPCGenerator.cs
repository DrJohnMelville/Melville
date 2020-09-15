using System;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Melville.Generators.INPC.INPC
{
    [Generator]
    public class INPCGenerator : ISourceGenerator
    {
        public void Initialize(InitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new INPCReceiver());
        }

        public void Execute(SourceGeneratorContext context)
        {
            try
            {
                InjectGenerationAttributes(context);
                InjectINPCProperties(context);
            }
            catch (Exception e)
            {
                context.ReportDiagnostic(Diagnostic.Create(new DiagnosticDescriptor("INPCGen0003", 
                        "Exception Thrown", e.Message+e.StackTrace,"Fatal", DiagnosticSeverity.Error, true),
                    Location.None));
            }
        }

        private void InjectINPCProperties(SourceGeneratorContext context)
        {
            if (!(context.SyntaxReceiver is INPCReceiver receiver))
                throw new InvalidProgramException("Receiver is the wrong type");
      
            var elaboratedClasses = receiver.ClassesToAugment.Values
                .Select(i => i.ElaborateSemanticInfo(context.Compilation))
                .ToList();
      
            var factory = new InpcClassGeneratorFactory(elaboratedClasses,
                context.Compilation.GetSpecialType(SpecialType.System_String));
      
            foreach (var augmenter in elaboratedClasses)
            {
                factory.CreateGenerator(augmenter, context).Generate();
            }
        }

        private static void InjectGenerationAttributes(SourceGeneratorContext context)
        {
            var sourceText = SourceText.From(@"
using System;
using System.ComponentModel;

namespace Melville.Generated
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, 
                   Inherited = false, AllowMultiple = false)]
    sealed internal class AutoNotifyAttribute : Attribute
    {
    }

    internal interface IExternalNotifyPropertyChanged:System.ComponentModel.INotifyPropertyChanged 
    {
      void OnPropertyChanged(string propertyName);
    } 
}", Encoding.UTF8);
            context.AddSource("INPCGenerationAttributes.cs", sourceText);
        }
    }
}