CREATE TYPE [dbo].[SchemaModelType] AS TABLE
(
	SchemaId INT,
	ServerId INT not null,
	DatabaseId INT NOT NULL, 
	SchemaName nvarchar(50) not null,
	Purpose nvarchar(max),
	RemovalDate datetime2(7),
	CreatedDate datetime2(7),
	CreatedBy nvarchar(50),
	UpdatedDate datetime2(7),
	UpdatedBy nvarchar(50)
)
