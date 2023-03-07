using ToolboxWebApiLibrary.Helpers;
using ToolboxWebApiLibrary.Internal.DataAccess;

namespace ToolboxWebApiLibrary.DataAccess;

public class FieldData : IFieldData
{
    private ISqlDataAccess _sqlDataAccess;
    private IConverterHelper _converterHelper;

    public FieldData(ISqlDataAccess SqlDataAccess, IConverterHelper ConverterHelper)
    {
        _sqlDataAccess = SqlDataAccess;
        _converterHelper = ConverterHelper;
    }


    public async Task<List<FieldModel>> GetAllFields()
    {
        var output = await _sqlDataAccess.LoadData<FieldModel, dynamic>("dbo.spFields_GetAll", new { }, "ToolboxData");
        return output;
    }


    public async Task<List<FieldModel>> GetAllFieldsByTableId(int tableId)
    {
        var output = await _sqlDataAccess.LoadData<FieldModel, dynamic>("dbo.spFields_GetByTableId @TableId", new { TableId = tableId }, "ToolboxData");
        return output;
    }


    public async Task<int> GetFieldIdByNames(string ServerName, string DatabaseName, string SchemaName, string TableName, string FieldName)
    {
        var parameters = new { serverName = ServerName, databaseName = DatabaseName, schemaName = SchemaName, tableName = TableName, fieldName = FieldName };
        var output = await _sqlDataAccess.LoadData<int, dynamic>("dbo.spFields_GetFieldIdByNames", parameters, "ToolboxData");
        return output.FirstOrDefault();
    }


    /// <summary>
    /// Gets a FieldDetailedModel object from the ToolboxApp database that has a matching <c>FieldId</c>.
    /// </summary>
    /// <param name="FieldId">The <c>INT</c> field id value that houses the desired fields.</param>
    /// <returns>FieldDetailedModel</returns>
    public async Task<FieldDetailedModel> GetFieldDetailsByFieldId(int FieldId)
    {
        var output = await _sqlDataAccess.LoadData<FieldDetailedModel, dynamic>("dbo.spFields_GetFieldDetailsByFieldId", new { fieldId = FieldId }, "ToolboxData");

        return output.FirstOrDefault();
    }



    public Task<List<FieldModel>> FindAllFields(string ServerName, string DatabaseName, string SchemaName, string TableName)
    {
        Console.WriteLine($"Entering FieldData.FindAllFields(string ServerName, string DatabaseName, string SchemaName, string TableName)");

        string ConnectionString = _sqlDataAccess.GetConnectionString(ServerName, DatabaseName);
        List<FieldModel> Fields = new List<FieldModel>();

        using (SqlConnection Connection = new SqlConnection(ConnectionString))
        {
            Connection.Open();
            string SqlScript = $@"
                USE {DatabaseName};
                SELECT COLUMN_NAME AS fieldName,
                    ORDINAL_POSITION AS ordinalNumber,
                    COLUMN_DEFAULT AS defaultValue,
                    IS_NULLABLE AS nullable,
                    DATA_TYPE AS dataType,
                    CHARACTER_MAXIMUM_LENGTH AS characterLength,
                    cast(NUMERIC_PRECISION as int) AS numericPrecision,
                    NUMERIC_SCALE AS numericScale,
                    cast(DATETIME_PRECISION as int) AS datetimePrecision,
                    CHARACTER_SET_NAME AS characterSetName,
                    COLLATION_NAME AS collationName
                FROM INFORMATION_SCHEMA.COLUMNS
                WHERE TABLE_CATALOG = '{DatabaseName}'
                    AND TABLE_SCHEMA = '{SchemaName}'
                    AND TABLE_NAME = '{TableName}';";

            using (SqlCommand Command = new SqlCommand(SqlScript, Connection))
            {

                using (SqlDataReader Reader = Command.ExecuteReader())
                {

                    if (Reader.HasRows)
                    {

                        while (Reader.Read())
                        {

                            FieldModel Field = new FieldModel();
                            
                            Field.FieldName = Reader["fieldName"].ToString() ?? String.Empty;
                            Field.OrdinalNumber = Reader.GetInt32("ordinalNumber");
                            Field.DefaultValue = Reader["defaultValue"].ToString() ?? String.Empty;
                            Field.IsNullable = Reader["nullable"].ToString() == "NO" ? 0 : 1;
                            Field.DataType = Reader["dataType"].ToString() ?? String.Empty;

                            if (Reader["characterLength"] != DBNull.Value)
                            {
                                Field.CharacterLength = Reader.GetInt32("characterLength");
                            }

                            if (Reader["numericPrecision"] != DBNull.Value)
                            {
                                Field.NumericPrecision = Reader.GetInt32("numericPrecision");
                            }

                            if (Reader["numericScale"] != DBNull.Value)
                            {
								Field.NumericScale = Reader.GetInt32("numericScale");
                            }

                            if (Reader["datetimePrecision"] != DBNull.Value)
                            {
                                Field.DateTimePrecision = Reader.GetInt32("datetimePrecision");
                            }

                            Field.CharacterSetName = Reader["characterSetName"].ToString() ?? String.Empty;
                            Field.CollationName = Reader["collationName"].ToString() ?? String.Empty;

                            Console.WriteLine(Field.FieldName);
                            Fields.Add(Field);

                        }
                    }
                }
            }
        }
        return Task.FromResult(Fields);

    }



    public async Task FieldUpdate(FieldDetailedModel Field)
    {
        Console.WriteLine("Entering FieldData.FieldUpdate(FieldDetailedModel Field)");

        try
        {
            var parameters = new { fieldId = Field.FieldId,
                fieldPrimaryKey = Field.PrimaryKey,
                fieldIndexed = Field.Indexed,
                fieldMinValue = Field.MinValue,
                fieldMaxValue = Field.MaxValue,
                fieldSampleValue1 = Field.SampleValue1,
                fieldSampleValue2 = Field.SampleValue2,
                fieldSampleValue3 = Field.SampleValue3,
                fieldSampleValue4 = Field.SampleValue4,
                fieldSampleValue5 = Field.SampleValue5,
                fieldSampleValue6 = Field.SampleValue6,
                fieldSampleValue7 = Field.SampleValue7,
                fieldSampleValue8 = Field.SampleValue8,
                fieldSampleValue9 = Field.SampleValue9,
                fieldSampleValue10 = Field.SampleValue10,
                fieldNullPercentage = Field.NullPercentage };
            await _sqlDataAccess.SaveData<dynamic, DataTable>("dbo.spField_Update", parameters, "ToolboxData");

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception encountered in FieldUpdate: {ex.Message}");

        }
    }



    public async Task FieldsUpsert(List<FieldModel> Fields, int TableId)
    {
        Console.WriteLine("Entering FieldData.FieldUpsert(List<FieldModel> fields)");

        try
        {
            var parameters = new { fields = _converterHelper.ConvertModelToDataTable<FieldModel>(Fields), tableId = TableId };
            await _sqlDataAccess.SaveData<dynamic, DataTable>("dbo.spFields_Upsert", parameters, "ToolboxData");

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception encountered in FieldUpsert: {ex.Message}");

        }
    }



    public Task<FieldDetailedModel> FindAllValues(FieldDetailedModel field)
    {
        Console.WriteLine($"Entering FieldData.FindAllValues(FieldDetailModel field)");

        string ConnectionString = _sqlDataAccess.GetConnectionString(field.ServerName, field.DatabaseName);

        using (SqlConnection Connection = new SqlConnection(ConnectionString))
        {
            Connection.Open();
            string SqlScript = $@"
                USE {field.DatabaseName};
                declare 
	                @pkey bit
	                , @indexed bit  
                    , @bittype int
	                , @min varchar(max)
	                , @max varchar(max)
	                , @sampleValue1 varchar(max)
	                , @sampleValue2 varchar(max)
	                , @sampleValue3 varchar(max)
	                , @sampleValue4 varchar(max)
	                , @sampleValue5 varchar(max)
	                , @sampleValue6 varchar(max)
	                , @sampleValue7 varchar(max)
	                , @sampleValue8 varchar(max)
	                , @sampleValue9 varchar(max)
	                , @sampleValue10 varchar(max)
                    , @totalRecords float
                    , @totalNulls float
                    , @nullPercentage float

                set @pkey = (
	                select case when count(col.[name]) > 0 
			                then 1 
			                else 0 end
	                from sys.tables tab
		                inner join sys.indexes pk
			                on tab.[name] = '{field.TableName}'
			                and pk.is_primary_key = 1
			                and tab.object_id = pk.object_id
		                inner join sys.index_columns ic
			                on ic.object_id = pk.object_id
			                and ic.index_id = pk.index_id
		                inner join sys.columns col
			                on col.[name] = '{field.FieldName}'
			                and pk.object_id = col.object_id
			                and col.column_id = ic.column_id
	                )
                set @indexed  = (
	                select case when count(col.[name]) > 0 
			                then 1 
			                else 0 end
	                from sys.tables tab
		                inner join sys.indexes pk
			                on tab.object_id = pk.object_id
			                and tab.[name] = '{field.TableName}'
		                inner join sys.index_columns ic
			                on ic.object_id = pk.object_id
			                and ic.index_id = pk.index_id
		                inner join sys.columns col
			                on pk.object_id = col.object_id
			                and col.column_id = ic.column_id
			                and col.[name] = '{field.FieldName}'
	                )
                set @bittype = (
                    SELECT Count(DATA_TYPE)
                    FROM INFORMATION_SCHEMA.COLUMNS
                    WHERE TABLE_CATALOG = '{field.DatabaseName}'
                        AND TABLE_SCHEMA = '{field.SchemaName}'
                        AND TABLE_NAME = '{field.TableName}'
                        AND COLUMN_NAME = '{field.FieldName}'
                        AND DATA_TYPE = 'bit'
                        )
                set @min = (
	                SELECT MIN(CASE WHEN @bittype = 1 THEN cast('0' as varchar(max)) else cast([{field.FieldName}] as varchar(max)) END)
	                from [{field.DatabaseName}].[{field.SchemaName}].[{field.TableName}]
                    WHERE [{field.FieldName}] IS NOT NULL
	                )
                set @max = (
	                SELECT MAX(CASE WHEN @bittype = 1 THEN cast('1' as varchar(max)) else cast([{field.FieldName}] as varchar(max)) END) 
	                from [{field.DatabaseName}].[{field.SchemaName}].[{field.TableName}]
                    WHERE [{field.FieldName}] IS NOT NULL
	                )
                set @sampleValue1 = (
	                SELECT TOP 1 [{field.FieldName}]
	                FROM [{field.DatabaseName}].[{field.SchemaName}].[{field.TableName}] a
                    WHERE [{field.FieldName}] IS NOT NULL
                    ORDER BY NEWID()
	                )
                set @sampleValue2 = (
	                SELECT TOP 1 [{field.FieldName}]
	                FROM [{field.DatabaseName}].[{field.SchemaName}].[{field.TableName}] a 
	                WHERE [{field.FieldName}] IS NOT NULL AND [{field.FieldName}] <> @sampleValue1
                    ORDER BY NEWID()
	                )
                set @sampleValue3 = (
	                SELECT TOP 1 [{field.FieldName}]
	                FROM [{field.DatabaseName}].[{field.SchemaName}].[{field.TableName}] a 
	                WHERE [{field.FieldName}] IS NOT NULL AND [{field.FieldName}] NOT IN (@sampleValue1, @sampleValue2)                    
                    ORDER BY NEWID()
	                )
                set @sampleValue4 = (
	                SELECT TOP 1 [{field.FieldName}]
	                FROM [{field.DatabaseName}].[{field.SchemaName}].[{field.TableName}] a 
	                WHERE [{field.FieldName}] IS NOT NULL AND [{field.FieldName}] NOT IN (@sampleValue1, @sampleValue2, @sampleValue3)
                    ORDER BY NEWID()
	                )
                set @sampleValue5 = (
	                SELECT TOP 1 [{field.FieldName}]
	                FROM [{field.DatabaseName}].[{field.SchemaName}].[{field.TableName}] a 
	                WHERE [{field.FieldName}] IS NOT NULL AND [{field.FieldName}] NOT IN (@sampleValue1, @sampleValue2, @sampleValue3, @sampleValue4)
                    ORDER BY NEWID()
	                )
                set @sampleValue6 = (
	                SELECT TOP 1 [{field.FieldName}]
	                from [{field.DatabaseName}].[{field.SchemaName}].[{field.TableName}] a 
	                where [{field.FieldName}] IS NOT NULL AND [{field.FieldName}] not in (@sampleValue1, @sampleValue2, @sampleValue3, @sampleValue4, @sampleValue5)
                    ORDER BY NEWID()
	                )
                set @sampleValue7 = (
	                SELECT TOP 1 [{field.FieldName}]
	                from [{field.DatabaseName}].[{field.SchemaName}].[{field.TableName}] a 
	                where [{field.FieldName}] IS NOT NULL AND [{field.FieldName}] not in (@sampleValue1, @sampleValue2, @sampleValue3, @sampleValue4, @sampleValue5, @sampleValue6)
                    ORDER BY NEWID()
	                )
                set @sampleValue8 = (
	                SELECT TOP 1 [{field.FieldName}]
	                from [{field.DatabaseName}].[{field.SchemaName}].[{field.TableName}] a 
	                where [{field.FieldName}] IS NOT NULL AND [{field.FieldName}] not in (@sampleValue1, @sampleValue2, @sampleValue3, @sampleValue4, @sampleValue5, @sampleValue6, @sampleValue7)
                    ORDER BY NEWID()
	                )
                set @sampleValue9 = (
	                SELECT TOP 1 [{field.FieldName}]
	                from [{field.DatabaseName}].[{field.SchemaName}].[{field.TableName}] a 
	                where [{field.FieldName}] IS NOT NULL AND [{field.FieldName}] not in (@sampleValue1, @sampleValue2, @sampleValue3, @sampleValue4, @sampleValue5, @sampleValue6, @sampleValue7, @sampleValue8)
                    ORDER BY NEWID()
	                )
                set @sampleValue10 = (
	                SELECT TOP 1 [{field.FieldName}]
	                from [{field.DatabaseName}].[{field.SchemaName}].[{field.TableName}] a 
	                where [{field.FieldName}] IS NOT NULL AND [{field.FieldName}] not in (@sampleValue1, @sampleValue2, @sampleValue3, @sampleValue4, @sampleValue5, @sampleValue6, @sampleValue7, @sampleValue8, @sampleValue9)
                    ORDER BY NEWID()
	                )

                SET @totalRecords = (
                    SELECT COUNT(1) 
                    FROM [{field.DatabaseName}].[{field.SchemaName}].[{field.TableName}]
                    )

                SET @totalNulls = (
                    SELECT COUNT(1) 
                    FROM [{field.DatabaseName}].[{field.SchemaName}].[{field.TableName}]
                    WHERE [{field.FieldName}] IS NULL
                    )

                SET @nullPercentage = (CAST( ( @totalNulls / @totalRecords ) AS FLOAT) * 100.00)

                select @pkey PrimaryKey
	                , @indexed Indexed
	                , @min MinValue
	                , @max MaxValue
	                , @sampleValue1 SampleValue1
	                , @sampleValue2 SampleValue2
	                , @sampleValue3 SampleValue3
	                , @sampleValue4 SampleValue4
	                , @sampleValue5 SampleValue5
	                , @sampleValue6 SampleValue6
	                , @sampleValue7 SampleValue7
	                , @sampleValue8 SampleValue8
	                , @sampleValue9 SampleValue9
	                , @sampleValue10 SampleValue10
                    , @nullPercentage NullPercentage";

            using (SqlCommand Command = new SqlCommand(SqlScript, Connection))
            {

                using (SqlDataReader Reader = Command.ExecuteReader())
                {

                    if (Reader.HasRows)
                    {

                        while (Reader.Read())
                        {

                            int oPKey = Reader.GetOrdinal("PrimaryKey");

                            if (Reader["PrimaryKey"] != DBNull.Value)
                            {
                                field.PrimaryKey = 0;
                            }
                            else if (Reader.GetBoolean(oPKey) == true)
                            {
                                field.PrimaryKey = 1;
                            }
                            else
                            {
                                field.PrimaryKey = 0;
                            }

                            int oIndexed = Reader.GetOrdinal("Indexed");

                            if (Reader["Indexed"] != DBNull.Value)
                            {
                                field.Indexed = 0;
                            }
                            else if (Reader.GetBoolean(oIndexed) == true)
                            {
                                field.Indexed = 1;
                            }
                            else
                            {
                                field.Indexed = 0;
                            }

                            field.MinValue = Reader["MinValue"].ToString() ?? String.Empty;
                            field.MaxValue = Reader["MaxValue"].ToString() ?? String.Empty;
                            field.SampleValue1 = Reader["SampleValue1"].ToString() ?? String.Empty;
                            field.SampleValue2 = Reader["SampleValue2"].ToString() ?? String.Empty;
                            field.SampleValue3 = Reader["SampleValue3"].ToString() ?? String.Empty;
                            field.SampleValue4 = Reader["SampleValue4"].ToString() ?? String.Empty;
                            field.SampleValue5 = Reader["SampleValue5"].ToString() ?? String.Empty;
                            field.SampleValue6 = Reader["SampleValue6"].ToString() ?? String.Empty;
                            field.SampleValue7 = Reader["SampleValue7"].ToString() ?? String.Empty;
                            field.SampleValue8 = Reader["SampleValue8"].ToString() ?? String.Empty;
                            field.SampleValue9 = Reader["SampleValue9"].ToString() ?? String.Empty;
                            field.SampleValue10 = Reader["SampleValue10"].ToString() ?? String.Empty;

                            if (Reader["NullPercentage"] != DBNull.Value)
                            {
                                Console.WriteLine("Assigning NullPercentage");
                                //Field.NullPercentage = Reader.GetFloat("NullPercentage");
                                object oNullPercentage = Reader.GetValue(Reader.GetOrdinal("NullPercentage"));
                                field.NullPercentage = float.Parse(oNullPercentage.ToString());
                            }

                            Console.WriteLine(field.FieldName);

                        }
                    }
                }
            }
        }
        return Task.FromResult(field);

    }
}
