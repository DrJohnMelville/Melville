namespace Melville.ExcelDataContext.SchemaComputation
{
    public sealed class SchemaTargetType: ISchemaTargetType
    {
        public string TypeName { get; }
        private readonly string parsingStatementTemplate;
        public string ParsingStatement(string content) => string.Format(parsingStatementTemplate, content);

        public SchemaTargetType(string parsingStatementTemplate, string typeName)
        {
            this.parsingStatementTemplate = parsingStatementTemplate;
            TypeName = typeName;
        }

        public static readonly SchemaTargetType String = new SchemaTargetType("{0}", "string");
        public static readonly SchemaTargetType Int = new SchemaTargetType("ParseInt({0})", "int");
        public static readonly SchemaTargetType Double = 
            new SchemaTargetType("ParseDouble({0})", "double");
        public static readonly SchemaTargetType Date = 
            new SchemaTargetType("ParseDateTime({0})", "System.DateTime");
        public static readonly SchemaTargetType Bool = new SchemaTargetType("ParseBoolean({0})", "bool");
    }
}