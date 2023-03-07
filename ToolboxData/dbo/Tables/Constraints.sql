CREATE TABLE [dbo].[Constraints]
(
	[ConstraintId] INT NOT NULL PRIMARY KEY identity(1,1),
	ServerId Int not null,
	DatabaseId Int not null,
	SchemaId int not null,
	TableId int not null,
	ConstraintName nvarchar(200) not null,
	[ConstraintDefinition] nvarchar(max) null,
	Purpose nvarchar(max) null,
	ConstraintType nvarchar(25) null,
	ConstraintTypeDescription nvarchar(150) null,
	IsSystemNamed bit not null default 0,
	CreatedDate DATETIME2(7) NOT NULL DEFAULT GETUTCDATE(),
	CreatedBy NVARCHAR(50) NOT NULL DEFAULT SUSER_SNAME(),
	UpdatedDate DATETIME2(7) NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy NVARCHAR(50) NOT NULL DEFAULT SUSER_SNAME()
)
