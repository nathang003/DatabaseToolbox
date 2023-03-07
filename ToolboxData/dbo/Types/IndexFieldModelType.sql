CREATE TYPE [dbo].[IndexFieldModelType] AS TABLE
(
	IndexFieldId INT NULL,
	ServerId INT NULL,
	ServerName NVARCHAR(150) null,
	DatabaseId INT NULL,
	DatabaseName NVARCHAR(150) null,
	SchemaId INT NULL,
	SchemaName NVARCHAR(150) null,
	TableId INT NULL,
	TableName NVARCHAR(150) null,
	IndexId INT NULL,
	IndexName NVARCHAR(150) null,
	FieldId INT NULL,
	FieldName NVARCHAR(150) null,
	KeyOrdinal SMALLINT NULL,
	PartialOrdinal SMALLINT NULL,
	IsDescendingKey BIT NULL,
	IsIncludedColumn BIT NULL,
	CreatedDate DATETIME2(7) NULL,
	CreatedBy NVARCHAR(50) NULL,
	UpdatedDate DATETIME2(7) NULL,
	UpdatedBy NVARCHAR(50) NULL
)
