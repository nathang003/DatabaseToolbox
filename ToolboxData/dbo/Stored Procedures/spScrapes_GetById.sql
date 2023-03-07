CREATE PROCEDURE [dbo].[spScrapes_GetById]
	@scrapeId uniqueidentifier
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		ScrapeId,
		ScrapeScope,
		DynamicScrapeObject,
		DynamicScrapeObjectId,
		ScrapeScheduledDateTime,
		ScrapeWorkerId,
		ScrapeStatusId,
		CreatedDate,
		CreatedBy,
		UpdatedDate,
		UpdatedBy
	FROM dbo.Scrapes
	WHERE
		ScrapeId = @scrapeId;
END
