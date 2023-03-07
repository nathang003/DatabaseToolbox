
/*
--------------------------------------
-- ToolboxData.dbo.[IndexModelType] --
--------------------------------------
Created By:		Nate Gallagher
Created Date:	08/11/2022
Purpose:		Provide a way to pass in index data using data tables for bulk upsert functionality.

Updated Date:	
Updated By:		
Ticket # & URL: 
Change Notes:	

*/

CREATE TYPE [dbo].[IndexModelType] AS TABLE
(
	IndexId INT NULL,
	ServerId INT NULL,
	DatabaseId INT NULL,
	SchemaId INT NULL,
	TableId INT NULL,
	IndexName nvarchar(200) NULL,
	Purpose NVARCHAR(MAX) NULL,
	IndexType INT NULL,
	IndexTypeDescription NVARCHAR(25) NULL,
	IsPrimaryKey BIT NULL,
	IsUniqueConstraint BIT NULL,
	IsUnique BIT NULL,
	IsDisabled BIT NULL,
	CreatedDate DATETIME2(7) NULL,
	CreatedBy NVARCHAR(50) NULL,
	UpdatedDate DATETIME2(7) NULL,
	UpdatedBy NVARCHAR(50) NULL
)
