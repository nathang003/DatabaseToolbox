CREATE PROCEDURE [dbo].[spFields_GetByTableId]
	@tableId int = 0
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
        [FieldId],
        [ServerId],
        [DatabaseId],
        [SchemaId],
        [TableId],
        [FieldName],
        [Purpose],
        OrdinalNumber,
        DefaultValue,
        Nullable,
        DataType,
        [CharacterLength],
        [NumericPrecision],
        NumericScale,
        DateTimePrecision,
        CharacterSetName,
        [CollationName],
        PrimaryKey,
        Indexed,
        MinValue,
        MaxValue,
        SampleValue1,
        SampleValue2,
        SampleValue3,
        SampleValue4,
        SampleValue5,
        SampleValue6,
        SampleValue7,
        SampleValue8,
        SampleValue9,
        SampleValue10,
        NullPercentage,
        [CreatedDate],
        [CreatedBy],
        [UpdatedDate],
        [UpdatedBy]
	FROM dbo.Fields
    WHERE TableId = @tableId
	ORDER BY OrdinalNumber;
END
