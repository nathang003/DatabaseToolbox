CREATE PROCEDURE [dbo].[spScrapes_Insert]
	@scrapeScope nvarchar(50),
	@dynamicScrapeObject nvarchar(50),
	@dynamicScrapeObjectId int,
	@scrapeScheduledDateTime datetime2(7)
AS
BEGIN
	DECLARE @existingScrapeCount INT = 0;

	SET @existingScrapeCount = 
		(
			SELECT COUNT(1)
			FROM dbo.Scrapes s
			WHERE @scrapeScope = s.ScrapeScope
				AND @dynamicScrapeObject = s.DynamicScrapeObject
				AND @dynamicScrapeObjectId = s.DynamicScrapeObjectId
				AND s.ScrapeStatusId NOT IN (5,6)
		)

	IF @existingScrapeCount = 0
	begin
		insert into dbo.Scrapes (ScrapeScope, DynamicScrapeObject, DynamicScrapeObjectId, ScrapeScheduledDateTime, ScrapeWorkerId, ScrapeStatusId, CreatedDate, CreatedBy, UpdatedDate, UpdatedBy)
		values (@scrapeScope, @dynamicScrapeObject, @dynamicScrapeObjectId, @scrapeScheduledDateTime, null, 2, getutcdate(), suser_sname(), getutcdate(), suser_sname())
	END
end
