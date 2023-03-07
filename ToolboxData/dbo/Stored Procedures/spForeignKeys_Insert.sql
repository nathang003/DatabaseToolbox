CREATE PROCEDURE [dbo].[spForeignKeys_Insert]
	@serverId int,
	@databaseId int,
	@schemaId int,
	@tableId int,
	@foreignKeyName nvarchar(200),
	@purpose nvarchar(max) = null,
	@constraintFieldId int,
	@referenceFieldId int,
	@isDisabled bit = 0,
	@isNotTrusted bit = 1,
	@deleteReferencialActionDescription nvarchar(50),
	@updateReferencialActionDescription nvarchar(50)
AS
begin
	insert into dbo.ForeignKeys (ServerId, DatabaseId, SchemaId, TableId, ForeignKeyName, Purpose, ConstraintFieldId, ReferencedFieldId, IsDisabled, IsNotTrusted, DeleteReferentialActionDescription, UpdateReferentialActionDescription, CreatedDate, CreatedBy, UpdatedDate, UpdatedBy)
	VALUES (@serverId, @databaseId, @schemaId, @tableId, @foreignKeyName, @purpose, @constraintFieldId, @referenceFieldId, @isDisabled, @isNotTrusted, @deleteReferencialActionDescription, @updateReferencialActionDescription, getutcdate(), suser_sname(), getutcdate(), suser_sname());
end
