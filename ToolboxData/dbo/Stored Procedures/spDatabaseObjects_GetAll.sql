CREATE PROCEDURE [dbo].[spDatabaseObjects_GetAll]
AS
BEGIN
	SET NOCOUNT ON;

	select *
	from dbo.vwDatabaseObjects			
	ORDER BY 
		DatabaseFullAddress ASC;
END

