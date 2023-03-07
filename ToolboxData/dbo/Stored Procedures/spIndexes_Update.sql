CREATE PROCEDURE [dbo].[spIndexes_Update]
	@tableId int,
	@purpose nvarchar(max),
	@indexType int,
	@indexTypeDescription nvarchar(25),
	@isPrimaryKey bit = 0,
	@isUniqueConstraint bit = 0,
	@isUnique bit = 0,
	@isDisabled bit = 0
AS
begin
	update dbo.Indexes
	set Purpose = @purpose,
		IndexType = @indexType,
		IndexTypeDescription = @indexTypeDescription,
		IsPrimaryKey = @isPrimaryKey,
		IsUniqueConstraint = @isUniqueConstraint,
		IsUnique = @isUnique,
		IsDisabled = @isDisabled,
		UpdatedDate = getutcdate(),
		UpdatedBy = suser_sname()
	where TableId = @tableId;
end
