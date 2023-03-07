CREATE PROCEDURE [dbo].[spDataTableLineage_GetAll]
AS
BEGIN

	SELECT DataTableLineageId,
		ParentTableId,
		ChildTableId,
		LineageStartDate,
		LineageEndDate,
		CreatedBy,
		CreatedDate,
		UpdatedBy,
		UpdatedDate
	from dbo.DataTableLineage

END