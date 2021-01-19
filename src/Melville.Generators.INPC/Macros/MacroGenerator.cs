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
    public class UdpConsole
    {
        private static UdpClient? client = null;
        private static UdpClient Client
        {
            get
            {
                client ??= new UdpClient();
                return client ;
            }
        }

        public static void WriteLine(string str)
        {
            var bytes = Encoding.UTF8.GetBytes(str);
            Client.Send(bytes, bytes.Length, "127.0.0.1", 15321);
        }
    }
    [Generator]
    public class MacroGenerator: ISourceGenerator
    {
        public void Initialize(GeneratorInitializationContext context)
        {
            context.RegisterForSyntaxNotifications(()=>new MacroSyntaxReceiver());
        }

        public void Execute(GeneratorExecutionContext context)
        {
            UdpConsole.WriteLine("Inside Generator");
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

namespace Melville.MaroGen
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