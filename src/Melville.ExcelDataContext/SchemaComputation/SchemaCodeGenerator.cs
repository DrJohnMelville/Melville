using System.Text;

namespace Melville.ExcelDataContext.SchemaComputation
{
    public static class SchemaCodeGenerator
    {
        public static string GenerateDataObjectSourceCode(this SchemaTableDecl[] decls,
            string nameSpace, string typeName)
        {
            var code = new StringBuilder();
            code.AppendFormat("using System.Linq;\r\nnamespace {0} {{\r\n", nameSpace);
            foreach (var decl in decls)
            {
                decl.GenerateObjectCode(code);
            }
            // create the SummaryObject
            GenerateFacade(decls, typeName, code);
            code.Append("}");
            return code.ToString();
        }

        private static void GenerateFacade(SchemaTableDecl[] decls, string typeName, StringBuilder code)
        {  
            code.AppendFormat("public class {0}:  Melville.ExcelDataContext.DataContextBases.DataContextBase {{\r\n", typeName);
            foreach (var decl in decls)
            {
                decl.AppendTableDecl(code);
            }
            code.AppendFormat("public {0}(Melville.ExcelDataContext.FileReader.RawFileReader obtuseArgName986y2e):base(obtuseArgName986y2e) {{\r\n", typeName);
            foreach (var decl in decls)
            {
                decl.AppendTableConstructor(code);
            }
            code.Append("}");
            code.Append("}");
        }
    }
}