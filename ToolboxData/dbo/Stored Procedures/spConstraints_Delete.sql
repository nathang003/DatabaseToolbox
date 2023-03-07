CREATE PROCEDURE [dbo].[spConstraints_Delete]
	@constraintId int
AS
begin
-- delete constraint field records tied to the constraint id
	exec dbo.spConstraintFields_Delete @constraintId;

-- delete the constraint
	delete from dbo.Constraints
	where ConstraintId = @constraintId;
end
