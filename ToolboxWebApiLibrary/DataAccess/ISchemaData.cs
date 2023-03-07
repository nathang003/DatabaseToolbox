
namespace ToolboxWebApiLibrary.DataAccess
{
    public interface ISchemaData
    {
        Task<List<SchemaModel>> GetAllSchemas();
        Task<List<SchemaModel>> GetAllSchemasByDatabaseId(int databaseId);
        Task<SchemaDetailedModel> GetSchemaDetailsBySchemaId(int SchemaId);
        Task<List<SchemaModel>> FindAllSchemas(string serverName, string databaseName);
        Task SchemaUpsert(List<SchemaModel> schemas, int DatabaseId);
        Task<int> GetSchemaIdByNames(string ServerName, string DatabaseName, string SchemaName);
    }
}