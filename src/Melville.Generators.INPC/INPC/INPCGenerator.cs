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
        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(() => new INPCReceiver());
        }

        public void Execute(GeneratorExecutionContext context)
        {
            try
            {
                InjectINPCProperties(context);
            }
            catch (Exception e)
            {
                context.ReportDiagnostic(Diagnostic.Create(new DiagnosticDescriptor("INPCGen0003",
                        "Exception Thrown", e.Message + e.StackTrace, "Fatal", DiagnosticSeverity.Error, true),
                    Location.None));
            }
        }

        private void InjectINPCProperties(GeneratorExecutionContext context)
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
    }
}