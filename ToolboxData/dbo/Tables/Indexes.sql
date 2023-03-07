CREATE TABLE [dbo].[Indexes]
(
	[IndexId] INT NOT NULL PRIMARY KEY identity(1,1),
	ServerId Int not null,
	DatabaseId INT NOT NULL,
	SchemaId INT NOT NULL,
	TableId INT NOT NULL,
	IndexName nvarchar(200) NOT NULL,
	Purpose NVARCHAR(MAX) NULL,
	IndexType int NOT NULL,
	IndexTypeDescription nvarchar(25) not null,
	IsPrimaryKey bit not null default 0,
	IsUniqueConstraint bit not null default 0,
	IsUnique BIT NOT NULL DEFAULT 0,
	IsDisabled bit not null default 0,
	CreatedDate DATETIME2(7) NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy NVARCHAR(50) NOT NULL DEFAULT SUSER_SNAME(),
	UpdatedDate DATETIME2(7) NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy NVARCHAR(50) NOT NULL DEFAULT SUSER_SNAME()
)
