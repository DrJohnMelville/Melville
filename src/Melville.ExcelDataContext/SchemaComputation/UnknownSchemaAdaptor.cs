namespace Melville.ExcelDataContext.SchemaComputation
{
    public sealed class UnknownSchemaAdaptor : ISchemaTargetType
    {
        private readonly ISchemaTargetType inner;

        public UnknownSchemaAdaptor(ISchemaTargetType inner)
        {
            this.inner = inner;
        }

        public string TypeName => inner.TypeName+"?";

        public string ParsingStatement(string content)
        {
            var parsing = inner.ParsingStatement(content);
            return $"IsNullOrWhiteSpace({content})?({TypeName})null:{parsing}";
        }
    }
}