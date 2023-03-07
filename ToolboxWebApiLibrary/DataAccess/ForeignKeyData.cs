using System.Runtime.CompilerServices;

using ToolboxWebApiLibrary.Helpers;
using ToolboxWebApiLibrary.Internal.DataAccess;

namespace ToolboxWebApiLibrary.DataAccess;

public class ForeignKeyData : IForeignKeyData
{
    private ISqlDataAccess _sqlDataAccess;
    private ISchemaData _schemaData;
    private ITableData _tableData;
    private IFieldData _fieldData;
    private IConverterHelper _converterHelper;

    public ForeignKeyData(ISqlDataAccess sqlDataAccess, ISchemaData schemaData, ITableData tableData, IFieldData fieldData, IConverterHelper converterHelper)
    {
        _sqlDataAccess = sqlDataAccess;
        _schemaData = schemaData;
        _tableData = tableData;
        _fieldData = fieldData;
        _converterHelper = converterHelper;
    }


    public Task<List<ForeignKeyModel>> GetAllForeignKeys()
    {
        var output = _sqlDataAccess.LoadData<ForeignKeyModel, dynamic>("dbo.spForeignKeys_GetAll", new { }, "ToolboxData");

        return output;
    }



    public Task<List<ForeignKeyDetailedModel>> FindAllForeignKeys(TableDetailedModel table)
    {
        Console.WriteLine($"Entering ForeignKeyData.FindAllForeignKeys(TableDetailedModel table)");

        string ConnectionString = _sqlDataAccess.GetConnectionString(table.ServerName, table.DatabaseName);
        List<ForeignKeyDetailedModel> foreignKeys = new List<ForeignKeyDetailedModel>();

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
                        f.[name] as ForeignKeyName,
                        OBJECT_SCHEMA_NAME(f.parent_object_id) AS ConstraintSchemaName,
                        OBJECT_NAME(f.parent_object_id) AS ConstraintTableName,
                        COL_NAME(fc.parent_object_id, fc.parent_column_id) AS ConstraintFieldName,
                        OBJECT_SCHEMA_NAME(f.referenced_object_id) AS ReferencedSchemaName,
                        OBJECT_NAME (f.referenced_object_id) AS ReferencedTableName,
                        COL_NAME(fc.referenced_object_id, fc.referenced_column_id) AS ReferencedFieldName,
                        f.is_disabled AS IsDisabled,
                        f.is_not_trusted AS IsNotTrusted,
                        f.delete_referential_action_desc AS DeleteReferentialActionDescription,
                        f.update_referential_action_desc AS UpdateReferentialActionDescription,
                        GETUTCDATE() AS CreatedDate,
                        SUSER_SNAME() AS CreatedBy,
                        GETUTCDATE() AS UpdatedDate,
                        SUSER_SNAME() AS UpdatedBy
                    FROM sys.foreign_keys AS f  
                    INNER JOIN sys.foreign_key_columns AS fc   
                       ON f.object_id = fc.constraint_object_id   
                    WHERE f.parent_object_id = OBJECT_ID('eods.plan');";

                using (SqlCommand Command = new SqlCommand(SqlScript, Connection))
                {

                    using (SqlDataReader Reader = Command.ExecuteReader())
                    {

                        if (Reader.HasRows)
                        {

                            while (Reader.Read())
                            {

                                ForeignKeyDetailedModel foreignKey = new ForeignKeyDetailedModel();

                                Console.WriteLine($"ForeignKeyData.FindAllForeignKeys assigning foreignKey.ServerId: {table.ServerId}");
                                foreignKey.ServerId = table.ServerId;

                                Console.WriteLine($"ForeignKeyData.FindAllForeignKeys assigning foreignKey.ServerName: {table.ServerName}");
                                foreignKey.ServerName = table.ServerName;

                                Console.WriteLine($"ForeignKeyData.FindAllForeignKeys assigning foreignKey.DatabaseId: {table.DatabaseId}");
                                foreignKey.DatabaseId = table.DatabaseId;

                                Console.WriteLine($"ForeignKeyData.FindAllForeignKeys assigning foreignKey.DatabaseName: {table.DatabaseName}");
                                foreignKey.DatabaseName = table.DatabaseName;

                                Console.WriteLine($"ForeignKeyData.FindAllForeignKeys assigning foreignKey.SchemaId: {table.SchemaId}");
                                foreignKey.SchemaId = table.SchemaId;

                                Console.WriteLine($"ForeignKeyData.FindAllForeignKeys assigning foreignKey.SchemaName: {table.SchemaName}");
                                foreignKey.SchemaName = table.SchemaName;

                                Console.WriteLine($"ForeignKeyData.FindAllForeignKeys assigning foreignKey.TableId: {table.TableId}");
                                foreignKey.TableId = table.TableId;

                                Console.WriteLine($"ForeignKeyData.FindAllForeignKeys assigning foreignKey.TableName: {table.TableName}");
                                foreignKey.TableName = table.TableName;

                                Console.WriteLine($"ForeignKeyData.FindAllForeignKeys assigning foreignKey.ForeignKeyName: {Reader["ForeignKeyName"].ToString() ?? string.Empty}");
                                foreignKey.ForeignKeyName = Reader["ForeignKeyName"].ToString() ?? string.Empty;

                                Console.WriteLine($"ForeignKeyData.FindAllForeignKeys assigning foreignKey.ConstraintSchemaId: {table.SchemaId}");
                                foreignKey.ConstraintSchemaId = table.SchemaId;

                                Console.WriteLine($"ForeignKeyData.FindAllForeignKeys assigning foreignKey.ConstraintSchemaName: {table.SchemaName}");
                                foreignKey.ConstraintSchemaName = table.SchemaName;

                                Console.WriteLine($"ForeignKeyData.FindAllForeignKeys assigning foreignKey.ConstraintTableId: {table.TableId}");
                                foreignKey.ConstraintTableId = table.TableId;

                                Console.WriteLine($"ForeignKeyData.FindAllForeignKeys assigning foreignKey.ConstraintTableName: {Reader["ConstraintTableName"].ToString() ?? string.Empty}");
                                foreignKey.ConstraintTableName = table.TableName;

                                Console.WriteLine($"ForeignKeyData.FindAllForeignKeys assigning foreignKey.ConstraintFieldId for {Reader["ConstraintFieldName"].ToString() ?? string.Empty}");
                                foreignKey.ConstraintFieldId = _fieldData.GetFieldIdByNames(table.SchemaName, table.DatabaseName, table.SchemaName, table.TableName, Reader["ConstraintFieldName"].ToString()).Result;

                                Console.WriteLine($"ForeignKeyData.FindAllForeignKeys assigning foreignKey.ConstraintFieldName: {Reader["ConstraintFieldName"].ToString() ?? string.Empty}");
                                foreignKey.ConstraintFieldName = Reader["ConstraintFieldName"].ToString() ?? string.Empty;

                                Console.WriteLine($"ForeignKeyData.FindAllForeignKeys assigning foreignKey.ReferencedSchemaId for {Reader["ReferencedSchemaName"].ToString() ?? string.Empty}");
                                foreignKey.ReferencedSchemaId = _schemaData.GetSchemaIdByNames(table.ServerName, table.DatabaseName, Reader["ReferencedSchemaName"].ToString()).Result;

                                Console.WriteLine($"ForeignKeyData.FindAllForeignKeys assigning foreignKey.ReferencedSchemaName: {Reader["ReferencedSchemaName"].ToString() ?? string.Empty}");
                                foreignKey.ReferencedSchemaName = Reader["ReferencedSchemaName"].ToString() ?? string.Empty;

                                Console.WriteLine($"ForeignKeyData.FindAllForeignKeys assigning foreignKey.ReferencedTableId for {Reader["ReferencedTableName"].ToString() ?? string.Empty}");
                                foreignKey.ReferencedTableId = _tableData.GetTableByNames(table.ServerName, table.DatabaseName, Reader["ReferencedSchemaName"].ToString(), Reader["ReferencedTableName"].ToString()).Result.FirstOrDefault().TableId;

                                Console.WriteLine($"ForeignKeyData.FindAllForeignKeys assigning foreignKey.ReferencedTableName: {Reader["ReferencedTableName"].ToString() ?? string.Empty}");
                                foreignKey.ReferencedTableName = Reader["ReferencedTableName"].ToString() ?? string.Empty;

                                Console.WriteLine($"ForeignKeyData.FindAllForeignKeys assigning foreignKey.ReferencedFieldId for {Reader["ReferencedFieldName"].ToString() ?? string.Empty}");
                                foreignKey.ReferencedFieldId = _fieldData.GetFieldIdByNames(table.SchemaName, table.DatabaseName, Reader["ReferencedSchemaName"].ToString(), Reader["ReferencedTableName"].ToString(), Reader["ReferencedFieldName"].ToString()).Result;

                                Console.WriteLine($"ForeignKeyData.FindAllForeignKeys assigning foreignKey.ReferencedFieldName: {Reader["ReferencedFieldName"].ToString() ?? string.Empty}");
                                foreignKey.ReferencedFieldName = Reader["ReferencedFieldName"].ToString() ?? string.Empty;

                                Console.WriteLine($"ForeignKeyData.FindAllForeignKeys assigning foreignKey.IsDisabled: {(bool)Reader["IsDisabled"]}");
                                foreignKey.IsDisabled = (bool)Reader["IsDisabled"];

                                Console.WriteLine($"ForeignKeyData.FindAllForeignKeys assigning foreignKey.IsNotTrusted: {(bool)Reader["IsNotTrusted"]}");
                                foreignKey.IsNotTrusted = (bool)Reader["IsNotTrusted"];

                                Console.WriteLine($"ForeignKeyData.FindAllForeignKeys assigning foreignKey.DeleteReferentialActionDescription: {Reader["DeleteReferentialActionDescription"].ToString() ?? String.Empty}");
                                foreignKey.DeleteReferentialActionDescription = Reader["DeleteReferentialActionDescription"].ToString() ?? string.Empty;

                                Console.WriteLine($"ForeignKeyData.FindAllForeignKeys assigning foreignKey.UpdateReferentialActionDescription: {Reader["UpdateReferentialActionDescription"].ToString() ?? String.Empty}");
                                foreignKey.UpdateReferentialActionDescription = Reader["UpdateReferentialActionDescription"].ToString() ?? string.Empty;

                                Console.WriteLine($"ForeignKeyData.FindAllForeignKeys assigning foreignKey.CreatedDate: {Reader.GetDateTime("CreatedDate")}");
                                foreignKey.CreatedDate = Reader.GetDateTime("CreatedDate");

                                Console.WriteLine($"ForeignKeyData.FindAllForeignKeys assigning foreignKey.CreatedBy: {Reader["CreatedBy"].ToString() ?? string.Empty}");
                                foreignKey.CreatedBy = Reader["CreatedBy"].ToString() ?? string.Empty;

                                Console.WriteLine($"ForeignKeyData.FindAllForeignKeys assigning foreignKey.UpdatedDate: {Reader.GetDateTime("UpdatedDate")}");
                                foreignKey.UpdatedDate = Reader.GetDateTime("UpdatedDate");

                                Console.WriteLine($"ForeignKeyData.FindAllForeignKeys assigning foreignKey.UpdatedBy: {Reader["UpdatedBy"].ToString() ?? string.Empty}");
                                foreignKey.UpdatedBy = Reader["UpdatedBy"].ToString() ?? string.Empty;

                                Console.WriteLine($"ForeignKeyData.FindAllForeignKeys adding foreign key: {foreignKey.ForeignKeyName}");
                                foreignKeys.Add(foreignKey);

                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"FindAllConstraints had a runtime exception: {ex.Message}");
        }

        return Task.FromResult(foreignKeys);
    }



    public async Task ForeignKeyUpsert(List<ForeignKeyDetailedModel> ForeignKeys, int TableId)
    {
        Console.WriteLine("Entering ForeignKeyData.ForeignKeyUpsert(List<ForeignKeyDetailedModel> ForeignKeys, int TableId)");

        try
        {

            var parameters = new { foreignKeys = _converterHelper.ConvertModelToDataTable<ForeignKeyDetailedModel>(ForeignKeys), tableId = TableId };
            await _sqlDataAccess.SaveData<dynamic, DataTable>("dbo.spForeignKeys_Upsert", parameters, "ToolboxData");

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception encountered in ForeignKeyData.ForeignKeyUpsert: {ex.Message}");

        }
    }
}
