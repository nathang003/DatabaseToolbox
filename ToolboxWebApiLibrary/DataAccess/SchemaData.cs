using ToolboxWebApiLibrary.Internal.DataAccess;
using ToolboxWebApiLibrary.Helpers;

namespace ToolboxWebApiLibrary.DataAccess;

public class SchemaData : ISchemaData
{
    private ISqlDataAccess _sqlDataAccess;
    private IConverterHelper _converterHelper;

    public SchemaData(ISqlDataAccess sqlDataAccess, IConverterHelper converterHelper)
    {
        _sqlDataAccess = sqlDataAccess;
        _converterHelper = converterHelper;
    }


    public async Task<List<SchemaModel>> GetAllSchemas()
    {
        var output = await _sqlDataAccess.LoadData<SchemaModel, dynamic>("dbo.spSchemas_GetAll", new { }, "ToolboxData");

        return output;
    }

    public async Task<List<SchemaModel>> GetAllSchemasByDatabaseId(int databaseId)
    {
        var output = await _sqlDataAccess.LoadData<SchemaModel, dynamic>("dbo.spSchemas_GetByDatabaseId", databaseId, "ToolboxData");

        return output;
    }


    public async Task<SchemaDetailedModel> GetSchemaDetailsBySchemaId(int SchemaId)
    {
        var output = await _sqlDataAccess.LoadData<SchemaDetailedModel, dynamic>("dbo.spSchemas_GetSchemaDetailsBySchemaId", new { schemaId = SchemaId }, "ToolboxData");

        return output.FirstOrDefault();

    }



    public Task<List<SchemaModel>> FindAllSchemas(string ServerName, string DatabaseName)
    {
        string connectionString = _sqlDataAccess.GetConnectionString(ServerName, DatabaseName);
        List<SchemaModel> schemas = new List<SchemaModel>();

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            connection.Open();
            string sqlScript = $@"
                USE {DatabaseName};
                SELECT SCHEMA_NAME AS schema_name
                FROM INFORMATION_SCHEMA.SCHEMATA
                WHERE SCHEMA_NAME NOT LIKE 'db[_]%'
                    AND SCHEMA_NAME NOT IN 
                        (
                            'INFORMATION_SCHEMA'
                            ,'sys'
                            ,'guest'
                        );";

            using (SqlCommand command = new SqlCommand(sqlScript, connection))
            {

                using (SqlDataReader reader = command.ExecuteReader())
                {

                    if (reader.HasRows)
                    {

                        while (reader.Read())
                        {

                            SchemaModel schema = new SchemaModel();                            
                            schema.SchemaName = reader.GetString(reader.GetOrdinal("schema_name"));

                            Console.WriteLine(schema.SchemaName);
                            schemas.Add(schema);

                        }
                    }
                }
            }
        }
        return Task.FromResult(schemas);

    }



    public async Task SchemaUpsert(List<SchemaModel> schemas, int DatabaseId)
    {
        Console.WriteLine("Entering SchemaData.SchemaUpsert(List<SchemaModel> schemas)");

        try
        {
            var parameters = new { schemas = _converterHelper.ConvertModelToDataTable<SchemaModel>(schemas), databaseId = DatabaseId };
            await _sqlDataAccess.SaveData<dynamic, DataTable>("dbo.spSchemas_Upsert", parameters, "ToolboxData");

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception encountered in SchemaUpsert: {ex.Message}");

        }
    }


    /// <summary>
    /// Gets a schema id from the ToolboxApp database based on matching entered parameters.
    /// </summary>
    /// <param name="serverName">The table's server name.</param>
    /// <param name="databaseName">The table's database name.</param>
    /// <param name="schemaName">The table's schema name.</param>
    /// <returns>A TableModel if the table is known and it exists.</returns>
    public async Task<int> GetSchemaIdByNames(string ServerName, string DatabaseName, string SchemaName)
    {
        Console.WriteLine("Entering SchemaData.GetSchemaIdByNames(string ServerName, string DatabaseName, string SchemaName)");

        var parameters = new { serverName = ServerName, databaseName = DatabaseName, schemaName = SchemaName };
        var output = await _sqlDataAccess.LoadData<int, dynamic>("dbo.spSchemas_GetIdByNames", parameters, "ToolboxData");

        return output.FirstOrDefault();
    }
}
