CREATE PROCEDURE [dbo].[spTables_GetAll]
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
	ORDER BY TableName;
END
