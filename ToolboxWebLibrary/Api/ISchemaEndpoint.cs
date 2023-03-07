namespace ToolboxWebLibrary.Api
{
    public interface ISchemaEndpoint
    {
        Task<List<SchemaModel>> GetAllSchemas();
        Task<List<SchemaModel>> GetAllSchemasByDatabaseId(int databaseId);
    }
}