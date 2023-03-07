CREATE PROCEDURE [dbo].[spScrapes_GetSuggestionCount]

AS
BEGIN
	SELECT COUNT(1)
	FROM dbo.vwSuggestedScrapes
END