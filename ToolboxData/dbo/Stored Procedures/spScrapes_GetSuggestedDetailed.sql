CREATE PROCEDURE [dbo].[spScrapes_GetSuggestedDetailed]

AS
BEGIN
	SELECT *
	FROM dbo.vwSuggestedScrapes
	WHERE ScrapePriorityLevel <= 16
end