CREATE PROCEDURE [dbo].[spServers_GetAll]
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
	ORDER BY [ServerName];
END
