CREATE PROCEDURE [dbo].[spScrapes_GetTopSuggestedDetailed]
AS
BEGIN
	SELECT TOP 300 *
	FROM dbo.vwSuggestedScrapes
	WHERE ScrapePriorityLevel <= 16
end