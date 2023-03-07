/*
-------------
-- TESTING --
-------------

EXEC dbo.spFields_GetFieldDetailsByFieldId <any number between 1 - 100,000>
*/

/*
-------------------------------------------------------
-- ToolboxData.dbo.spFields_GetFieldDetailsByFieldId --
-------------------------------------------------------
Created By:		Nate Gallagher
Created Date:	08/09/2022
Purpose:		Provide a standard way to get detailed field data in a format that matches the FieldDetailedModel.

Updated By:
Updated Date:
Ticket #:
Ticket Url:
Ticket Notes:

*/
CREATE PROCEDURE [dbo].[spFields_GetFieldDetailsByFieldId]
	@fieldId INT
AS
begin
	SELECT 
		f.FieldId,
		f.ServerId,
		f.DatabaseId,
		f.SchemaId,
		f.TableId,
		f.FieldName,
		f.Purpose,
		f.OrdinalNumber,
		f.DefaultValue,
		f.Nullable,
		f.DataType,
		f.CharacterLength,
		f.NumericPrecision,
		f.NumericScale,
		f.DateTimePrecision,
		f.CharacterSetName,
		f.[CollationName],
		f.PrimaryKey,
		f.Indexed,
		f.MinValue,
		f.MaxValue,
		f.SampleValue1,
		f.SampleValue2,
		f.SampleValue2,
		f.SampleValue2,
		f.SampleValue2,
		f.SampleValue2,
		f.SampleValue2,
		f.SampleValue2,
		f.SampleValue2,
		f.SampleValue2,
		f.NullPercentage,
		f.RemovalDate,
		f.CreatedDate,
		f.CreatedBy,
		f.UpdatedDate,
		f.UpdatedBy,
		f.ServerId,
		f_s.ServerName,
		f_d.DatabaseName,
		f_sc.SchemaName,
		f_t.TableName
	FROM dbo.fields f
		INNER JOIN dbo.Tables f_t
			ON f.TableId = f_t.TableId
		INNER JOIN dbo.Schemas f_sc 
			ON f.SchemaId = f_sc.SchemaId
		INNER JOIN dbo.Databases f_d 
			ON f.DatabaseId = f_d.DatabaseId
		INNER JOIN dbo.Servers f_s 
			ON f.ServerId = f_s.ServerId 
	WHERE f.FieldId = @fieldId
end
