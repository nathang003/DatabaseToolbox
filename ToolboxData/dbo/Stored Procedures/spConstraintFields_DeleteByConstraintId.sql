CREATE PROCEDURE [dbo].[spConstraintFields_DeleteByConstraintId]
	@constraintId int
AS
begin
	delete from dbo.ConstraintFields
	where ConstraintId = @constraintId;
end
