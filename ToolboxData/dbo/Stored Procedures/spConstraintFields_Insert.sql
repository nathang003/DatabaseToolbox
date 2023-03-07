CREATE PROCEDURE [dbo].[spConstraintFields_Insert]
	@constraintId int,
	@fieldId int
AS
begin
	insert into dbo.ConstraintFields (ConstraintId, FieldId)
	VALUES (@constraintId, @fieldId);
end
