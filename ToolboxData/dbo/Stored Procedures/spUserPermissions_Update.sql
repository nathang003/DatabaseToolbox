/*
	Title:	ToolboxApp.dbo.spUserPermissions_Update
	Author: Nate Gallagher
	Created Date:	03/14/2022
	Description:	This sproc is designed to update a user's permission
		for a given functionality.
*/

CREATE PROCEDURE [dbo].[spUserPermissions_Update]
	@userId NVARCHAR(128) = '-1',
	@permissionId INT = -1
AS
BEGIN
	-- Validate the received variables
	IF (@userId = -1
		OR @permissionId = -1)
		RETURN -1

	-- Make the update to the user permission
	ELSE
	BEGIN
		DECLARE @granted BIT = 0;
		SET @granted = 
			(
				SELECT TOP(1) Granted 
				FROM dbo.UserPermissions 
				WHERE UserId = @userId 
					AND PermissionId = @permissionId
				ORDER BY UserPermissionId
			)

		UPDATE dbo.UserPermissions
		SET Granted = CASE @granted WHEN 0 THEN 1 ELSE 0 end,
			UpdatedBy = SUSER_SNAME(),
			UpdatedDate = GETDATE()
		WHERE UserId = @userId
			AND PermissionId = @permissionId

		RETURN 0
	END
END