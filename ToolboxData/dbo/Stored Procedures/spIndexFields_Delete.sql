CREATE PROCEDURE [dbo].[spIndexFields_Delete]
	@indexFieldId int
AS
begin
	delete from dbo.IndexFields
	where IndexFieldId = @indexFieldId;
end
