CREATE PROCEDURE [dbo].[spIndexes_GetAll]
AS
begin
	select
		IndexId,
		ServerId,
		DatabaseId,
		TableId,
		IndexName,
		Purpose,
		IndexType,
		IsUnique,
		CreatedDate,
		CreatedBy,
		UpdatedDate,
		UpdatedBy
	from dbo.Indexes
end
