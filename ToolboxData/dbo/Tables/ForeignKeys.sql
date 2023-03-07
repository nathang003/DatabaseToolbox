CREATE TABLE [dbo].[ForeignKeys]
(
	[ForeignKeyId] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	ServerId INT NOT NULL,
	DatabaseId INT NOT NULL,
	SchemaId INT NOT NULL,
	TableId INT NOT NULL,
	ForeignKeyName NVARCHAR(200) NOT NULL,
	Purpose NVARCHAR(MAX) NULL,
	ConstraintSchemaId int not null DEFAULT -1,
	ConstraintTableId INT NOT NULL DEFAULT -1,
	ConstraintFieldId INT NOT NULL DEFAULT -1,
	ReferencedSchemaId INT NOT NULL DEFAULT -1,
	ReferencedTableId INT NOT NULL DEFAULT -1,
	ReferencedFieldId INT NOT NULL DEFAULT -1,
	IsDisabled bit not null default 0,
	IsNotTrusted bit not null default 1,
	DeleteReferentialActionDescription nvarchar(50),
	UpdateReferentialActionDescription nvarchar(50),
	CreatedDate DATETIME2(7) NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy NVARCHAR(50) NOT NULL DEFAULT SUSER_SNAME(),
	UpdatedDate DATETIME2(7) NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy NVARCHAR(50) NOT NULL DEFAULT SUSER_SNAME()
)
