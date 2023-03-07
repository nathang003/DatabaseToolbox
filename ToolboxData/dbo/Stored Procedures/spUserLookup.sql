CREATE PROCEDURE [dbo].[spUserLookup]
	@UserId nvarchar(128)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT *
	FROM dbo.Users
	WHERE UserId = @UserId;
END
