CREATE PROCEDURE [dbo].[spDataTableLineage_GetChildrenByTableId]
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
	WHERE ParentTableId = @TableId;

end