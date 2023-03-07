CREATE PROCEDURE [dbo].[spDatabases_GetAll]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
        [DatabaseId],
        [ServerId],
        [DatabaseName],
        [Purpose],
        [RemovalDate],
        [CreatedDate],
        [CreatedBy],
        [UpdatedDate],
        [UpdatedBy]
	FROM dbo.[Databases]
	ORDER BY DatabaseName;
END
