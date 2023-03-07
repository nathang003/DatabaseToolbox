CREATE PROCEDURE [dbo].[spScrapes_GetAll]
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
	FROM dbo.Scrapes;
END
