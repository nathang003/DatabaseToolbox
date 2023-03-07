CREATE PROCEDURE [dbo].[spDatabases_GetDatabaseDetailsByDatabaseId]
	@databaseId int = 0
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
        d.[DatabaseId],
        d.[ServerId],
        d.[DatabaseName],
        d.[Purpose],
        d.[RemovalDate],
        d.[CreatedDate],
        d.[CreatedBy],
        d.[UpdatedDate],
        d.[UpdatedBy],
        s.ServerName
	FROM dbo.Databases d
        inner join dbo.Servers s
            on d.ServerId = s.ServerId
	WHERE [DatabaseId] = @databaseId;
END