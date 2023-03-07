using ToolboxWebApiLibrary.Helpers;
using ToolboxWebApiLibrary.Internal.DataAccess;

namespace ToolboxWebApiLibrary.DataAccess;

public class IndexData : IIndexData
{
    private ISqlDataAccess _sqlDataAccess;
    private IFieldData _fieldData;
    private IConverterHelper _converterHelper;

    public IndexData(ISqlDataAccess sqlDataAccess, IFieldData fieldData, IConverterHelper converterHelper)
    {
        _sqlDataAccess = sqlDataAccess;
        _fieldData = fieldData;
        _converterHelper = converterHelper;
    }


    public Task<List<IndexModel>> GetAllIndexes()
    {
        var output = _sqlDataAccess.LoadData<IndexModel, dynamic>("dbo.spIndexes_GetAll", new { }, "ToolboxData");
        List<IndexModel> indexes = output.Result;

        var output2 = _sqlDataAccess.LoadData<IndexFieldModel, dynamic>("dbo.spIndexFields_GetAll", new { }, "ToolboxData");
        List<IndexFieldModel> indexFields = output2.Result;

        for (int i = 0; i < indexes.Count; i++)
        {

            indexes[i].Fields = indexFields.Where(f => f.IndexId == indexes[i].IndexId).ToArray();
        }

        return Task.FromResult<List<IndexModel>>(indexes);
    }


    public Task<List<IndexModel>> GetIndexesByTableId(int tableId)
    {
        var parameters = new
        {
            TableId = tableId
        };
        var output = _sqlDataAccess.LoadData<IndexModel, dynamic>("dbo.spIndexes_GetByTableId", parameters, "ToolboxData");
        List<IndexModel> indexes = output.Result;

        List<IndexFieldModel> indexFields;
        foreach (IndexModel index in indexes)
        {        
            var parameters2 = new
            {
                IndexId = index.IndexId
            };
            var output2 = _sqlDataAccess.LoadData<IndexFieldModel, dynamic>("dbo.spIndexFields_GetByIndexId", parameters2, "ToolboxData");
            indexFields = output2.Result;

            index.Fields = indexFields.Where(f => f.IndexId == index.IndexId).ToArray();
        }

        return Task.FromResult<List<IndexModel>>(indexes);
    }



    public Task<List<IndexDetailedModel>> FindAllIndexes(TableDetailedModel table)
    {
        Console.WriteLine($"Entering IndexData.FindAllIndexes(TableDetailedModel table)");

        string ConnectionString = _sqlDataAccess.GetConnectionString(table.ServerName, table.DatabaseName);
        List<IndexDetailedModel> indexes = new List<IndexDetailedModel>();

        try
        {
            using (SqlConnection Connection = new SqlConnection(ConnectionString))
            {
                Connection.Open();

                string SqlScript = $@"
                    USE {table.DatabaseName};
                    SELECT {table.ServerId} as ServerId,
                        '{table.ServerName}' as ServerName,
                        {table.DatabaseId} as DatabaseId,
                        '{table.DatabaseName}' as DatabaseName,
                        {table.SchemaId} as SchemaId,
                        '{table.SchemaName}' as SchemaName,
                        {table.TableId} as TableId,
                        '{table.TableName}' as TableName,
                        [name] as IndexName,
                        NULL as Purpose,
                        [type] as IndexType,
                        type_desc as IndexTypeDescription,
                        is_primary_key AS IsPrimaryKey,
                        is_unique_constraint AS IsUniqueConstraint,
                        is_unique AS IsUnique,
                        is_disabled AS IsDisabled,
                        GETUTCDATE() AS CreatedDate,
                        SUSER_SNAME() AS CreatedBy,
                        GETUTCDATE() AS UpdatedDate,
                        SUSER_SNAME() AS UpdatedBy
                    FROM sys.indexes
                    WHERE OBJECT_ID = OBJECT_ID('{table.SchemaName + "." + table.TableName}');";

                using (SqlCommand Command = new SqlCommand(SqlScript, Connection))
                {

                    using (SqlDataReader Reader = Command.ExecuteReader())
                    {

                        if (Reader.HasRows)
                        {

                            while (Reader.Read())
                            {

                                IndexDetailedModel index = new IndexDetailedModel();

                                Console.WriteLine($"IndexData.FindAllIndexes assigning index.ServerId: {table.ServerId}");
                                index.ServerId = table.ServerId;

                                Console.WriteLine($"IndexData.FindAllIndexes assigning index.ServerName: {table.ServerName}");
                                index.ServerName = table.ServerName;

                                Console.WriteLine($"IndexData.FindAllIndexes assigning index.DatabaseId: {table.DatabaseId}");
                                index.DatabaseId = table.DatabaseId;

                                Console.WriteLine($"IndexData.FindAllIndexes assigning index.DatabaseName: {table.DatabaseName}");
                                index.DatabaseName = table.DatabaseName;

                                Console.WriteLine($"IndexData.FindAllIndexes assigning index.SchemaId: {table.SchemaId}");
                                index.SchemaId = table.SchemaId;

                                Console.WriteLine($"IndexData.FindAllIndexes assigning index.SchemaName: {table.SchemaName}");
                                index.SchemaName = table.SchemaName;

                                Console.WriteLine($"IndexData.FindAllIndexes assigning index.TableId: {table.TableId}");
                                index.TableId = table.TableId;

                                Console.WriteLine($"IndexData.FindAllIndexes assigning index.TableName: {table.TableName}");
                                index.TableName = table.TableName;

                                Console.WriteLine($"IndexData.FindAllIndexes assigning index.IndexName: {Reader["IndexName"].ToString() ?? string.Empty}");
                                index.IndexName = Reader["IndexName"].ToString() ?? string.Empty;

                                Console.WriteLine($"IndexData.FindAllIndexes assigning index.Purpose: {Reader["Purpose"].ToString() ?? string.Empty}");
                                index.Purpose = Reader["Purpose"].ToString() ?? string.Empty;

                                Console.WriteLine($"IndexData.FindAllIndexes assigning index.IndexType: {Convert.ToInt32(Reader["IndexType"])}");
                                index.IndexType = Convert.ToInt32(Reader["IndexType"]);

                                Console.WriteLine($"IndexData.FindAllIndexes assigning index.IndexTypeDescription: {Reader["IndexTypeDescription"].ToString() ?? string.Empty}");
                                index.IndexTypeDescription = Reader["IndexTypeDescription"].ToString() ?? string.Empty;

                                Console.WriteLine($"IndexData.FindAllIndexes assigning index.IsPrimaryKey: {(bool)Reader["IsPrimaryKey"]}");
                                index.IsPrimaryKey = (bool)Reader["IsPrimaryKey"];

                                Console.WriteLine($"IndexData.FindAllIndexes assigning index.IsUniqueConstraint: {(bool)Reader["IsUniqueConstraint"]}");
                                index.IsUniqueConstraint = (bool)Reader["IsUniqueConstraint"];

                                Console.WriteLine($"IndexData.FindAllIndexes assigning index.IsUnique: {(bool)Reader["IsUnique"]}");
                                index.IsUnique = (bool)Reader["IsUnique"];

                                Console.WriteLine($"IndexData.FindAllIndexes assigning index.IsDisabled: {(bool)Reader["IsDisabled"]}");
                                index.IsDisabled = (bool)Reader["IsDisabled"];

                                Console.WriteLine($"IndexData.FindAllIndexes assigning index.CreatedDate: {Reader.GetDateTime("CreatedDate")}");
                                index.CreatedDate = Reader.GetDateTime("CreatedDate");

                                Console.WriteLine($"IndexData.FindAllIndexes assigning index.CreatedBy: {Reader["CreatedBy"].ToString() ?? string.Empty}");
                                index.CreatedBy = Reader["CreatedBy"].ToString() ?? string.Empty;

                                Console.WriteLine($"IndexData.FindAllIndexes assigning index.UpdatedDate: {Reader.GetDateTime("UpdatedDate")}");
                                index.UpdatedDate = Reader.GetDateTime("UpdatedDate");

                                Console.WriteLine($"IndexData.FindAllIndexes assigning index.UpdatedBy: {Reader["UpdatedBy"].ToString() ?? string.Empty}");
                                index.UpdatedBy = Reader["UpdatedBy"].ToString() ?? string.Empty;

                                Console.WriteLine($"IndexData.FindAllIndexes adding index: {index.IndexName}");
                                indexes.Add(index);

                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"FindAllIndexes had a runtime exception: {ex.Message}");
        }

        return Task.FromResult(indexes);
    }



    public async Task<List<IndexFieldModel>> FindAllIndexFields(IndexDetailedModel index)
    {
        Console.WriteLine($"Entering IndexData.FindAllIndexFields(IndexDetailedModel index)");

        string ConnectionString = _sqlDataAccess.GetConnectionString(index.ServerName, index.DatabaseName);
        List<IndexFieldModel> indexFields = new List<IndexFieldModel>();

        try
        {
            using (SqlConnection Connection = new SqlConnection(ConnectionString))
            {
                Connection.Open();

                string SqlScript = $@"
                    USE {index.DatabaseName};
                    SELECT {index.ServerId} as ServerId,
                        '{index.ServerName}' as ServerName,
                        {index.DatabaseId} as DatabaseId,
                        '{index.DatabaseName}' as DatabaseName,
                        {index.SchemaId} as SchemaId,
                        '{index.SchemaName}' as SchemaName,
                        {index.TableId} as TableId,
                        '{index.TableName}' as TableName,
                        i.[name] as IndexName,
                        COL_NAME(ic.object_id, ic.column_id) AS FieldName,
                        ic.key_ordinal as KeyOrdinal,
                        ic.partition_ordinal as PartitionOrdinal,
                        ic.is_descending_key as IsDescendingKey,
                        ic.is_included_column AS IsIncludedColumn,
                        GETUTCDATE() AS CreatedDate,
                        SUSER_SNAME() AS CreatedBy,
                        GETUTCDATE() AS UpdatedDate,
                        SUSER_SNAME() AS UpdatedBy
                    FROM sys.indexes i
                        INNER JOIN sys.index_columns ic
                            ON i.object_id = ic.object_id
                            AND i.index_id = ic.index_id
                    WHERE i.is_hypothetical = 0
                        AND i.index_id != 0
                        AND i.[name] = '{index.IndexName}';";

                using (SqlCommand Command = new SqlCommand(SqlScript, Connection))
                {

                    Console.WriteLine($"IndexData.FindAllIndexFields assigned to Command: {SqlScript}");

                    using (SqlDataReader Reader = Command.ExecuteReader())
                    {

                        if (Reader.HasRows)
                        {

                            while (Reader.Read())
                            {

                                Console.WriteLine($"IndexData.FindAllIndexFields began reading");

                                IndexFieldModel indexField = new IndexFieldModel(index);
                                indexField.FieldId = await _fieldData.GetFieldIdByNames(index.ServerName, index.DatabaseName, index.SchemaName, index.TableName, Reader["FieldName"].ToString());
                                indexField.FieldName = Reader["FieldName"].ToString() ?? string.Empty;
                                indexField.IsDescendingKey = (bool)Reader["IsDescendingKey"];
                                indexField.IsIncludedColumn = (bool)Reader["IsIncludedColumn"];
                                indexField.KeyOrdinal = Convert.ToInt32(Reader["KeyOrdinal"]);
                                indexField.PartitionOrdinal = Convert.ToInt32(Reader["PartitionOrdinal"]);
                                indexField.CreatedDate = Reader.GetDateTime("CreatedDate");
                                indexField.CreatedBy = Reader["CreatedBy"].ToString() ?? string.Empty;
                                indexField.UpdatedDate = Reader.GetDateTime("UpdatedDate");
                                indexField.UpdatedBy = Reader["UpdatedBy"].ToString() ?? string.Empty;

                                Console.WriteLine($"IndexData.FindAllIndexFields adding index field: {indexField.FieldName}");
                                indexFields.Add(indexField);

                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception encountered in IndexData.FindAllIndexFields: {ex.Message}");
            throw ex;
        }

        return indexFields;
    }



    public async Task IndexUpsert(IndexModel Indexes, int TableId)
    {
        Console.WriteLine("Entering IndexData.IndexUpsert(IndexModel Indexes, int TableId)");

        List<BasicIndexModel> indexes = new List<BasicIndexModel>();
        indexes.Add(Indexes.ToBasicIndexModel());

        try
        {
            var parameters = new { indexes = _converterHelper.ConvertModelToDataTable<BasicIndexModel>(indexes), tableId = TableId };
            await _sqlDataAccess.SaveData<dynamic, DataTable>("dbo.spIndexes_Upsert", parameters, "ToolboxData");

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception encountered in IndexData.IndexUpsert(IndexModel Indexes, int TableId): {ex.Message}");

        }
    }



    public async Task IndexUpsert(List<IndexModel> Indexes, int TableId)
    {
        Console.WriteLine("Entering IndexData.IndexUpsert(List<IndexModel> Indexes, int TableId)");

        List<BasicIndexModel> basicIndexes = new List<BasicIndexModel>();
        foreach (var index in Indexes)
        {
            basicIndexes.Add(index.ToBasicIndexModel());
        }

        try
        {
            var parameters = new { indexes = _converterHelper.ConvertModelToDataTable<BasicIndexModel>(basicIndexes), tableId = TableId };
            await _sqlDataAccess.SaveData<dynamic, DataTable>("dbo.spIndexes_Upsert", parameters, "ToolboxData");

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception encountered in IndexData.IndexUpsert(List<IndexModel> Indexes, int TableId): {ex.Message}");

        }
    }



    public async Task IndexFieldsUpsert(List<IndexFieldModel> IndexFields, int IndexId)
    {
        Console.WriteLine("Entering IndexData.IndexFieldsUpsert(List<IndexFieldModel> IndexFields, int IndexId)");

        try
        {
            var parameters = new { indexFields = _converterHelper.ConvertModelToDataTable<IndexFieldModel>(IndexFields), indexId = IndexId };
            await _sqlDataAccess.SaveData<dynamic, DataTable>("dbo.spIndexFields_Upsert", parameters, "ToolboxData");

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception encountered in IndexData.IndexFieldsUpsert: {ex.Message}");

        }
    }
}
