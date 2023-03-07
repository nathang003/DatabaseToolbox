CREATE TYPE [DatabaseModelType] AS TABLE
(
	DatabaseId INT, 
	ServerId INT not null,
	DatabaseName nvarchar(100) not null,
	Purpose nvarchar(max),
	RemovalDate datetime2(7),
	CreatedDate datetime2(7),
	CreatedBy nvarchar(50),
	UpdatedDate datetime2(7),
	UpdatedBy nvarchar(50)
)
