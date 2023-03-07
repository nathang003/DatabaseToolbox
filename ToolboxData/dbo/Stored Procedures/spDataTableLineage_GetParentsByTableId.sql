CREATE PROCEDURE [dbo].[spDataTableLineage_GetParentsByTableId]
	@TableId INT = -1
AS
BEGIN

	SET NOCOUNT ON;
	
	SELECT 
		DataTableLineageId,
		ParentTableId,
		ChildTableId,
		LineageStartDate,
		LineageEndDate,
		CreatedDate,
		CreatedBy,
		UpdatedDate,
		UpdatedBy
	FROM dbo.DataTableLineage
	WHERE ChildTableId = @TableId;

end