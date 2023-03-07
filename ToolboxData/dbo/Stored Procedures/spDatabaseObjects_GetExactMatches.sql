CREATE PROCEDURE [dbo].[spDatabaseObjects_GetExactMatches]
	@searchText varchar(200)
AS
begin
	select *
	from dbo.vwDatabaseObjects
	where DatabaseObjectName = @searchText
		or DatabaseFullAddress = @searchText
		or isnull(Purpose,'') = @searchText
end
