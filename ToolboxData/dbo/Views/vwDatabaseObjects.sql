CREATE VIEW [dbo].[vwDatabaseObjects]
	AS 
SELECT
	s.ServerId DatabaseObjectId,
	'Server' DatabaseObjectType,
	s.ServerName DatabaseObjectName,
	s.ServerName DatabaseFullAddress,
	ISNULL(s.Purpose, '') Purpose
FROM dbo.Servers s
UNION
SELECT
	d.DatabaseId DatabaseObjectId,
	'Database' DatabaseObjectType,
	d.DatabaseName DatabaseObjectName,
	s.ServerName + '.' + d.DatabaseName DatabaseFullAddress,
	ISNULL(d.Purpose, '') Purpose
FROM dbo.Databases d
	inner JOIN dbo.Servers s
		ON d.ServerId = s.ServerId
UNION
SELECT
	sc.SchemaId DatabaseObjectId,
	'Schema' DatabaseObjectType,
	sc.SchemaName DatabaseObjectName,
	s.ServerName + '.' + d.DatabaseName + '.' + sc.SchemaName DatabaseFullAddress,
	ISNULL(sc.Purpose, '') Purpose
FROM dbo.Schemas sc
	inner JOIN dbo.Databases d
		ON sc.DatabaseId = d.DatabaseId
	INNER JOIN dbo.Servers s
		ON s.ServerId = d.ServerId
UNION
SELECT
	t.TableId DatabaseObjectId,
	'Table' DatabaseObjectType,
	t.TableName DatabaseObjectName,
	s.ServerName + '.' + d.DatabaseName + '.' + sc.SchemaName + '.' + t.TableName DatabaseFullAddress,
	ISNULL(t.Purpose, '') Purpose
FROM dbo.Tables t
	inner JOIN dbo.Schemas sc
		ON t.SchemaId = sc.SchemaId
	INNER JOIN dbo.Databases d
		ON sc.DatabaseId = d.DatabaseId
	INNER JOIN dbo.Servers s
		ON d.ServerId = s.ServerId
union
SELECT
	f.FieldId DatabaseObjectId,
	'Field' DatabaseObjectType,
	f.FieldName DatabaseObjectName,
	s.ServerName + '.' + d.DatabaseName + '.' + sc.SchemaName + '.' + t.TableName + '.' + f.FieldName DatabaseFullAddress,
	ISNULL(f.Purpose, '') Purpose
FROM dbo.Fields f
	INNER JOIN dbo.Tables t
		ON f.TableId = t.TableId
	inner JOIN dbo.Schemas sc
		ON t.SchemaId = sc.SchemaId
	INNER JOIN dbo.Databases d
		ON sc.DatabaseId = d.DatabaseId
	INNER JOIN dbo.Servers s
		ON d.ServerId = s.ServerId