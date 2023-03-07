CREATE PROCEDURE [dbo].[spIndexes_Insert]
	@serverId int,
	@databaseId int,
	@schemaId int,
	@tableId int,
	@indexName nvarchar(200),
	@purpose nvarchar(max),
	@indexType int,
	@indexTypeDescription nvarchar(25),
	@isPrimaryKey bit,
	@isUniqueConstraint bit,
	@isUnique bit,
	@isDisabled bit
AS
begin
	insert into dbo.Indexes (ServerId, DatabaseId, SchemaId, TableId, IndexName, Purpose, IndexType, IndexTypeDescription, IsPrimaryKey, IsUniqueConstraint, IsUnique, IsDisabled, CreatedDate, CreatedBy, UpdatedDate, UpdatedBy)
	VALUES (@serverId, @databaseId, @schemaId, @tableId, @indexName, @purpose, @indexType, @indexTypeDescription, @isPrimaryKey, @isUniqueConstraint, @isUnique, @isDisabled, getutcdate(), suser_sname(), getutcdate(), suser_sname());
end
