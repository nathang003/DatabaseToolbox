CREATE PROCEDURE [dbo].[spScrapes_GetUnassignedScrapeCount]
AS
begin
	select count(1)
	from dbo.Scrapes scr
	where scr.ScrapeWorkerId is null
end