using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using LINQPad.Extensibility.DataContext;
using Melville.ExcelDataContext.FileReader;
using Melville.ExcelDataContext.SchemaComputation;

namespace Melville.ExcelDataContext
{
    public class TargetObjectBuilder
    {
        public static List<ExplorerItem> GetSchemaAndBuildAssembly(AssemblyName name, string nameSpace, string typeName,
            ISchemaDataSource source)
        {
            BuildAssembly(name, nameSpace, typeName, source);
            return source.GetSchema();
        }

        public static CompilationOutput BuildAssembly(AssemblyName name, string nameSpace, string typeName,
            ISchemaDataSource source) => Compile(source.GetSourceCode(nameSpace, typeName), 
                name.CodeBase??"C:\\");
              static CompilationOutput Compile(string cSharpSourceCode, string outputFile)
        {
            File.WriteAllText(@"C:\Users\jom252\Desktop\pdf\fileOut.txt", cSharpSourceCode);
            var compileResult = DataContextDriver.CompileSource(new CompilationInput
            {
                FilePathsToReference = AssembliesToReference(),
                OutputPath = outputFile,
                SourceCode = new[] {cSharpSourceCode}
            });

            if (compileResult.Errors.Length > 0)
                throw new Exception("Cannot compile typed context: " + compileResult.Errors[0]);

            return compileResult;
        }

              private static string[] AssembliesToReference()
              {
                  return DataContextDriver.GetCoreFxReferenceAssemblies()
                      .Append(typeof(TargetObjectBuilder).Assembly.Location).ToArray();
              }
    }
}