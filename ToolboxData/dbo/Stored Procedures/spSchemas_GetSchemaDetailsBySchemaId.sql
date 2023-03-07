CREATE PROCEDURE [dbo].[spSchemas_GetSchemaDetailsBySchemaId]
	@schemaId int = 0
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
        sc.SchemaId,
        sc.[ServerId],
        sc.[DatabaseId],
        sc.[SchemaName],
        sc.[Purpose],
        sc.[RemovalDate],
        sc.[CreatedDate],
        sc.[CreatedBy],
        sc.[UpdatedDate],
        sc.[UpdatedBy],
        s.ServerName,
        d.DatabaseName
	FROM dbo.Schemas sc
        inner join dbo.Databases d
            on sc.DatabaseId = d.DatabaseId
        inner join dbo.Servers s
            on d.ServerId = s.ServerId
	WHERE [SchemaId] = @schemaId;
END
