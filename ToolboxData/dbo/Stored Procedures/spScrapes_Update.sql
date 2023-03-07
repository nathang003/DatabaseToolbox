CREATE PROCEDURE [dbo].[spScrapes_Update]
	@scrapeId uniqueidentifier,
	@scrapeScope varchar(50), --
	@dynamicScrapeObject varchar(50), --
	@dynamicScrapeObjectId int,
	@scrapeScheduledDateTime datetime,
	@scrapeWorkerId uniqueidentifier,
	@scrapeStatusId int
AS
begin
	update dbo.Scrapes
	set ScrapeScheduledDateTime = @scrapeScheduledDateTime,
		ScrapeWorkerId = @scrapeWorkerId,
		ScrapeStatusId = @scrapeStatusId,
		UpdatedDate = getutcdate(),
		UpdatedBy = suser_sname()
	where ScrapeId = @scrapeId
end
