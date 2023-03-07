CREATE PROCEDURE [dbo].[spDatabaseObjects_GetPartialMatches]
	@searchText varchar(200)
AS
begin
	set @searchText = '%' + @searchText + '%'

	select *
	from dbo.vwDatabaseObjects
	where DatabaseObjectName like @searchText
		or DatabaseFullAddress like @searchText
		or isnull(Purpose,'') like @searchText
end
