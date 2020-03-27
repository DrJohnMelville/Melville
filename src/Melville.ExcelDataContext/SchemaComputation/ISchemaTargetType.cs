namespace Melville.ExcelDataContext.SchemaComputation
{
    public interface ISchemaTargetType
    {
        string TypeName { get; }
        string ParsingStatement(string content);

    }
}