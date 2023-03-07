CREATE PROCEDURE [dbo].[spFields_GetFieldIdByNames]
	@serverName varchar(max),
	@databaseName varchar(max),
	@schemaName varchar(max),
	@tableName varchar(max),
	@fieldName varchar(max)
AS
begin
	SELECT FieldId
	FROM dbo.Fields f
		INNER JOIN dbo.Servers f_s
			ON f.ServerId = f_s.ServerId
		INNER JOIN dbo.Databases f_d
			ON f.DatabaseId = f_d.DatabaseId
		INNER JOIN dbo.Schemas f_sc
			ON f.SchemaId = f_sc.SchemaId
		INNER JOIN dbo.Tables f_t
			ON f.TableId = f_t.TableId
	WHERE f_s.ServerName = @serverName
		AND f_d.DatabaseName = @databaseName
		AND f_sc.SchemaName = @schemaName
		AND f_t.TableName = @tableName
		AND f.FieldName = @fieldName
end