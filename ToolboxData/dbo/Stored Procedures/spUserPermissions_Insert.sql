/*
	Title:	ToolboxApp.dbo.spUserPermissions_Insert
	Author: Nate Gallagher
	Created Date:	03/14/2022
	Description:	This sproc is designed to insert new user permission
		records for available application functionality.
*/

CREATE PROCEDURE [dbo].[spUserPermissions_Insert]
	@userId NVARCHAR(128) = '-1'
AS
BEGIN
	-- Check if the entered @userId is valid.
	IF @userId = '-1' OR 
		(
			SELECT COUNT(1)
			FROM dbo.Users
			WHERE UserId = QUOTENAME(@userId)
		) < 1

		RETURN -1

	ELSE
    BEGIN
		-- Find missing permissions by matching any available permissions to the user
		; WITH MissingPermissions AS (
			SELECT p.PermissionId,
				p.Permission,
				p.[Description],
				p.CreatedDate,
				p.CreatedBy,
				p.UpdatedDate,
				p.UpdatedBy
			FROM dbo.[Permissions] p
				LEFT JOIN dbo.UserPermissions up
					ON p.PermissionId = up.PermissionId
						AND up.UserId = @userId
			WHERE up.UserPermissionId IS null
			)

		-- Insert any missing permissions for the user.
		INSERT INTO dbo.UserPermissions
		(
			UserId,
			PermissionId,
			Granted,
			CreatedDate,
			CreatedBy,
			UpdatedDate,
			UpdatedBy
		)
		SELECT @userId,
			mp.PermissionId,
			0,
			GETUTCDATE(),
			SUSER_SNAME(),
			GETUTCDATE(),
			SUSER_SNAME()
		FROM MissingPermissions mp

		RETURN 0

    END
    
end
