CREATE PROCEDURE [dbo].[spUserPermissions_GetByUserId]
	@UserId nvarchar(128)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT p.PermissionId,
		p.Permission,
		p.[Description],
		up.Granted,
		up.CreatedBy,
		up.CreatedDate,
		up.UpdatedBy,
		up.UpdatedDate
	FROM dbo.UserPermissions up
		INNER JOIN dbo.[Permissions] p
			ON up.PermissionId = p.PermissionId
	WHERE UserId = @UserId;
END
