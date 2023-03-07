using ToolboxWebApiLibrary.Helpers;
using ToolboxWebApiLibrary.Internal.DataAccess;

namespace ToolboxWebApiLibrary.DataAccess;

public class TableData : ITableData
{
    private ISqlDataAccess _sqlDataAccess;
    private IConverterHelper _converterHelper;

    public TableData(ISqlDataAccess sqlDataAccess, IConverterHelper converterHelper)
    {
        _sqlDataAccess = sqlDataAccess;
        _converterHelper = converterHelper;
    }

    /// <summary>
    /// This method gets a list of all known existing tables from the ToolboxApp database.
    /// </summary>
    /// <returns>A list of TableModel objects.</returns>
    public async Task<List<TableModel>> GetAllTables()
    {
        var output = await _sqlDataAccess.LoadData<TableModel, dynamic>("dbo.spTables_GetAll", new { }, "ToolboxData");

        return output;
    }


    /// <summary>
    /// Gets a list of TableModel objects from the ToolboxApp database which have a matching <c>schemaId</c>.
    /// </summary>
    /// <param name="schemaId">The <c>INT</c> schema id value that houses the desired tables.</param>
    /// <returns>A list of TableModel objects.</returns>
    public async Task<List<TableModel>> GetAllTablesBySchemaId(int schemaId)
    {
        var output = await _sqlDataAccess.LoadData<TableModel, dynamic>("dbo.spTables_GetBySchemaId", schemaId, "ToolboxData");

        return output;
    }


    /// <summary>
    /// Gets a list of TableModel objects from the ToolboxApp database which have a matching <c>schemaId</c>.
    /// </summary>
    /// <param name="schemaId">The <c>INT</c> schema id value that houses the desired tables.</param>
    /// <returns>A list of TableModel objects.</returns>
    public async Task<TableDetailedModel> GetTableDetailsByTableId(int TableId)
    {
        var parameters = new { tableId = TableId };
        var output = await _sqlDataAccess.LoadData<TableDetailedModel, dynamic>("dbo.spTables_GetTableDetailsByTableId", parameters, "ToolboxData");

        return output.FirstOrDefault();
    }


    /// <summary>
    /// Gets a table model from the ToolboxApp database based on matching entered parameters.
    /// </summary>
    /// <param name="serverName">The table's server name.</param>
    /// <param name="databaseName">The table's database name.</param>
    /// <param name="schemaName">The table's schema name.</param>
    /// <param name="tableName">The table's name.</param>
    /// <returns>A TableModel if the table is known and it exists.</returns>
    public async Task<List<TableModel>> GetTableByNames(string serverName, string databaseName, string schemaName, string tableName)
    {
        Console.WriteLine("Entering TableData.GetTableByNames(string ServerName, string DatabaseName, string SchemaName, string tableName)");

        var parameters = new { ServerName = serverName, DatabaseName = databaseName, SchemaName = schemaName, TableName = tableName };
        var output = await _sqlDataAccess.LoadData<TableModel, dynamic>("dbo.spTables_GetByNames", parameters, "ToolboxData");

        return output;
    }



    public Task<List<TableModel>> FindAllTables(string ServerName, string DatabaseName, string SchemaName)
    {
        Console.WriteLine("Entering TableData.FindAllTables(string ServerName, string DatabaseName, string SchemaName)");

        string ConnectionString = _sqlDataAccess.GetConnectionString(ServerName, DatabaseName);
        List<TableModel> Tables = new List<TableModel>();

        using (SqlConnection Connection = new SqlConnection(ConnectionString))
        {
            Connection.Open();
            string SqlScript = $@"
                USE {DatabaseName};
                SELECT DISTINCT TABLE_NAME AS table_name
                FROM INFORMATION_SCHEMA.COLUMNS
                WHERE TABLE_CATALOG = '{DatabaseName}'
                    AND TABLE_SCHEMA = '{SchemaName}'";

            using (SqlCommand Command = new SqlCommand(SqlScript, Connection))
            {

                using (SqlDataReader Reader = Command.ExecuteReader())
                {

                    if (Reader.HasRows)
                    {

                        while (Reader.Read())
                        {

                            TableModel Table = new TableModel();
                            Table.TableName = Reader.GetString(Reader.GetOrdinal("table_name"));

                            Console.WriteLine(Table.TableName);
                            Tables.Add(Table);

                        }
                    }
                }
            }
        }
        return Task.FromResult(Tables);

    }



    public async Task TableUpsert(List<TableModel> tables, int SchemaId)
    {
        Console.WriteLine("Entering TableData.TableUpsert(List<TableModel> tables)");

        try
        {
            var parameters = new { tables = _converterHelper.ConvertModelToDataTable<TableModel>(tables), schemaId = SchemaId };
            await _sqlDataAccess.SaveData<dynamic, DataTable>("dbo.spTables_Upsert", parameters, "ToolboxData");

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception encountered in TableUpsert: {ex.Message}");

        }
    }
}
