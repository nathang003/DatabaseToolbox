
/*
--------------------------------------
-- ToolboxData.dbo.[ConstraintModelType] --
--------------------------------------
Created By:		Nate Gallagher
Created Date:	08/25/2022
Purpose:		Provide a way to pass in constraint data using data tables for bulk upsert functionality.

Updated Date:	
Updated By:		
Ticket # & URL: 
Change Notes:	

*/

CREATE TYPE [dbo].[ConstraintModelType] AS TABLE
(
	[ConstraintId] INT NULL,
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
	CreatedDate DATETIME2(7) NULL,
	CreatedBy NVARCHAR(50) NULL,
	UpdatedDate DATETIME2(7) NULL,
	UpdatedBy NVARCHAR(50) NULL
)
