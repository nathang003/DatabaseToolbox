CREATE PROCEDURE [dbo].[spConstraintFields_GetAll]
AS
Begin
	select
		ConstraintFieldId,
		ServerId,
		DatabaseId,
		SchemaId,
		TableId,
		ConstraintId,
		FieldId,
		IsNullable,
		IsAnsiPadded,
		IsRowGuidColumn,
		IsIdentity,
		GeneratedAlwaysType,
		GeneratedAlwaysTypeDescription,
		CreatedDate,
		CreatedBy,
		UpdatedDate,
		UpdatedBy
	from dbo.ConstraintFields
end
