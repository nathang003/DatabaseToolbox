CREATE TABLE [dbo].[ConstraintFields]
(
	[ConstraintFieldId] INT NOT NULL PRIMARY KEY identity(1,1),
	ServerId INT NOT NULL DEFAULT -1,
	DatabaseId INT NOT NULL DEFAULT -1,
	SchemaId INT NOT NULL DEFAULT -1,
	TableId INT NOT NULL DEFAULT -1,
	ConstraintId INT NOT NULL DEFAULT -1,
	FieldId INT NOT NULL DEFAULT -1, 
	IsNullable bit NULL,
	IsAnsiPadded bit NULL,
	IsRowGuidColumn BIT NULL,
	IsIdentity BIT NULL,
	GeneratedAlwaysType tinyint null,
	GeneratedAlwaysTypeDescription nvarchar(60) null,
	CreatedDate DATETIME2(7) NULL,
	CreatedBy NVARCHAR(50) NULL,
	UpdatedDate DATETIME2(7) NULL,
	UpdatedBy NVARCHAR(50) NULL
)
