/*
-------------
-- TESTING --
-------------

1. In SSMS, execute the script:
	SELECT *
	FROM dbo.Fields
	WHERE ServerName = <enter server name>
		 AND DatabaseName = <enter database name>
		 AND SchemaName = <enter schema name>
		 AND TableName = <enter table name>
		 AND FieldName = <enter field name>
2. Open the Toolbox app.
3. Navigate to any field.
4. Select "Scrape Values".
5. In SSMS, execute the script:
	SELECT *
	FROM dbo.Fields
	WHERE ServerName = <enter server name>
		 AND DatabaseName = <enter database name>
		 AND SchemaName = <enter schema name>
		 AND TableName = <enter table name>
		 AND FieldName = <enter field name>
6. When the scrape has completed, the results of step 5 should be different from those in step 1 
	(unless the table is extremely small or empty).

*/

/*
------------------------------------
-- ToolboxData.dbo.spField_Update --
------------------------------------
Created By:		Nate Gallagher
Created Date:	08/09/2022
Purpose:		Provide a standard way to update basic field value statistics complete with a small random sampling.

Updated Date:	
Updated By:		
Ticket # & URL: 
Change Notes:	

*/

CREATE PROCEDURE [dbo].[spField_Update]
(
	@fieldId int,
	@fieldPrimaryKey int,
	@fieldIndexed int,
	@fieldMinValue varchar(max),
	@fieldMaxValue varchar(max),
	@fieldSampleValue1 varchar(max),
	@fieldSampleValue2 varchar(max),
	@fieldSampleValue3 varchar(max),
	@fieldSampleValue4 varchar(max),
	@fieldSampleValue5 varchar(max),
	@fieldSampleValue6 varchar(max),
	@fieldSampleValue7 varchar(max),
	@fieldSampleValue8 varchar(max),
	@fieldSampleValue9 varchar(max),
	@fieldSampleValue10 varchar(max),
	@fieldNullPercentage float
)
AS
BEGIN
	---------------------------
	-- UPDATE EXISTING FIELD --
	---------------------------

	UPDATE f
	SET f.PrimaryKey = @fieldPrimaryKey,
		f.Indexed = @fieldIndexed,
		f.MinValue = @fieldMinValue,
		f.MaxValue = @fieldMaxValue,
		f.SampleValue1 = @fieldSampleValue1,
		f.SampleValue2 = @fieldSampleValue2, 
		f.SampleValue3 = @fieldSampleValue3,
		f.SampleValue4 = @fieldSampleValue4,
		f.SampleValue5 = @fieldSampleValue5,
		f.SampleValue6 = @fieldSampleValue6,
		f.SampleValue7 = @fieldSampleValue7,
		f.SampleValue8 = @fieldSampleValue8,
		f.SampleValue9 = @fieldSampleValue9,
		f.SampleValue10 = @fieldSampleValue10,
		f.NullPercentage = @fieldNullPercentage,
		f.UpdatedDate = GETUTCDATE(),
		f.UpdatedBy = SUSER_SNAME()
	FROM dbo.Fields f
	WHERE f.FieldId = @fieldId;

END
