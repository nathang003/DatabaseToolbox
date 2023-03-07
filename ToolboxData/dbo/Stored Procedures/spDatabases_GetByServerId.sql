CREATE PROCEDURE [dbo].[spDatabases_GetByServerId]
	@serverId int = 0
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
    WHERE ServerId = @serverId
	ORDER BY DatabaseName;
END