CREATE PROCEDURE [dbo].[spIndexes_DeleteByTableId]
	@tableId int
AS
	delete from dbo.Indexes
	where TableId = @tableId;
RETURN 0
