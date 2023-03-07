
/*
--------------------------------------
-- ToolboxData.dbo.[FieldModelType] --
--------------------------------------
Created By:		Nate Gallagher
Created Date:	07/20/2022
Purpose:		Provide a way to pass in field data using data tables for bulk upsert functionality.

Updated Date:	08/09/2022
Updated By:		Nate Gallagher
Ticket # & URL: Task 73414: Develop SQL script to get value data for a targeted field
                https://mynexusdevelopment.visualstudio.com/Portal%203.0/_workitems/edit/73414/
Change Notes:	Added the NullPercentage field.

*/

CREATE TYPE [dbo].[FieldModelType] AS TABLE
(
    [FieldId] INT NULL, 
    [ServerId] INT NULL, 
    [DatabaseId] INT NULL, 
    [SchemaId] INT NULL, 
    [TableId] INT NULL, 
    [FieldName] NVARCHAR(150) NULL, 
    [Purpose] NVARCHAR(MAX) NULL, 
    [OrdinalNumber] INT NULL DEFAULT 0,
    DefaultValue NVARCHAR(MAX) NULL,
    [IsNullable] BIT NULL DEFAULT 1, 
    [DataType] NVARCHAR(50) NULL, 
    [CharacterLength] INT NULL, 
    [NumericPrecision] INT NULL, 
    NumericScale INT NULL,
    DateTimePrecision INT NULL,
    CharacterSetName NVARCHAR(50) NULL,
    [CollationName] NVARCHAR(100) NULL,
    [PrimaryKey] BIT NULL DEFAULT 0, 
    [Indexed] BIT NULL DEFAULT 0, 
    [MinValue] NVARCHAR(MAX) NULL,
    [MaxValue] NVARCHAR(MAX) NULL,
    [SampleValue1] NVARCHAR(MAX) NULL,
    [SampleValue2] NVARCHAR(MAX) NULL,
    [SampleValue3] NVARCHAR(MAX) NULL,
    [SampleValue4] NVARCHAR(MAX) NULL,
    [SampleValue5] NVARCHAR(MAX) NULL,
    [SampleValue6] NVARCHAR(MAX) NULL,
    [SampleValue7] NVARCHAR(MAX) NULL,
    [SampleValue8] NVARCHAR(MAX) NULL,
    [SampleValue9] NVARCHAR(MAX) NULL,
    [SampleValue10] NVARCHAR(MAX) NULL,
    [NullPercentage] FLOAT NULL,
    [RemovalDate] DATETIME2(7) NULL,
    [CreatedDate] DATETIME2 NULL DEFAULT GETUTCDATE(), 
    [CreatedBy] NVARCHAR(50) NULL DEFAULT SUSER_SNAME(), 
    [UpdatedDate] DATETIME2 NULL DEFAULT GETUTCDATE(), 
    [UpdatedBy] NVARCHAR(50) NULL DEFAULT SUSER_SNAME()
)
