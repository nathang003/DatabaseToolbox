CREATE TABLE [dbo].[UserPermissions]
(
	[UserPermissionId] INT NOT NULL PRIMARY KEY, 
    [UserId] NVARCHAR(128) NOT NULL, 
    [PermissionId] INT NOT NULL, 
    [Granted] BIT NOT NULL DEFAULT 0, 
    [CreatedDate] DATETIME2 NOT NULL DEFAULT getutcdate(), 
    [CreatedBy] NVARCHAR(128) NOT NULL DEFAULT SUSER_SNAME(), 
    [UpdatedDate] DATETIME2 NOT NULL DEFAULT getutcdate(), 
    [UpdatedBy] NVARCHAR(128) NOT NULL DEFAULT SUSER_SNAME()
)
