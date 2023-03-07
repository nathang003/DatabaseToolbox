CREATE PROCEDURE [dbo].[spTables_GetBySchemaId]
	@schemaId int = 0
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
        [TableId],
        [ServerId],
        [DatabaseId],
        [SchemaId],
        [TableName],
        [Purpose],
        [RemovalDate],
        [CreatedDate],
        [CreatedBy],
        [UpdatedDate],
        [UpdatedBy]
	FROM dbo.Tables
    WHERE SchemaId = @schemaId
	ORDER BY TableName;
END
