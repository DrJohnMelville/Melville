using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using LINQPad.Extensibility.DataContext;
using Melville.ExcelDataContext.SchemaComputation;
using Melville.INPC;

namespace Melville.ExcelDataContext
{
    public readonly partial struct TargetObjectBuilder
    {
        [FromConstructor] private readonly AssemblyName name;
        [FromConstructor] private readonly string nameSpace;
        [FromConstructor] private readonly string typeName;
        [FromConstructor] private readonly ISchemaDataSource source;
        [FromConstructor] private readonly IConnectionInfo connectionInfo;
        public  List<ExplorerItem> GetSchemaAndBuildAssembly()
        {
            BuildAssembly();
            return source.GetSchema();
        }

        private  CompilationOutput BuildAssembly() => Compile(source.GetSourceCode(nameSpace, typeName),
            AppDomain.CurrentDomain.BaseDirectory);

        private CompilationOutput Compile(string cSharpSourceCode, string outputFile)
        {
            var compileResult = DataContextDriver.CompileSource(new CompilationInput
            {
                FilePathsToReference = AssembliesToReference(),
                OutputPath = outputFile,
                SourceCode = new[] { cSharpSourceCode }
            });

            if (compileResult.Errors.Length > 0)
                throw new Exception("Cannot compile typed context: " + compileResult.Errors[0]);

            return compileResult;
        }

        private string[] AssembliesToReference() =>
            DataContextDriver.GetCoreFxReferenceAssemblies(connectionInfo)
                .Append(typeof(TargetObjectBuilder).Assembly.Location).ToArray();
    }
}