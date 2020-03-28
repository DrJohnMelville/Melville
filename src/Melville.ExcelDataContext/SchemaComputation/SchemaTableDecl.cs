using System.Linq;
using System.Text;
using LINQPad.Extensibility.DataContext;

namespace Melville.ExcelDataContext.SchemaComputation
{
    public class SchemaTableDecl
    {
        private string Name { get; }
        private SchemaFieldDecl[] Fields {get;}

        public SchemaTableDecl(string name, SchemaFieldDecl[] fields)
        {
            Name = name;
            Fields = fields;
        }
        public ExplorerItem SchemaDisplay() =>
            new ExplorerItem(Name, ExplorerItemKind.QueryableObject, ExplorerIcon.Table)
            {
                Children = Fields.Select(i=>i.SchemaDisplay()).ToList()
            };

        public void GenerateObjectCode(StringBuilder code)
        { 
            code.AppendFormat("public class {0}Row_gen: Melville.ExcelDataContext.DataContextBases.DataTableBase {{\r\n", Name);
            foreach (var field in Fields)
            {
                field.GenerateFieldDecl(code);
            }
            GenerateConstructor(code);
            code.Append("}\r\n");
        }

        private void GenerateConstructor(StringBuilder code)
        { 
            code.AppendFormat("public {0}Row_gen(System.Collections.Generic.IList<string> obtuseRowName23456) {{\r\n", Name);
            int position = 0;
            foreach (var field in Fields)
            {
                var innerxpr = $"obtuseRowName23456[{position++}]";
                code.AppendFormat("{0} = {1};\r\n", field.Name, field.Type.ParsingStatement(innerxpr));
            }
            code.Append("}");
        }

        public void AppendTableDecl(StringBuilder code)
        {
            code.AppendFormat("public System.Collections.Generic.IList<{0}Row_gen> {0} {{get; private set;}}\r\n", Name);
        }

        public void AppendTableConstructor(StringBuilder code)
        {
            code.AppendFormat("{0} = new System.Collections.Generic.List<{0}Row_gen>(obtuseArgName986y2e.Tables[\"{0}\"].Count);\r\n", Name);
            code.AppendFormat(
                "foreach (var localRow0973w290 in obtuseArgName986y2e.Tables[\"{0}\"].Skip(1)) {0}.Add(new {0}Row_gen(localRow0973w290));\r\n", Name);
        }
    }
}