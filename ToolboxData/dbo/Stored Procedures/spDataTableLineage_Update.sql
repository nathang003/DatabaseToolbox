CREATE PROCEDURE [dbo].[spDataTableLineage_Update]
	@dataTableLineageId int = -1,
	@parentTableId int = -1,
	@childTableId int = -1,
	@lineageStartDate date,
	@lineageEndDate date
AS
	-- VALIDATE PARAMETERS
	IF @lineageStartDate < '2017-01-01'
		OR @dataTableLineageId = -1
		OR @parentTableId = -1
		OR @childTableId = -1
		OR (@lineageEndDate IS NOT NULL 
			AND @lineageEndDate < @lineageStartDate)

		RETURN -1

	ELSE
	BEGIN
		
		DECLARE @sql NVARCHAR(MAX) =
		'UPDATE dbo.DataTableLineage
		SET ParentTableId = ' + @parentTableId + ',
			ChildTableId = ' + @childTableId + ',
			LineageStartDate = ' + QUOTENAME(@lineageStartDate)
			
		IF (@lineageEndDate IS NOT NULL)
			SET @sql = @sql + ', LineageEndDate = ' + QUOTENAME(@lineageEndDate)

		SET @sql = @sql + ' WHERE Id = ' + @dataTableLineageId

		EXEC @sql

		RETURN 0

	END

RETURN 0
