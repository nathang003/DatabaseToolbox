namespace ToolboxWebLibrary.Api
{
    public interface ITableEndpoint
    {
        Task<List<TableModel>> GetAllTables();
        Task<List<TableModel>> GetAllTablesBySchemaId(int schemaId);
        Task<List<TableModel>> GetTableByNames(string serverName, string databaseName, string schemaName, string tableName);
    }
}