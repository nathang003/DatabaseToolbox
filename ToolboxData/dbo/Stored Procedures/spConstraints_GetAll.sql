CREATE PROCEDURE [dbo].[spConstraints_GetAll]
AS
begin
	select
		ConstraintId,
		ServerId,
		DatabaseId,
		SchemaId,
		TableId,
		ConstraintName,
		ConstraintDefinition,
		Purpose,
		ConstraintType,
		ConstraintTypeDescription,
		IsSystemNamed,
		CreatedDate,
		CreatedBy,
		UpdatedDate,
		UpdatedBy
	from dbo.Constraints
end
