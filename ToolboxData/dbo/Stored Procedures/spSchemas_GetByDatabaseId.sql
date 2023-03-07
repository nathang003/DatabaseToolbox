CREATE PROCEDURE [dbo].[spSchemas_GetByDatabaseId]
	@databaseId int = 0
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
        [SchemaId],
        [ServerId],
        [DatabaseId],
        [SchemaName],
        [Purpose],
        [RemovalDate],
        [CreatedDate],
        [CreatedBy],
        [UpdatedDate],
        [UpdatedBy]
	FROM dbo.[Schemas]
    WHERE DatabaseId = @databaseId
	ORDER BY SchemaName;
END
