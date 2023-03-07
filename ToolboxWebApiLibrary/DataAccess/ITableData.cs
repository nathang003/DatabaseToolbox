namespace ToolboxWebApiLibrary.DataAccess;

public interface ITableData
{
    Task<List<TableModel>> GetAllTables();
    Task<List<TableModel>> GetAllTablesBySchemaId(int schemaId);
    Task<TableDetailedModel> GetTableDetailsByTableId(int schemaId);
    Task<List<TableModel>> GetTableByNames(string serverName, string databaseName, string schemaName, string tableName);
    Task<List<TableModel>> FindAllTables(string ServerName, string DatabaseName, string SchemaName);
    Task TableUpsert(List<TableModel> tables, int SchemaId);
}