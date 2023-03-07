CREATE PROCEDURE [dbo].[spForeignKeys_Update]
	@foreignKeyId int,
	@purpose nvarchar(max) = null,
	@constraintFieldId int,
	@referenceFieldId int,
	@isDisabled bit = 0,
	@isNotTrusted bit = 1,
	@deleteReferencialActionDescription nvarchar(50),
	@updateReferencialActionDescription nvarchar(50)
AS
begin
	update dbo.ForeignKeys 
	set Purpose = @purpose, 
		ConstraintFieldId = @constraintFieldId, 
		ReferencedFieldId = @referenceFieldId, 
		IsDisabled = @isDisabled, 
		IsNotTrusted = @isNotTrusted, 
		DeleteReferentialActionDescription = @deleteReferencialActionDescription, 
		UpdateReferentialActionDescription = @updateReferencialActionDescription, 
		UpdatedDate = getutcdate(), 
		UpdatedBy = suser_sname()
	WHERE ForeignKeyId = @foreignKeyId;
end
