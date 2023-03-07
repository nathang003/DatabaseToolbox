using ToolboxWebApiLibrary.Internal.DataAccess;
using ToolboxWebApiLibrary.DataAccess;
using ToolboxWebAppLibrary.Models;

namespace ToolboxWebApiLibrary.DataAccess;

public class ScrapeData : IScrapeData
{
    private ISqlDataAccess _sqlDataAccess;
    private IServerData _serverData;
    private IDatabaseData _databaseData;
    private ISchemaData _schemaData;
    private ITableData _tableData;
    private IFieldData _fieldData;
    private IIndexData _indexData;
    private IConstraintData _constraintData;
    private IForeignKeyData _foreignKeyData;

    public ScrapeData(ISqlDataAccess SqlDataAccess, IServerData ServerData, IDatabaseData DatabaseData, 
        ISchemaData SchemaData, ITableData TableData, IFieldData fieldData, IIndexData indexData, IConstraintData constraintData, IForeignKeyData foreignKeyData)
    {
        _sqlDataAccess = SqlDataAccess;
        _serverData = ServerData;
        _databaseData = DatabaseData;
        _schemaData = SchemaData;
        _tableData = TableData;
        _fieldData = fieldData;
        _indexData = indexData;
        _constraintData = constraintData;
        _foreignKeyData = foreignKeyData;
    }


    public async Task<List<ScrapeModel>> GetAllScrapes()
    {
        var output = await _sqlDataAccess.LoadData<ScrapeModel, dynamic>("dbo.spScrapes_GetAll", new { }, "ToolboxData");

        return output;
    }


    public async Task<List<ScrapeDetailedModel>> GetAllScrapesDetailed()
    {
        var output = await _sqlDataAccess.LoadData<ScrapeDetailedModel, dynamic>("dbo.spScrapes_GetAllDetailed", new { }, "ToolboxData");

        return output;
    }


    public async Task<List<ScrapeDetailedModel>> GetAllUnassignedScrapes()
    {
        var output = await _sqlDataAccess.LoadData<ScrapeDetailedModel, dynamic>("dbo.spScrapes_GetAllUnassigned", new { }, "ToolboxData");

        return output;
    }


    public async Task<List<ScrapeSuggestionModel>> GetSuggestedScrapes()
    {
        var output = await _sqlDataAccess.LoadData<ScrapeSuggestionModel, dynamic>("dbo.spScrapes_GetSuggestedDetailed", new { }, "ToolboxData");

        return output;
    }


    public async Task<List<ScrapeSuggestionModel>> GetTopSuggestedScrapes()
    {
        var output = await _sqlDataAccess.LoadData<ScrapeSuggestionModel, dynamic>("dbo.spScrapes_GetTopSuggestedDetailed", new { }, "ToolboxData");

        return output;
    }


    public async Task<int> GetSuggestionCount()
    {
        var output = await _sqlDataAccess.LoadData<int, dynamic>("dbo.spScrapes_GetSuggestionCount", new { }, "ToolboxData");

        return output.FirstOrDefault();
    }


    public async Task<ScrapeModel> GetScrapeById(Guid scrapeId)
    {
        var parameters = new { QueuedScrapeId = scrapeId };
        var output = await _sqlDataAccess.LoadData<ScrapeModel, dynamic>("dbo.spScrapes_GetById", parameters, "ToolboxData");

        return output.First();
    }


    public async Task<int> GetUnassignedScrapeCount()
    {
        var output = await _sqlDataAccess.LoadData<int, dynamic>("dbo.spScrapes_GetUnassignedScrapeCount", new { }, "ToolboxData");

        return output.FirstOrDefault();
    }


    /// <summary>
    /// Queues a desired scrape by adding it to the ToolboxApp.dbo.Scrapes table.
    /// </summary>
    /// <param name="scrapeScope">The type of the scrape being executed. (ex. Servers, Databases, Schemas, Tables, Fields, Foreign Keys, Indexes, or Constraints)</param>
    /// <param name="dynamicScrapeObject">The target of the scrape. Note: When the scrape scope is Foreign Keys, Indexes, or Constraints, the target will be 'table'.</param>
    /// <param name="dynamicScrapeObjectId">The id of the scrape target. If a server scrape is being queued, you would use the server_id; if a foreign key scrape is being queued, you would use the table_id.</param>
    /// <returns>Task</returns>
    public async Task AddScrape(string scrapeScope, string dynamicScrapeObject, int dynamicScrapeObjectId, DateTime? scrapeScheduledDateTime)
    {
        Console.WriteLine("Entering ScrapeData.AddScrape(string scrapeScope, string dynamicScrapeObject, int dynamicScrapeObjectId, DateTime? scrapeScheduledDateTime)");

        var parameters = new { scrapeScope = scrapeScope, dynamicScrapeObject = dynamicScrapeObject, dynamicScrapeObjectId = dynamicScrapeObjectId, 
            scrapeScheduledDateTime = scrapeScheduledDateTime ?? DateTime.UtcNow};

        await _sqlDataAccess.SaveData<dynamic, ScrapeModel>("dbo.spScrapes_Insert", parameters, "ToolboxData");
    }


    /// <summary>
    /// Updates a desired scrape in the ToolboxApp.dbo.Scrapes table.
    /// </summary>
    /// <param name="scrape">The detailed model of the scrape being executed. (ex. Servers, Databases, Schemas, Tables, Fields, Foreign Keys, Indexes, or Constraints)</param>
    /// <returns>Task</returns>
    public async Task UpdateScrape(ScrapeDetailedModel scrape)
    {
        Console.WriteLine("Entering ScrapeData.UpdateScrape(ScrapeDetailedModel scrape)");

        var parameters = new { scrapeId = scrape.ScrapeId, scrapeScope = scrape.ScrapeScope, dynamicScrapeObject = scrape.DynamicScrapeObject, 
            dynamicScrapeObjectId = scrape.DynamicScrapeObjectId, scrapeScheduledDateTime = scrape.ScrapeScheduledDate,
            scrapeWorkerId = scrape.ScrapeWorkerId, scrapeStatusId = scrape.ScrapeStatusId};

        await _sqlDataAccess.SaveData<dynamic, ScrapeModel>("dbo.spScrapes_Update", parameters, "ToolboxData");
    }


    /// <summary>
    /// Executes a server scrape by targeting the server name.
    /// </summary>
    /// <param name="scrape">The ScrapeDetailedModel being executed.</param>
    /// <returns>Task</returns>
    public async Task<bool> ExecuteServerScrape(ScrapeDetailedModel scrape)
    {
        var result = _serverData.GetByServerId(scrape.DynamicScrapeObjectId);
        ServerModel? server = result.Result;

        try
        {
            List<DatabaseModel> databases = await _databaseData.FindAllDatabases(server.ServerName);
            foreach (DatabaseModel database in databases)
            {
                database.ServerId = server.ServerId;
            }

            await _databaseData.DatabaseUpsert(databases, scrape.DynamicScrapeObjectId);
            return true;
        }
        catch(Exception ex)
        {
            Console.WriteLine($"ExecuteServerScrape had a runtime exception: {ex.Message}");
            return false;
        }
    }


    /// <summary>
    /// Executes a database scrape by targeting the database name.
    /// </summary>
    /// <param name="scrape">The ScrapeDetailedModel being executed.</param>
    /// <returns>Task</returns>
    public async Task<bool> ExecuteDatabaseScrape(ScrapeDetailedModel scrape)
    {
        var result = await _databaseData.GetDatabaseDetailsByDatabaseId(scrape.DynamicScrapeObjectId);
        DatabaseDetailedModel? database = result;

        try
        {
            List<SchemaModel> schemas = await _schemaData.FindAllSchemas(database.ServerName, database.DatabaseName);

            foreach (SchemaModel schema in schemas)            
            {
                schema.ServerId = database.ServerId;
                schema.DatabaseId = database.DatabaseId;
            }

            await _schemaData.SchemaUpsert(schemas, scrape.DynamicScrapeObjectId);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ExecuteDatabaseScrape had a runtime exception: {ex.Message}");
            return false;
        }
    }


    /// <summary>
    /// Executes a schema scrape by targeting the schema name.
    /// </summary>
    /// <param name="scrape">The ScrapeDetailedModel being executed.</param>
    /// <returns>Task</returns>
    public async Task<bool> ExecuteSchemaScrape(ScrapeDetailedModel scrape)
    {
        var result = await _schemaData.GetSchemaDetailsBySchemaId(scrape.DynamicScrapeObjectId);
        SchemaDetailedModel? schema = result;

        try
        {
            List<TableModel> tables = await _tableData.FindAllTables(schema.ServerName, schema.DatabaseName, schema.SchemaName);

            foreach (TableModel table in tables)
            {
                table.ServerId = schema.ServerId;
                table.DatabaseId = schema.DatabaseId;
                table.SchemaId = schema.SchemaId;
            }

            await _tableData.TableUpsert(tables, scrape.DynamicScrapeObjectId);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ExecuteSchemaScrape had a runtime exception: {ex.Message}");
            return false;
        }
    }


    /// <summary>
    /// Executes a table scrape by targeting the table name.
    /// </summary>
    /// <param name="scrape">The ScrapeDetailedModel being executed.</param>
    /// <returns>Task</returns>
    public async Task<bool> ExecuteTableScrape(ScrapeDetailedModel scrape)
    {
        Console.WriteLine($"Entering ScrapeData.ExecuteTableScrape(ScrapeDetailedModel scrape)");

        TableDetailedModel? table;        

        try
        {
            var result = await _tableData.GetTableDetailsByTableId(scrape.DynamicScrapeObjectId);
            table = result;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ExecuteTableScrape exception while get detailed table data: {ex.Message}");
            return false;
        }

        try
        {
            List<FieldModel> fields = await _fieldData.FindAllFields(table.ServerName, table.DatabaseName, table.SchemaName, table.TableName);

            foreach (FieldModel field in fields)
            {
                field.ServerId = table.ServerId;
                field.DatabaseId = table.DatabaseId;
                field.SchemaId = table.SchemaId;
                field.TableId = table.TableId;
            }

            await _fieldData.FieldsUpsert(fields, scrape.DynamicScrapeObjectId);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ExecuteTableScrape had a runtime exception: {ex.Message}");
            return false;
        }
    }


    /// <summary>
    /// Executes a field value scrape by targeting the field id.
    /// </summary>
    /// <param name="scrape">The ScrapeDetailedModel being executed.</param>
    /// <returns>Task</returns>
    public async Task<bool> ExecuteFieldScrape(ScrapeDetailedModel scrape)
    {
        Console.WriteLine($"Entering ScrapeData.ExecuteFieldScrape(ScrapeDetailedModel scrape)");

        FieldDetailedModel? field;

        try
        {
            var result = await _fieldData.GetFieldDetailsByFieldId(scrape.DynamicScrapeObjectId);
            field = result;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ExecuteFieldScrape exception while getting detailed field data: {ex.Message}");
            return false;
        }

        try
        {
            field = await _fieldData.FindAllValues(field);
            Console.WriteLine($"Completed Field Value scrape for {field.TableName + "." + field.FieldName}");

            await _fieldData.FieldUpdate(field);
            Console.WriteLine($"Completed Field Update for {field.TableName + "." + field.FieldName}");
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ExecuteFieldScrape had a runtime exception: {ex.Message}");
            return false;
        }
    }


    /// <summary>
    /// Executes an index scrape by targeting the table name.
    /// </summary>
    /// <param name="scrape">The ScrapeDetailedModel being executed.</param>
    /// <returns>Task</returns>
    public async Task<bool> ExecuteIndexScrape(ScrapeDetailedModel scrape)
    {
        Console.WriteLine($"Entering ScrapeData.ExecuteIndexScrape(ScrapeDetailedModel scrape)");

        TableDetailedModel? table;

        try
        {
            var result = await _tableData.GetTableDetailsByTableId(scrape.DynamicScrapeObjectId);
            table = result;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ExecuteIndexScrape exception while getting index data: {ex.Message}");
            return false;
        }

        try
        {
            Console.WriteLine($"ExecuteIndexScrape found target table: {table.TableName}");

            List<IndexDetailedModel> indexes = await _indexData.FindAllIndexes(table);

            Console.WriteLine($"FindAllIndexes found {indexes.Count} index(es).");

            foreach (IndexDetailedModel index in indexes)
            {
                await _indexData.IndexUpsert(index.ToIndexModel(), index.TableId);

                List<IndexFieldModel> indexFields = _indexData.FindAllIndexFields(index).Result;
                await _indexData.IndexFieldsUpsert(indexFields, index.IndexId);
            }
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ExecuteIndexScrape had a runtime exception: {ex.Message}");
            return false;
        }
    }


    /// <summary>
    /// Executes a constraint scrape by targeting the table name.
    /// </summary>
    /// <param name="scrape">The ScrapeDetailedModel being executed.</param>
    /// <returns>Task</returns>
    public async Task<bool> ExecuteConstraintScrape(ScrapeDetailedModel scrape)
    {
        Console.WriteLine($"Entering ScrapeData.ExecuteConstraintScrape(ScrapeDetailedModel scrape)");

        TableDetailedModel? table;

        try
        {
            var result = await _tableData.GetTableDetailsByTableId(scrape.DynamicScrapeObjectId);
            table = result;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ExecuteConstraintScrape exception while getting contraint data: {ex.Message}");
            return false;
        }

        try
        {
            Console.WriteLine($"ExecuteConstraintScrape found target table: {table.TableName}");

            List<ConstraintDetailedModel> constraints = await _constraintData.FindAllConstraints(table);

            Console.WriteLine($"FindAllConstraints found {constraints.Count} constraints(s).");

            await _constraintData.ConstraintUpsert(constraints, table.TableId);

            constraints = await _constraintData.GetConstraintsByTableId(table.TableId);

            foreach (ConstraintDetailedModel constraint in constraints)
            {
                List<ConstraintFieldModel> constraintFields = _constraintData.FindAllConstraintFields(constraint).Result;

                Console.WriteLine($"FindAllConstraintFields found {constraintFields.Count} field(s).");
                if (constraintFields.Count > 0)
                {
                    await _constraintData.ConstraintFieldsUpsert(constraintFields, constraint.ConstraintId);
                }
            }
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ExecuteConstraintScrape had a runtime exception: {ex.Message}");
            return false;
        }
    }


    /// <summary>
    /// Executes a foreign key scrape by targeting the table name.
    /// </summary>
    /// <param name="scrape">The ScrapeDetailedModel being executed.</param>
    /// <returns>Task</returns>
    public async Task<bool> ExecuteForeignKeyScrape(ScrapeDetailedModel scrape)
    {
        Console.WriteLine($"Entering ScrapeData.ExecuteForeignKeyScrape(ScrapeDetailedModel scrape)");

        TableDetailedModel? table;

        try
        {
            var result = await _tableData.GetTableDetailsByTableId(scrape.DynamicScrapeObjectId);
            table = result;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ExecuteForeignKeyScrape exception while getting foreign key data: {ex.Message}");
            return false;
        }

        try
        {
            List<ForeignKeyDetailedModel> foreignKeys = await _foreignKeyData.FindAllForeignKeys(table);

            await _foreignKeyData.ForeignKeyUpsert(foreignKeys, table.TableId);
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ExecuteForeignKeyScrape had a runtime exception: {ex.Message}");
            return false;
        }
    }
}
