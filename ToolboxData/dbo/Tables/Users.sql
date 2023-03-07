CREATE TABLE [dbo].[Users]
(
	[UserId] NVARCHAR(128) NOT NULL PRIMARY KEY, 
    [FirstName] NVARCHAR(50) NOT NULL, 
    [LastName] NVARCHAR(50) NOT NULL, 
    [EmailAddress] NVARCHAR(256) NOT NULL, 
    [ManagerUserId] NVARCHAR(128) NULL,
    [CreatedDate] DATETIME2 NOT NULL DEFAULT getutcdate(), 
    [CreatedBy] NVARCHAR(50) NOT NULL DEFAULT SUSER_SNAME(),
    [UpdatedDate] DATETIME2 NOT NULL DEFAULT getutcdate(),
    [UpdatedBy] NVARCHAR(50) NOT NULL DEFAULT SUSER_SNAME()
)
