CREATE PROCEDURE [dbo].[spForeignKeys_GetAll]
AS
begin
	select
		ForeignKeyId,
		ServerId,
		DatabaseId,
		SchemaId,
		TableId,
		ForeignKeyName,
		Purpose,
		ConstraintFieldId,
		ReferencedFieldId,
		IsDisabled,
		IsNotTrusted,
		DeleteReferentialActionDescription,
		UpdateReferentialActionDescription,
		CreatedDate,
		CreatedBy,
		UpdatedDate,
		UpdatedBy
	from dbo.ForeignKeys
end
