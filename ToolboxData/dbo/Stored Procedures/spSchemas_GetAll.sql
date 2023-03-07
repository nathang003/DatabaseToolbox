CREATE PROCEDURE [dbo].[spSchemas_GetAll]
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
	ORDER BY [SchemaName];
END
