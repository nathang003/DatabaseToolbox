CREATE PROCEDURE [dbo].[spScrapes_Delete]
	@scrapeId uniqueidentifier
AS
begin
	delete from dbo.Scrapes
	where ScrapeId = @scrapeId;
end
