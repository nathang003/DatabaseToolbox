CREATE PROCEDURE [dbo].[spScrapes_DeleteAll]
AS
begin
	truncate table dbo.Scrapes;
end
