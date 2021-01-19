using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using Melville.Generators.Tools.CodeWriters;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Melville.Generators.INPC.Macros
{
    [Generator]
    public class MacroGenerator: ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(()=>new MacroSyntaxReceiver());
        }

        public void Execute(GeneratorExecutionContext context)
        {
            AddAttributes(context);
            if (context.SyntaxReceiver is MacroSyntaxReceiver msr)
                Generate(msr.Requests, context);
        }

        private void Generate(List<MacroRequest> msrRequests, GeneratorExecutionContext context)
        {
            var namer = new GeneratedFileUniqueNamer();
            foreach (var group in msrRequests.GroupBy(i=>i.NodeToEnclose))
            {
                context.RenderInType(namer, group.Key, group.Select(i => i.GeneratedCode));
            }
        }

        private static void AddAttributes(GeneratorExecutionContext context)
        {
            context.AddSource("MacroAttributes.MacroGen.cs", @"
using System;

namespace Melville.MacroGen
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Class |
                    AttributeTargets.Method | AttributeTargets.Field | AttributeTargets.Event | AttributeTargets.Struct, 
        Inherited = false, AllowMultiple = true)]
    internal sealed class MacroCodeAttribute : Attribute
    {
       public object Prefix {get;set;} = """";
       public object Postfix {get;set;} = """";
       public MacroCodeAttribute(object text){}
    }
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property | AttributeTargets.Class |
                    AttributeTargets.Method | AttributeTargets.Field | AttributeTargets.Event | AttributeTargets.Struct, 
        Inherited = false, AllowMultiple = true)]
    internal sealed class MacroItemAttribute : Attribute
    {
        public MacroItemAttribute(params object[] parameters)
        {
        }
    }
}
");
        }
    }
}