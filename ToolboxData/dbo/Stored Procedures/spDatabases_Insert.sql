CREATE PROCEDURE [dbo].[spDatabases_Insert]
	@serverId int
    , @databaseName nvarchar(100)
    , @purpose nvarchar(max)
    , @removalDate datetime2(7)
    , @createdDate datetime2(7)
    , @createdBy nvarchar(50)
    , @updatedDate datetime2(7)
    , @updatedBy nvarchar(50)
AS
BEGIN
	SET NOCOUNT ON;

    if ((select count(*) from dbo.[Databases] where ServerId = @serverId and DatabaseName = @databaseName) = 0 )
    begin
    INSERT INTO dbo.[Databases] values
    (    @serverId,
        @databaseName,
        @purpose,
        @removalDate,
        @createdDate,
        @createdBy,
        @updatedDate,
        @updatedBy);
    end;
END