CREATE PROCEDURE [dbo].[spSchemas_GetIdByNames]
	@serverName varchar(150),
	@databaseName varchar(150),
	@schemaName varchar(150)
AS
BEGIN
	SELECT
		sc.SchemaId
	FROM dbo.Schemas sc
		INNER JOIN dbo.Servers sc_s
			ON sc_s.ServerId = sc.ServerId
		INNER JOIN dbo.Databases sc_d
			ON sc_d.DatabaseId = sc.databaseId
	WHERE
		sc_s.ServerName = @serverName
		AND sc_d.DatabaseName = @databaseName
		AND sc.SchemaName = @schemaName
END
