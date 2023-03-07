CREATE PROCEDURE [dbo].[spConstraintFields_GetByConstraintId]
	@constraintId int
AS
begin
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
	where
		ConstraintId = @constraintId;
end
