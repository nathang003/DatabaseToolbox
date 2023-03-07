CREATE PROCEDURE [dbo].[spIndexFields_Insert]
	@indexId int,
	@fieldId int,
	@keyOrdinal int
AS
begin
	insert into dbo.IndexFields (IndexId, FieldId, KeyOrdinal)
	VALUES (@indexId, @fieldId, @keyOrdinal);
end
