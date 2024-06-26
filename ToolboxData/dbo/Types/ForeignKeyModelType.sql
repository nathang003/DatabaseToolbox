﻿CREATE TYPE [dbo].[ForeignKeyModelType] AS TABLE
(
	ServerName NVARCHAR(150),
	DatabaseName NVARCHAR(150),
	SchemaName NVARCHAR(150),
	TableName NVARCHAR(150),
	ConstraintSchemaName NVARCHAR(150),
	ConstraintTableName NVARCHAR(150),
	ConstraintFieldName NVARCHAR(150),
	ReferencedSchemaName NVARCHAR(150),
	ReferencedTableName NVARCHAR(150),
	ReferencedFieldName NVARCHAR(150),
	[ForeignKeyId] INT NULL,
	ServerId INT NULL,
	DatabaseId INT NULL,
	SchemaId INT NULL,
	TableId INT NULL,
	ForeignKeyName NVARCHAR(200) NULL,
	Purpose NVARCHAR(MAX) NULL,
	ConstraintSchemaId int null,
	ConstraintTableId INT NULL,
	ConstraintFieldId INT NULL,
	ReferencedSchemaId INT NULL,
	ReferencedTableId INT NULL,
	ReferencedFieldId INT NULL,
	IsDisabled bit nULL,
	IsNotTrusted bit nULL,
	DeleteReferentialActionDescription nvarchar(50),
	UpdateReferentialActionDescription nvarchar(50),
	CreatedDate DATETIME2(7) NULL,
	CreatedBy NVARCHAR(50) NULL,
	UpdatedDate DATETIME2(7) NULL,
	UpdatedBy NVARCHAR(50) NULL
)
