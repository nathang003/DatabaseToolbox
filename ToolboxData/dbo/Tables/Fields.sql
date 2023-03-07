﻿CREATE TABLE [dbo].[Fields]
(
	[FieldId] INT NOT NULL PRIMARY KEY IDENTITY, 
    [ServerId] INT NOT NULL, 
    [DatabaseId] INT NOT NULL, 
    [SchemaId] INT NOT NULL, 
    [TableId] INT NOT NULL, 
    [FieldName] NVARCHAR(150) NOT NULL, 
    [Purpose] NVARCHAR(MAX) NULL, 
    [OrdinalNumber] INT NOT NULL DEFAULT 0,
    DefaultValue NVARCHAR(MAX) NULL,
    [Nullable] BIT NOT NULL DEFAULT 1, 
    [DataType] NVARCHAR(50) NOT NULL, 
    [CharacterLength] INT NULL, 
    [NumericPrecision] INT NULL, 
    NumericScale INT NULL,
    DateTimePrecision INT NULL,
    CharacterSetName NVARCHAR(50) NULL,
    [CollationName] NVARCHAR(100) NULL,
    [PrimaryKey] BIT NOT NULL DEFAULT 0, 
    [Indexed] BIT NOT NULL DEFAULT 0, 
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
    [CreatedDate] DATETIME2 NOT NULL DEFAULT GETUTCDATE(), 
    [CreatedBy] NVARCHAR(50) NOT NULL DEFAULT SUSER_SNAME(), 
    [UpdatedDate] DATETIME2 NOT NULL DEFAULT GETUTCDATE(), 
    [UpdatedBy] NVARCHAR(50) NOT NULL DEFAULT SUSER_SNAME()
)
