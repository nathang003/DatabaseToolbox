using System.Data;

using ToolboxWebApiLibrary.Helpers;
using ToolboxWebApiLibrary.Internal.DataAccess;

namespace ToolboxWebApiLibrary.DataAccess;

public class ConstraintData : IConstraintData
{
    private ISqlDataAccess _sqlDataAccess;
    public IScrapeData _scrapeData { get; set; }
    private IFieldData _fieldData;
    private IConverterHelper _converterHelper;

    public ConstraintData(ISqlDataAccess sqlDataAccess, IFieldData fieldData, IConverterHelper converterHelper)
    {
        _sqlDataAccess = sqlDataAccess;
        //_scrapeData = scrapeData;
        _fieldData = fieldData;
        _converterHelper = converterHelper;
    }


    public Task<List<ConstraintModel>> GetAllConstraints()
    {
        var output = _sqlDataAccess.LoadData<ConstraintModel, dynamic>("dbo.spConstraints_GetAll", new { }, "ToolboxData");
        List<ConstraintModel> constraints = output.Result;

        var output2 = _sqlDataAccess.LoadData<ConstraintFieldModel, dynamic>("dbo.spConstraintFields_GetAll", new { }, "ToolboxData");
        List<ConstraintFieldModel> constraintFields = output2.Result;

        foreach (ConstraintModel constraint in constraints)
        {
            constraint.Fields = constraintFields.Where(f => f.ConstraintId == constraint.ConstraintId).ToArray();
        }

        return Task.FromResult<List<ConstraintModel>>(constraints);
    }

    public async Task<List<ConstraintDetailedModel>> GetConstraintsByTableId(int tableId)
    {
        var parameters = new { TableId = tableId };
        var output = _sqlDataAccess.LoadData<ConstraintDetailedModel, dynamic>("dbo.spConstraints_GetByTableId", parameters, "ToolboxData").Result;

        return output;
    }

    public async Task AddConstraint(ConstraintModel constraint)
    {
        var parameters = new { 
            serverId = constraint.ServerId, 
            databaseId = constraint.DatabaseId,
            schemaId = constraint.SchemaId,
            tableId = constraint.TableId,
            constraintName = constraint.ConstraintName,
            constraintDefinition = constraint.ConstraintDefinition,
            purpose = constraint.Purpose,
            constraintType = constraint.ConstraintType,
            constraintTypeDefinition = constraint.ConstraintTypeDescription,
            isSystemName = constraint.IsSystemNamed
        };
        await _sqlDataAccess.SaveData<dynamic, ConstraintModel>("dbo.spConstraints_Insert", parameters, "ToolboxData");
    }

    public async Task DeleteConstraint(ConstraintModel constraint)
    {
        var parameters = new
        {
            constraintId = constraint.ConstraintId
        };
        await _sqlDataAccess.SaveData<dynamic, ConstraintModel>("dbo.spConstraints_Delete", parameters, "ToolboxData");
    }

    public async Task UpdateConstraint(ConstraintModel constraint)
    {
        var parameters = new
        {
            constraintId = constraint.ConstraintId,
            constraintName = constraint.ConstraintName,
            constraintDefinition = constraint.ConstraintDefinition,
            constraintType = constraint.ConstraintType,
            constraintTypeDefinition = constraint.ConstraintTypeDescription,
            isSystemName = constraint.IsSystemNamed
        };
        await _sqlDataAccess.SaveData<dynamic, ConstraintModel>("dbo.spConstraints_Update", parameters, "ToolboxData");
    }



    public async Task<List<ConstraintFieldModel>> GetAllConstraintFields()
    {
        var output = _sqlDataAccess.LoadData<ConstraintFieldModel, dynamic>("dbo.spConstraintFields_GetAll", new { }, "ToolboxData");

        return output.Result;
    }

    public async Task<List<ConstraintFieldModel>> GetConstraintFieldsByConstraintId( int constraintId )
    {
        var parameters = new
        {
            constraintId = constraintId
        };
        var output = _sqlDataAccess.LoadData<ConstraintFieldModel, dynamic>("dbo.spConstraintFields_GetByConstraintId", parameters, "ToolboxData");

        return output.Result;
    }

    public async Task AddConstraintField(ConstraintFieldModel constraintField)
    {
        var parameters = new
        {
            constraintId = constraintField.ConstraintId,
            fieldId = constraintField.FieldId
        };
        await _sqlDataAccess.SaveData<dynamic, ConstraintModel>("dbo.spConstraintFields_Insert", parameters, "ToolboxData");
    }

    public async Task DeleteConstraintField(ConstraintFieldModel constraintField)
    {
        var parameters = new
        {
            constraintFieldId = constraintField.ConstraintFieldId
        };
        await _sqlDataAccess.SaveData<dynamic, ConstraintModel>("dbo.spConstraintFields_Delete", parameters, "ToolboxData");
    }

    public async Task DeleteConstraintFields(int constraintId)
    {
        var parameters = new
        {
            constraintId = constraintId
        };
        await _sqlDataAccess.SaveData<dynamic, ConstraintModel>("dbo.spConstraintFields_DeleteByConstraintId", parameters, "ToolboxData");
    }



    public Task<List<ConstraintDetailedModel>> FindAllConstraints(TableDetailedModel table)
    {
        Console.WriteLine($"Entering ConstraintData.FindAllConstraints(TableDetailedModel table)");

        string ConnectionString = _sqlDataAccess.GetConnectionString(table.ServerName, table.DatabaseName);
        List<ConstraintDetailedModel> constraints = new List<ConstraintDetailedModel>();

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
                        c.[name] as ConstraintName,
	                    COALESCE(cc.definition, dc.definition) AS ConstraintDefinition,
	                    NULL AS Purpose,
	                    c.[type] AS ConstraintType,
	                    c.[type_desc] AS ConstraintTypeDescription,
	                    COALESCE(cc.is_system_named, dc.is_system_named, kc.is_system_named) AS IsSystemNamed,
	                    GETUTCDATE() AS CreatedDate,
	                    SUSER_SNAME() AS CreatedBy,
	                    GETUTCDATE() AS UpdatedDate,
	                    SUSER_SNAME() AS UpdatedBy
                    FROM sys.objects AS t
	                    INNER JOIN sys.objects AS c
		                    ON c.[type_desc] like '%_CONSTRAINT'
		                    AND t.object_id = c.parent_object_id
	                    LEFT JOIN sys.check_constraints cc
		                    ON cc.object_id = c.object_id
	                    LEFT JOIN sys.default_constraints dc
		                    ON dc.object_id = c.object_id
	                    LEFT JOIN sys.key_constraints kc
		                    ON kc.object_id = c.object_id
                    WHERE t.[name] = '{table.TableName}'
                        AND OBJECT_SCHEMA_NAME(t.object_id) = '{table.SchemaName}';";

                using (SqlCommand Command = new SqlCommand(SqlScript, Connection))
                {

                    using (SqlDataReader Reader = Command.ExecuteReader())
                    {

                        if (Reader.HasRows)
                        {

                            while (Reader.Read())
                            {

                                ConstraintDetailedModel constraint = new ConstraintDetailedModel();

                                Console.WriteLine($"ConstraintData.FindAllConstraints assigning constraint.ServerId: {table.ServerId}");
                                constraint.ServerId = table.ServerId;

                                Console.WriteLine($"ConstraintData.FindAllConstraints assigning constraint.ServerName: {table.ServerName}");
                                constraint.ServerName = table.ServerName;

                                Console.WriteLine($"ConstraintData.FindAllConstraints assigning constraint.DatabaseId: {table.DatabaseId}");
                                constraint.DatabaseId = table.DatabaseId;

                                Console.WriteLine($"ConstraintData.FindAllConstraints assigning constraint.DatabaseName: {table.DatabaseName}");
                                constraint.DatabaseName = table.DatabaseName;

                                Console.WriteLine($"ConstraintData.FindAllConstraints assigning constraint.SchemaId: {table.SchemaId}");
                                constraint.SchemaId = table.SchemaId;

                                Console.WriteLine($"ConstraintData.FindAllConstraints assigning constraint.SchemaName: {table.SchemaName}");
                                constraint.SchemaName = table.SchemaName;

                                Console.WriteLine($"ConstraintData.FindAllConstraints assigning constraint.TableId: {table.TableId}");
                                constraint.TableId = table.TableId;

                                Console.WriteLine($"ConstraintData.FindAllConstraints assigning constraint.TableName: {table.TableName}");
                                constraint.TableName = table.TableName;

                                Console.WriteLine($"ConstraintData.FindAllConstraints assigning constraint.ConstraintName: {Reader["ConstraintName"].ToString() ?? string.Empty}");
                                constraint.ConstraintName = Reader["ConstraintName"].ToString() ?? string.Empty;

                                Console.WriteLine($"ConstraintData.FindAllConstraints assigning constraint.ConstraintDefinition: {Reader["ConstraintDefinition"].ToString() ?? string.Empty}");
                                constraint.ConstraintDefinition = Reader["ConstraintDefinition"].ToString() ?? string.Empty;

                                Console.WriteLine($"ConstraintData.FindAllConstraints assigning constraint.Purpose: {Reader["Purpose"].ToString() ?? string.Empty}");
                                constraint.Purpose = Reader["Purpose"].ToString() ?? string.Empty;

                                Console.WriteLine($"ConstraintData.FindAllConstraints assigning constraint.ConstraintType: {Reader["ConstraintType"].ToString() ?? string.Empty}");
                                constraint.ConstraintType = Reader["ConstraintType"].ToString() ?? string.Empty;

                                Console.WriteLine($"ConstraintData.FindAllConstraints assigning constraint.ConstraintTypeDescription: {Reader["ConstraintTypeDescription"].ToString() ?? string.Empty}");
                                constraint.ConstraintTypeDescription = Reader["ConstraintTypeDescription"].ToString() ?? string.Empty;

                                Console.WriteLine($"ConstraintData.FindAllConstraints assigning constraint.IsSystemNamed: {(bool)Reader["IsSystemNamed"]}");
                                constraint.IsSystemNamed = (bool)Reader["IsSystemNamed"];

                                Console.WriteLine($"ConstraintData.FindAllConstraints assigning constraint.CreatedDate: {Reader.GetDateTime("CreatedDate")}");
                                constraint.CreatedDate = Reader.GetDateTime("CreatedDate");

                                Console.WriteLine($"ConstraintData.FindAllConstraints assigning constraint.CreatedBy: {Reader["CreatedBy"].ToString() ?? string.Empty}");
                                constraint.CreatedBy = Reader["CreatedBy"].ToString() ?? string.Empty;

                                Console.WriteLine($"ConstraintData.FindAllConstraints assigning constraint.UpdatedDate: {Reader.GetDateTime("UpdatedDate")}");
                                constraint.UpdatedDate = Reader.GetDateTime("UpdatedDate");

                                Console.WriteLine($"ConstraintData.FindAllConstraints assigning constraint.UpdatedBy: {Reader["UpdatedBy"].ToString() ?? string.Empty}");
                                constraint.UpdatedBy = Reader["UpdatedBy"].ToString() ?? string.Empty;

                                Console.WriteLine($"ConstraintData.FindAllConstraints adding constraint: {constraint.ConstraintName}");
                                constraints.Add(constraint);

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

        return Task.FromResult(constraints);
    }



    public async Task<List<ConstraintFieldModel>> FindAllConstraintFields(ConstraintDetailedModel constraint)
    {
        Console.WriteLine($"Entering ConstraintData.FindAllConstraintFields(ConstraintDetailedModel constraint)");

        string ConnectionString = _sqlDataAccess.GetConnectionString(constraint.ServerName, constraint.DatabaseName);
        List<ConstraintFieldModel> constraintFields = new List<ConstraintFieldModel>();

        try
        {
            using (SqlConnection Connection = new SqlConnection(ConnectionString))
            {
                Connection.Open();

                string SqlScript = $@"
                    USE {constraint.DatabaseName};
                    SELECT {constraint.ServerId} as ServerId,
                        '{constraint.ServerName}' as ServerName,
                        {constraint.DatabaseId} as DatabaseId,
                        '{constraint.DatabaseName}' as DatabaseName,
                        {constraint.SchemaId} as SchemaId,
                        '{constraint.SchemaName}' as SchemaName,
                        {constraint.TableId} as TableId,
                        '{constraint.TableName}' as TableName,
                        {constraint.ConstraintId} as ConstraintId,
                        '{constraint.ConstraintName}' as ConstraintName,
                        cc.[name] as FieldName,
                        cc.is_nullable as IsNullable,
                        cc.is_ansi_padded as IsAnsiPadded,
                        cc.is_rowguidcol as IsRowGuidColumn,
                        cc.is_identity as IsIdentity,
                        cc.generated_always_type as GeneratedAlwaysType,
                        cc.generated_always_type_desc as GeneratedAlwaysTypeDescription,
                        GETUTCDATE() AS CreatedDate,
                        SUSER_SNAME() AS CreatedBy,
                        GETUTCDATE() AS UpdatedDate,
                        SUSER_SNAME() AS UpdatedBy
                    FROM sys.objects ot
                        INNER JOIN sys.objects oc
                            ON ot.object_id = oc.parent_object_id
                        INNER JOIN sys.sysconstraints c
                            ON oc.object_id = c.constid
                        INNER JOIN sys.columns cc
                            ON cc.object_id = ot.object_id
                            AND cc.column_id = c.colid
                    WHERE oc.[name] = '{constraint.ConstraintName}'
                    UNION
                    SELECT {constraint.ServerId} as ServerId,
                        '{constraint.ServerName}' as ServerName,
                        {constraint.DatabaseId} as DatabaseId,
                        '{constraint.DatabaseName}' as DatabaseName,
                        {constraint.SchemaId} as SchemaId,
                        '{constraint.SchemaName}' as SchemaName,
                        {constraint.TableId} as TableId,
                        '{constraint.TableName}' as TableName,
                        {constraint.ConstraintId} as ConstraintId,
                        '{constraint.ConstraintName}' as ConstraintName,
                        cc.[name] as FieldName,
                        cc.is_nullable as IsNullable,
                        cc.is_ansi_padded as IsAnsiPadded,
                        cc.is_rowguidcol as IsRowGuidColumn,
                        cc.is_identity as IsIdentity,
                        cc.generated_always_type as GeneratedAlwaysType,
                        cc.generated_always_type_desc as GeneratedAlwaysTypeDescription,
                        GETUTCDATE() AS CreatedDate,
                        SUSER_SNAME() AS CreatedBy,
                        GETUTCDATE() AS UpdatedDate,
                        SUSER_SNAME() AS UpdatedBy
					FROM sys.indexes pk
						INNER JOIN sys.index_columns ic
							ON ic.object_id = pk.object_id
							AND ic.index_id = pk.index_id
						INNER JOIN sys.columns cc
							ON pk.object_id = cc.object_id
							AND cc.column_id = ic.column_id
                    WHERE pk.[name] = '{constraint.ConstraintName}';";

                using (SqlCommand Command = new SqlCommand(SqlScript, Connection))
                {

                    Console.WriteLine($"ConstraintData.FindAllConstraintFields assigned to Command: {SqlScript}");

                    using (SqlDataReader Reader = Command.ExecuteReader())
                    {

                        if (Reader.HasRows)
                        {

                            while (Reader.Read())
                            {

                                Console.WriteLine($"ConstraintData.FindAllConstraintFields began reading");

                                ConstraintFieldModel constraintField = new ConstraintFieldModel(constraint);
                                constraintField.FieldId = await _fieldData.GetFieldIdByNames(constraint.ServerName, constraint.DatabaseName, constraint.SchemaName, constraint.TableName, Reader["FieldName"].ToString());

                                // Check if the field was registered yet. If not, the constraint field will get a 0 for field id.
                                if (constraintField.FieldId == 0)
                                {
                                    // Scrape the table for fields so all necessary fields can be applied to the constraint.
                                    ScrapeDetailedModel subScrape = new() {
                                        ScrapeId = Guid.NewGuid(),
                                        DynamicScrapeObject = "Table",
                                        DynamicScrapeObjectId = constraint.TableId,
                                        DynamicScrapeObjectName = constraint.TableName,
                                        ScrapeScope = "Table",
                                        ScrapeStatusId = 2,
                                        ScrapeStatusName = "Scheduled",
                                        ScrapeScheduledDate = DateTime.UtcNow,
                                    };

                                    await _scrapeData.ExecuteTableScrape(subScrape);
                                    constraintField.FieldId = await _fieldData.GetFieldIdByNames(constraint.ServerName, constraint.DatabaseName, constraint.SchemaName, constraint.TableName, Reader["FieldName"].ToString());
                                }

                                constraintField.FieldName = Reader["FieldName"].ToString() ?? string.Empty;
                                constraintField.IsNullable = (bool)Reader["IsNullable"];
                                constraintField.IsAnsiPadded = (bool)Reader["IsAnsiPadded"];
                                constraintField.IsRowGuidColumn = (bool)Reader["IsRowGuidColumn"];
                                constraintField.IsIdentity = (bool)Reader["IsIdentity"];
                                constraintField.GeneratedAlwaysType = Convert.ToInt32(Reader["GeneratedAlwaysType"]);
                                constraintField.GeneratedAlwaysTypeDescription = Reader["GeneratedAlwaysTypeDescription"].ToString() ?? string.Empty;
                                constraintField.CreatedDate = Reader.GetDateTime("CreatedDate");
                                constraintField.CreatedBy = Reader["CreatedBy"].ToString() ?? string.Empty;
                                constraintField.UpdatedDate = Reader.GetDateTime("UpdatedDate");
                                constraintField.UpdatedBy = Reader["UpdatedBy"].ToString() ?? string.Empty;

                                Console.WriteLine($"ConstraintData.FindAllConstraintFields adding constraint field: {constraintField.FieldName}");
                                constraintFields.Add(constraintField);

                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception encountered in ConstraintData.FindAllConstraintFields: {ex.Message}");
            throw ex;
        }

        return constraintFields;
    }



    public async Task ConstraintUpsert(ConstraintModel Constraint, int TableId)
    {
        Console.WriteLine("Entering ConstraintData.ConstraintUpsert(ConstraintModel Constraints, int TableId)");

        List<BasicConstraintModel> basicConstraints = new List<BasicConstraintModel>();
        basicConstraints.Add(Constraint.ToBasicConstraintModel());

        try
        {
            var parameters = new { constraints = _converterHelper.ConvertModelToDataTable<BasicConstraintModel>(basicConstraints), tableId = TableId };
            await _sqlDataAccess.SaveData<dynamic, DataTable>("dbo.spConstraints_Upsert", parameters, "ToolboxData");

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception encountered in ConstraintData.ConstraintUpsert(ConstraintModel Constraints, int TableId): {ex.Message}");

        }
    }


    public async Task ConstraintUpsert(List<ConstraintModel> Constraints, int TableId)
    {
        Console.WriteLine("Entering ConstraintData.ConstraintUpsert(List<ConstraintModel> Constraints, int TableId)");

        List<BasicConstraintModel> basicConstraints = new List<BasicConstraintModel>();
        foreach (var constraint in Constraints)
        {
            basicConstraints.Add(constraint.ToBasicConstraintModel());
        }

        try
        {
            var parameters = new { constraints = _converterHelper.ConvertModelToDataTable<BasicConstraintModel>(basicConstraints), tableId = TableId };
            await _sqlDataAccess.SaveData<dynamic, DataTable>("dbo.spConstraints_Upsert", parameters, "ToolboxData");

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception encountered in ConstraintData.ConstraintUpsert(List<ConstraintModel> Constraints, int TableId): {ex.Message}");

        }
    }


    public async Task ConstraintUpsert(List<ConstraintDetailedModel> Constraints, int TableId)
    {
        Console.WriteLine("Entering ConstraintData.ConstraintUpsert(List<ConstraintModel> Constraints, int TableId)");

        List<BasicConstraintModel> basicConstraints = new List<BasicConstraintModel>();
        foreach (var constraint in Constraints)
        {
            basicConstraints.Add(constraint.ToBasicConstraintModel());
        }

        try
        {
            var parameters = new { constraints = _converterHelper.ConvertModelToDataTable<BasicConstraintModel>(basicConstraints), tableId = TableId };
            await _sqlDataAccess.SaveData<dynamic, DataTable>("dbo.spConstraints_Upsert", parameters, "ToolboxData");

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception encountered in ConstraintData.ConstraintUpsert(List<ConstraintModel> Constraints, int TableId): {ex.Message}");

        }
    }


    public async Task ConstraintFieldsUpsert(List<ConstraintFieldModel> ConstraintFields, int ConstraintId)
    {
        Console.WriteLine("Entering ConstraintData.ConstraintFieldsUpsert(List<ConstraintFieldModel> ConstraintFields, int ConstraintId)");

        try
        {
            var parameters = new { constraintFields = _converterHelper.ConvertModelToDataTable<ConstraintFieldModel>(ConstraintFields), constraintId = ConstraintId };
            await _sqlDataAccess.SaveData<dynamic, DataTable>("dbo.spConstraintFields_Upsert", parameters, "ToolboxData");

            Console.WriteLine("Finished spConstraintFields_Upsert in ConstraintData.ConstraintFieldsUpsert");

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception encountered in ConstraintData.ConstraintFieldsUpsert: {ex.Message}");

        }

        Console.WriteLine("Leaving ConstraintData.ConstraintFieldsUpsert(List<ConstraintFieldModel> ConstraintFields, int ConstraintId)");
    }
}
