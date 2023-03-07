CREATE PROCEDURE [dbo].[spServers_GetByServerId]
	@serverId int = 0
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
        [ServerId],
        [ServerName],
        [Purpose],
        [IsDevelopmentServer],
        [CreatedDate],
        [CreatedBy],
        [UpdatedDate],
        [UpdatedBy]
	FROM dbo.Servers
	WHERE [ServerId] = @serverId;
END
