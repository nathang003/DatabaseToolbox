CREATE PROCEDURE dbo.spTables_GetByNames 
@ServerName varchar(200),
@DatabaseName VARCHAR(200),
@SchemaName VARCHAR(200),
@TableName VARCHAR(200)
AS
begin
SELECT 
	t.TableId,
	t.ServerId,
	t.DatabaseId,
	t.SchemaId,
	t.TableName,
	t.Purpose,
	t.RemovalDate,
	t.CreatedDate,
	t.CreatedBy,
	t.UpdatedDate,
	t.UpdatedBy
FROM dbo.Servers se
	INNER JOIN dbo.Databases d
		ON se.ServerId = d.ServerId
	inner JOIN dbo.Schemas sc
		ON d.DatabaseId = sc.DatabaseId
	inner JOIN dbo.Tables t
		ON sc.SchemaId = t.SchemaId
WHERE
	se.ServerName = @ServerName
	AND d.DatabaseName = @DatabaseName
	AND sc.SchemaName = @SchemaName
	AND t.TableName = @TableName
	end