CREATE PROCEDURE [dbo].[spConstraints_Insert]
	@serverId int,
	@databaseId int,
	@schemaId int,
	@tableId int,
	@constraintName nvarchar(200),
	@constraintDefinition nvarchar(max),
	@purpose nvarchar(max) = null,
	@constraintType nvarchar(25),
	@constraintTypeDescription nvarchar(150),
	@isSystemNamed bit = 1
AS
begin
	insert into dbo.Constraints (ServerId, DatabaseId, SchemaId, TableId, ConstraintName, ConstraintDefinition, Purpose, ConstraintType, ConstraintTypeDescription, IsSystemNamed, CreatedDate, CreatedBy, UpdatedDate, UpdatedBy)
	VALUES (@serverId, @databaseId, @schemaId, @tableId, @constraintName, @constraintDefinition, @purpose, @constraintType, @constraintTypeDescription, @isSystemNamed, getutcdate(), suser_sname(), getutcdate(), suser_sname());
end
