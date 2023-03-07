CREATE PROCEDURE [dbo].[spConstraints_GetByTableId]
	@tableId int
AS
begin
	select
		c.ConstraintId,
		c.ServerId,
		c_s.ServerName,
		c.DatabaseId,
		c_d.DatabaseName,
		c.SchemaId,
		c_sc.SchemaName,
		c.TableId,
		c_t.TableName,
		c.ConstraintName,
		c.ConstraintDefinition,
		c.Purpose,
		c.ConstraintType,
		c.ConstraintTypeDescription,
		c.IsSystemNamed,
		c.CreatedDate,
		c.CreatedBy,
		c.UpdatedDate,
		c.UpdatedBy
	from dbo.Constraints c
		INNER JOIN dbo.Servers c_s
			ON c.ServerId = c_s.ServerId
		INNER JOIN dbo.Databases c_d
			ON c.DatabaseId = c_d.DatabaseId
		INNER JOIN dbo.Schemas c_sc
			ON c.SchemaId = c_sc.SchemaId
		INNER JOIN dbo.Tables c_t
			ON c.TableId = c_t.TableId
	WHERE c.TableId = @tableId
end
