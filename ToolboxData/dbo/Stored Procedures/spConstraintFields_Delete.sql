CREATE PROCEDURE [dbo].[spConstraintFields_Delete]
	@constraintFieldId int
AS
begin
	delete from dbo.ConstraintFields
	where ConstraintFieldId = @constraintFieldId;
end
