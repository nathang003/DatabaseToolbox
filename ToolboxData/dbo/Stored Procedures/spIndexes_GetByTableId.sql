CREATE PROCEDURE [dbo].[spIndexes_GetByTableId]
	@tableId int
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
	WHERE TableId = @tableId
end
