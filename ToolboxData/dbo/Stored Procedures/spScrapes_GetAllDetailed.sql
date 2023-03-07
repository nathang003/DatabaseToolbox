CREATE PROCEDURE [dbo].[spScrapes_GetAllDetailed]
AS
BEGIN
	SELECT 
		scr.ScrapeId,
		scr.ScrapeScope,
		scr.DynamicScrapeObject,
		scr.DynamicScrapeObjectId,
		CASE LOWER(scr.DynamicScrapeObject) WHEN 'server' THEN s.ServerName
			WHEN 'database' THEN d_s.ServerName + '.' + d.DatabaseName
			WHEN 'schema' THEN sc_s.ServerName + '.' + sc_d.DatabaseName + '.' + sc.SchemaName
			WHEN 'table' THEN t_s.ServerName + '.' + t_d.DatabaseName + '.' + t_sc.SchemaName + '.' + t.TableName
			WHEN 'field' THEN f_s.ServerName + '.' + f_d.DatabaseName + '.' + f_sc.SchemaName + '.' + f_t.TableName + '.' + f.FieldName
		END AS DynamicScrapeObjectName,
		scr.ScrapeScheduledDateTime,
		scr.ScrapeWorkerId,
		scr.ScrapeStatusId,
		ss.ScrapeStatus AS ScrapeStatusName,
		scr.CreatedDate,
		scr.CreatedBy,
		scr.UpdatedDate,
		scr.UpdatedBy
	FROM dbo.Scrapes scr
		LEFT JOIN dbo.Servers s
			ON LOWER(scr.DynamicScrapeObject) = 'server'
				AND scr.DynamicScrapeObjectId = s.ServerId
		LEFT JOIN dbo.Databases d
			ON LOWER(scr.DynamicScrapeObject) = 'database'
				AND scr.DynamicScrapeObjectId = d.DatabaseId
		LEFT JOIN dbo.Servers d_s
			ON d.ServerId = d_s.ServerId
		LEFT JOIN dbo.Schemas sc
			ON LOWER(scr.DynamicScrapeObject) = 'schema'
				AND scr.DynamicScrapeObjectId = sc.SchemaId
		LEFT JOIN dbo.Servers sc_s
			ON sc.ServerId = sc_s.ServerId
		LEFT JOIN dbo.Databases sc_d
			ON sc.DatabaseId = sc_d.DatabaseId
		LEFT JOIN dbo.Tables t
			ON LOWER(scr.DynamicScrapeObject) = 'table'
				AND scr.DynamicScrapeObjectId = t.TableId
		LEFT JOIN dbo.Servers t_s
			ON t.ServerId = t_s.ServerId
		LEFT JOIN dbo.Databases t_d
			ON t.DatabaseId = t_d.DatabaseId
		LEFT JOIN dbo.Schemas t_sc
			ON t.SchemaId = t_sc.SchemaId
		LEFT JOIN dbo.Fields f
			ON LOWER(scr.DynamicScrapeObject) = 'field'
				AND scr.DynamicScrapeObjectId = f.FieldId
		LEFT JOIN dbo.Servers f_s
			ON f.ServerId = f_s.ServerId
		LEFT JOIN dbo.Databases f_d
			ON f.DatabaseId = f_d.DatabaseId
		LEFT JOIN dbo.Schemas f_sc
			ON f.SchemaId = f_sc.SchemaId
		LEFT JOIN dbo.Tables f_t
			ON f.TableId = f_t.TableId
		LEFT JOIN dbo.ScrapeStatuses ss
			ON scr.ScrapeStatusId = ss.ScrapeStatusId
end