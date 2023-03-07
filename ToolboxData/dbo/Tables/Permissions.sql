CREATE TABLE [dbo].[Permissions]
(
	[PermissionId] INT NOT NULL PRIMARY KEY IDENTITY(1,1), 
    [Permission] NVARCHAR(150) NOT NULL, 
    [PermissionDisplayName] NVARCHAR(150) NOT NULL,
    [Description] NVARCHAR(MAX) NULL, 
    [Page] NVARCHAR(150) NOT NULL,
    [PermissionType] NVARCHAR(50) NOT NULL,
    [CreatedDate] DATETIME2 NOT NULL DEFAULT GETUTCDATE(), 
    [CreatedBy] NVARCHAR(128) NOT NULL DEFAULT SUSER_SNAME(), 
    [UpdatedDate] DATETIME2 NOT NULL DEFAULT GETUTCDATE(), 
    [UpdatedBy] NVARCHAR(128) NOT NULL DEFAULT SUSER_SNAME()
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Brief title for the permission.',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Permissions',
    @level2type = N'COLUMN',
    @level2name = N'Permission'
GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'Format: [Type of permission][Permission concept][Permission element category]
Example: ViewDataArchitecturePage',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Permissions',
    @level2type = N'COLUMN',
    @level2name = N'Description'