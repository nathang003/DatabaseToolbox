CREATE PROCEDURE [dbo].[spDatabaseObjects_GetAllNonDev]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
        se.ServerId,
        se.ServerName,
		se.IsDevelopmentServer,
        d.DatabaseId,
        d.[DatabaseName],
        sc.SchemaId,
        sc.[SchemaName],
        t.TableId,
        t.[TableName],
        f.FieldId,
        f.[FieldName]
	FROM dbo.Servers se
		INNER JOIN dbo.[Databases] d
			ON se.ServerId = d.ServerId
		INNER JOIN dbo.[Schemas] sc
			ON se.ServerId = sc.ServerId
			AND d.DatabaseId = sc.DatabaseId
		INNER JOIN dbo.[Tables] t
			ON se.ServerId = t.ServerId
			AND d.DatabaseId = t.DatabaseId
			AND sc.SchemaId = t.SchemaId
		INNER JOIN dbo.[Fields] f
			ON se.ServerId = f.ServerId
			AND d.DatabaseId = f.DatabaseId
			AND sc.SchemaId = f.SchemaId
			AND t.TableId = f.TableId
	WHERE se.IsDevelopmentServer = 0
	ORDER BY 
		se.ServerName ASC,
		d.DatabaseName ASC,
		sc.SchemaName ASC,
		t.TableName ASC,
		f.FieldName ASC;
END

