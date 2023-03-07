CREATE PROCEDURE [dbo].[spIndexes_Delete]
	@indexId int
AS
begin
	delete from dbo.Indexes
	where IndexId = @indexId;
end
