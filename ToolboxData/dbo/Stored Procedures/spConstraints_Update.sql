CREATE PROCEDURE [dbo].[spConstraints_Update]
	@constraintId int,
	@constraintName nvarchar(200),
	@constraintDefinition nvarchar(max),
	@constraintType nvarchar(25),
	@constraintTypeDescription nvarchar(150),
	@isSystemNamed bit = 1
AS
begin
	update dbo.Constraints
		set ConstraintName = @constraintName,
			ConstraintDefinition = @constraintDefinition,
			ConstraintType = @constraintType,
			ConstraintTypeDescription = @constraintTypeDescription,
			IsSystemNamed = @isSystemNamed,
			UpdatedDate = getutcdate(),
			UpdatedBy = suser_sname()
	WHERE ConstraintId = @constraintId
end
