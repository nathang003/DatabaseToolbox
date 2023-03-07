CREATE PROCEDURE [dbo].[spIndexFields_DeleteByIndexId]
	@indexId int
AS
begin
	delete from dbo.IndexFields
	where IndexId = @indexId;
end
