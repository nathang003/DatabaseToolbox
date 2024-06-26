﻿CREATE TABLE [dbo].[ScrapeStatuses]
(
	[ScrapeStatusId] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
	ScrapeStatus NVARCHAR(50) NOT NULL,
	StatusDescription NVARCHAR(300) NULL,
	CreatedDate DATETIME2(7) NOT NULL default GETUTCDATE(),
	CreatedBy NVARCHAR(50) NOT null default SUSER_SNAME(),
	UpdatedDate DATETIME2(7) NOT NULL DEFAULT GETUTCDATE(),
	UpdatedBy NVARCHAR(50) NOT NULL DEFAULT SUSER_SNAME()
)
