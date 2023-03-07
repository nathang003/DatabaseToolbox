CREATE TYPE [dbo].[TableModelType] AS TABLE
(
	TableId INT,
	ServerId INT not null,
	DatabaseId INT NOT NULL, 
	SchemaId INT NOT NULL,
	TableName nvarchar(100) not null,
	Purpose nvarchar(max),
	RemovalDate datetime2(7),
	CreatedDate datetime2(7),
	CreatedBy nvarchar(50),
	UpdatedDate datetime2(7),
	UpdatedBy nvarchar(50)
)
