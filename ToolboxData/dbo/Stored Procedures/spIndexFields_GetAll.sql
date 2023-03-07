CREATE PROCEDURE [dbo].[spIndexFields_GetAll]
AS
begin
	select
		inf.IndexFieldId,
		inf.IndexId,
		inf.FieldId,
		inf.KeyOrdinal,
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
		f.SampleValue3,
		f.SampleValue4,
		f.SampleValue5,
		f.SampleValue6,
		f.SampleValue7,
		f.SampleValue8,
		f.SampleValue9,
		f.SampleValue10,
		f.CreatedDate,
		f.CreatedBy,
		f.UpdatedDate,
		f.UpdatedBy
	from dbo.IndexFields inf
	inner join dbo.Fields f
		on inf.FieldId = f.FieldId
	order by inf.KeyOrdinal asc
end
