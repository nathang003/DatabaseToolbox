CREATE PROCEDURE [dbo].[spUserPermissions_GetByManagerUserId]
	@UserId nvarchar(128)
AS
SELECT Distinct
	   users.[UserId],
       users.[FirstName] + ' ' +  users.[LastName] AS Users,
	   manager.[UserId] ManagerUserId,
	   manager.FirstName + ' ' +   manager.LastName AS Managers,
	    p.Permission,
		p.PermissionDisplayName,
		p.[Description],
		p.[Page],
		p.[PermissionType],
		up.Granted,
		up.CreatedBy,
		up.CreatedDate,
		up.UpdatedBy,
		up.UpdatedDate
  FROM [dbo].[Users] users
  inner JOIN [dbo].[Users] manager ON manager.UserId = users.ManagerUserId
  INNER JOIN [dbo].[UserPermissions] up ON up.UserId = users.UserId
  INNER JOIN [dbo].[Permissions] p ON up.PermissionId = p.PermissionId
  WHERE manager.UserId =  @UserId;
