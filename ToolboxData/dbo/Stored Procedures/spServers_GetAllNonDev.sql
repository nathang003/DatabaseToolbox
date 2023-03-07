CREATE PROCEDURE [dbo].[spServers_GetAllNonDev]
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
    WHERE IsDevelopmentServer = 0
	ORDER BY [ServerName];
END
