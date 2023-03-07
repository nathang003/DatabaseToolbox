CREATE VIEW [dbo].[vwSuggestedScrapes]
	AS 

SELECT *
	, ROW_NUMBER() OVER (ORDER BY suggestions.ScrapePriorityLevel ASC) AS SuggestionRank
FROM (
SELECT distinct
	'Server' AS ScrapeScope,
	'Server' AS DynamicScrapeObject,
	s.ServerId AS DynamicScrapeObjectId,
	s.ServerName AS DynamicScrapeObjectName,
	s.CreatedDate AS ScrapeObjectCreatedDate,
	s.UpdatedDate AS ScrapeObjectUpdatedDate,
	DATEDIFF(DAY, s.CreatedDate, getutcdate()) AS DaysSinceCreation,
	DATEDIFF(DAY, s.UpdatedDate, getutcdate()) AS DaysSinceLastUpdate,
	CASE WHEN s_d.ServerId IS NULL AND (scr2.LatestUpdateDate IS NULL OR DATEDIFF(DAY, scr2.LatestUpdateDate, getutcdate()) > 90) THEN 1 
		WHEN DATEDIFF(DAY, s.UpdatedDate, getutcdate()) > 90 THEN 9
		ELSE 17 END AS ScrapePriorityLevel
FROM dbo.Servers s
	LEFT JOIN dbo.Databases s_d
		ON s.ServerId = s_d.ServerId
	LEFT JOIN 
		(
			SELECT scr.DynamicScrapeObjectId, MAX(scr.UpdatedDate) LatestUpdateDate
			FROM dbo.Scrapes scr
			WHERE scr.DynamicScrapeObject = 'Server'
				AND scr.ScrapeStatusId = 5
			GROUP BY scr.DynamicScrapeObjectId 
		) scr2
		ON s.ServerId = scr2.DynamicScrapeObjectId
UNION
SELECT distinct
	'Database' AS ScrapeScope,
	'Database' AS DynamicScrapeObject,
	d.DatabaseId AS DynamicScrapeObjectId,
	d_s.ServerName + '.' + d.DatabaseName AS DynamicScrapeObjectName,
	d.CreatedDate AS ScrapeObjectCreatedDate,
	d.UpdatedDate AS ScrapeObjectUpdatedDate,
	DATEDIFF(DAY, d.CreatedDate, GETUTCDATE()) AS DaysSinceCreation,
	DATEDIFF(DAY, d.UpdatedDate, GETUTCDATE()) AS DaysSinceLastUpdate,
	CASE WHEN d_sc.DatabaseId IS NULL AND (scr2.LatestUpdateDate IS NULL OR DATEDIFF(DAY, scr2.LatestUpdateDate, GETUTCDATE()) > 90) THEN 2 
		WHEN DATEDIFF(DAY, d.UpdatedDate, GETUTCDATE()) > 90 THEN 10
		ELSE 18 END AS ScrapePriorityLevel
FROM dbo.Databases d
	LEFT JOIN dbo.Schemas d_sc
		ON d.DatabaseId = d_sc.DatabaseId
	LEFT JOIN dbo.Servers d_s
		ON d.ServerId = d_s.ServerId
	LEFT JOIN 
		(
			SELECT scr.DynamicScrapeObjectId, MAX(scr.UpdatedDate) LatestUpdateDate
			FROM dbo.Scrapes scr
			WHERE scr.DynamicScrapeObject = 'Database'
				AND scr.ScrapeStatusId = 5
			GROUP BY scr.DynamicScrapeObjectId 
		) scr2
		ON d.DatabaseId = scr2.DynamicScrapeObjectId
WHERE d.RemovalDate IS NULL
UNION
SELECT distinct
	'Schema' AS ScrapeScope,
	'Schema' AS DynamicScrapeObject,
	sc.SchemaId AS DynamicScrapeObjectId,
	sc_s.ServerName + '.' + sc_d.DatabaseName + '.' + sc.SchemaName AS DynamicScrapeObjectName,
	sc.CreatedDate AS ScrapeObjectCreatedDate,
	sc.UpdatedDate AS ScrapeObjectUpdatedDate,
	DATEDIFF(DAY, sc.CreatedDate, getutcdate()) AS DaysSinceCreation,
	DATEDIFF(DAY, sc.UpdatedDate, getutcdate()) AS DaysSinceLastUpdate,
	CASE WHEN sc_t.SchemaId IS NULL AND (scr2.LatestUpdateDate IS NULL OR DATEDIFF(DAY, scr2.LatestUpdateDate, getutcdate()) > 90) then 3
		WHEN DATEDIFF(DAY, sc.UpdatedDate, getutcdate()) > 90 THEN 11
		ELSE 19 END AS ScrapePriorityLevel
FROM dbo.Schemas sc
	LEFT JOIN dbo.Tables sc_t
		ON sc.SchemaId = sc_t.SchemaId
	LEFT JOIN dbo.Servers sc_s
		ON sc.ServerId = sc_s.ServerId
	LEFT JOIN dbo.Databases sc_d
		ON sc.DatabaseId = sc_d.DatabaseId
	LEFT JOIN 
		(
			SELECT scr.DynamicScrapeObjectId, MAX(scr.UpdatedDate) LatestUpdateDate
			FROM dbo.Scrapes scr
			WHERE scr.DynamicScrapeObject = 'Schema'
				AND scr.ScrapeStatusId = 5
			GROUP BY scr.DynamicScrapeObjectId 
		) scr2
		ON sc.SchemaId = scr2.DynamicScrapeObjectId
WHERE sc_d.RemovalDate IS NULL
	AND sc.RemovalDate IS NULL
UNION
SELECT DISTINCT
	'Table' AS ScrapeScope,
	'Table' AS DynamicScrapeObject,
	t.TableId AS DynamicScrapeObjectId,
	t_s.ServerName + '.' + t_d.DatabaseName + '.' + t_sc.SchemaName + '.' + t.TableName AS DynamicScrapeObjectName,
	t.CreatedDate AS ScrapeObjectCreatedDate,
	t.UpdatedDate AS ScrapeObjectUpdatedDate,
	DATEDIFF(DAY, t.CreatedDate, GETUTCDATE()) AS DaysSinceCreation,
	DATEDIFF(DAY, t.UpdatedDate, GETUTCDATE()) AS DaysSinceLastUpdate,
	CASE WHEN t_f.TableId IS NULL AND (scr2.LatestUpdateDate IS NULL OR DATEDIFF(DAY, scr2.LatestUpdateDate, GETUTCDATE()) > 90) THEN 4
		WHEN DATEDIFF(DAY, t.UpdatedDate, GETUTCDATE()) > 90 THEN 12
		ELSE 20 END AS ScrapePriorityLevel
FROM dbo.Tables t
	LEFT JOIN dbo.Fields t_f
		ON t.TableId = t_f.TableId
	LEFT JOIN dbo.Servers t_s
		ON t.ServerId = t_s.ServerId
	LEFT JOIN dbo.Databases t_d
		ON t.DatabaseId = t_d.DatabaseId
	LEFT JOIN dbo.Schemas t_sc
		ON t.SchemaId = t_sc.SchemaId
	LEFT JOIN 
		(
			SELECT scr.DynamicScrapeObjectId, MAX(scr.UpdatedDate) LatestUpdateDate
			FROM dbo.Scrapes scr
			WHERE scr.ScrapeScope = 'Table'
				AND scr.ScrapeStatusId = 5
			GROUP BY scr.DynamicScrapeObjectId 
		) scr2
		ON t.TableId = scr2.DynamicScrapeObjectId
WHERE t_d.RemovalDate IS NULL
	AND t_sc.RemovalDate IS NULL
	AND t.RemovalDate IS NULL
UNION
SELECT DISTINCT
	'Index' AS ScrapeScope,
	'Table' AS DynamicScrapeObject,
	t.TableId AS DynamicScrapeObjectId,
	t_s.ServerName + '.' + t_d.DatabaseName + '.' + t_sc.SchemaName + '.' + t.TableName AS DynamicScrapeObjectName,
	t.CreatedDate AS ScrapeObjectCreatedDate,
	t.UpdatedDate AS ScrapeObjectUpdatedDate,
	DATEDIFF(DAY, t.CreatedDate, GETUTCDATE()) AS DaysSinceCreation,
	DATEDIFF(DAY, t.UpdatedDate, GETUTCDATE()) AS DaysSinceLastUpdate,
	CASE WHEN t_f.TableId IS NULL AND (scr2.LatestUpdateDate IS NULL OR DATEDIFF(DAY, scr2.LatestUpdateDate, GETUTCDATE()) > 90) THEN 6
		WHEN DATEDIFF(DAY, t.UpdatedDate, GETUTCDATE()) > 90 THEN 14
		ELSE 22 END AS ScrapePriorityLevel
FROM dbo.Tables t
	LEFT JOIN dbo.Fields t_f
		ON t.TableId = t_f.TableId
	LEFT JOIN dbo.Servers t_s
		ON t.ServerId = t_s.ServerId
	LEFT JOIN dbo.Databases t_d
		ON t.DatabaseId = t_d.DatabaseId
	LEFT JOIN dbo.Schemas t_sc
		ON t.SchemaId = t_sc.SchemaId
	LEFT JOIN 
		(
			SELECT scr.DynamicScrapeObjectId, MAX(scr.UpdatedDate) LatestUpdateDate
			FROM dbo.Scrapes scr
			WHERE scr.ScrapeScope = 'Index'
				AND scr.ScrapeStatusId = 5
			GROUP BY scr.DynamicScrapeObjectId 
		) scr2
		ON t.TableId = scr2.DynamicScrapeObjectId
WHERE t_d.RemovalDate IS NULL
	AND t_sc.RemovalDate IS NULL
	AND t.RemovalDate IS NULL
UNION
SELECT DISTINCT
	'Foreign Key' AS ScrapeScope,
	'Table' AS DynamicScrapeObject,
	t.TableId AS DynamicScrapeObjectId,
	t_s.ServerName + '.' + t_d.DatabaseName + '.' + t_sc.SchemaName + '.' + t.TableName AS DynamicScrapeObjectName,
	t.CreatedDate AS ScrapeObjectCreatedDate,
	t.UpdatedDate AS ScrapeObjectUpdatedDate,
	DATEDIFF(DAY, t.CreatedDate, GETUTCDATE()) AS DaysSinceCreation,
	DATEDIFF(DAY, t.UpdatedDate, GETUTCDATE()) AS DaysSinceLastUpdate,
	CASE WHEN t_f.TableId IS NULL AND (scr2.LatestUpdateDate IS NULL OR DATEDIFF(DAY, scr2.LatestUpdateDate, GETUTCDATE()) > 90) THEN 7
		WHEN DATEDIFF(DAY, t.UpdatedDate, GETUTCDATE()) > 90 THEN 15
		ELSE 23 END AS ScrapePriorityLevel
FROM dbo.Tables t
	LEFT JOIN dbo.Fields t_f
		ON t.TableId = t_f.TableId
	LEFT JOIN dbo.Servers t_s
		ON t.ServerId = t_s.ServerId
	LEFT JOIN dbo.Databases t_d
		ON t.DatabaseId = t_d.DatabaseId
	LEFT JOIN dbo.Schemas t_sc
		ON t.SchemaId = t_sc.SchemaId
	LEFT JOIN 
		(
			SELECT scr.DynamicScrapeObjectId, MAX(scr.UpdatedDate) LatestUpdateDate
			FROM dbo.Scrapes scr
			WHERE scr.ScrapeScope = 'Foreign Key'
				AND scr.ScrapeStatusId = 5
			GROUP BY scr.DynamicScrapeObjectId 
		) scr2
		ON t.TableId = scr2.DynamicScrapeObjectId
WHERE t_d.RemovalDate IS NULL
	AND t_sc.RemovalDate IS NULL
	AND t.RemovalDate IS NULL
UNION
SELECT DISTINCT
	'Constraint' AS ScrapeScope,
	'Table' AS DynamicScrapeObject,
	t.TableId AS DynamicScrapeObjectId,
	t_s.ServerName + '.' + t_d.DatabaseName + '.' + t_sc.SchemaName + '.' + t.TableName AS DynamicScrapeObjectName,
	t.CreatedDate AS ScrapeObjectCreatedDate,
	t.UpdatedDate AS ScrapeObjectUpdatedDate,
	DATEDIFF(DAY, t.CreatedDate, GETUTCDATE()) AS DaysSinceCreation,
	DATEDIFF(DAY, t.UpdatedDate, GETUTCDATE()) AS DaysSinceLastUpdate,
	CASE WHEN t_f.TableId IS NULL AND (scr2.LatestUpdateDate IS NULL OR DATEDIFF(DAY, scr2.LatestUpdateDate, GETUTCDATE()) > 90) THEN 8
		WHEN DATEDIFF(DAY, t.UpdatedDate, GETUTCDATE()) > 90 THEN 16
		ELSE 24 END AS ScrapePriorityLevel
FROM dbo.Tables t
	LEFT JOIN dbo.Fields t_f
		ON t.TableId = t_f.TableId
	LEFT JOIN dbo.Servers t_s
		ON t.ServerId = t_s.ServerId
	LEFT JOIN dbo.Databases t_d
		ON t.DatabaseId = t_d.DatabaseId
	LEFT JOIN dbo.Schemas t_sc
		ON t.SchemaId = t_sc.SchemaId
	LEFT JOIN 
		(
			SELECT scr.DynamicScrapeObjectId, MAX(scr.UpdatedDate) LatestUpdateDate
			FROM dbo.Scrapes scr
			WHERE scr.ScrapeScope = 'Constraint'
				AND scr.ScrapeStatusId = 5
			GROUP BY scr.DynamicScrapeObjectId 
		) scr2
		ON t.TableId = scr2.DynamicScrapeObjectId
WHERE t_d.RemovalDate IS NULL
	AND t_sc.RemovalDate IS NULL
	AND t.RemovalDate IS NULL
UNION
SELECT DISTINCT
	'Field' AS ScrapeScope,
	'Field' AS DynamicScrapeObject,
	f.FieldId AS DynamicScrapeObjectId,
	f_s.ServerName + '.' + f_d.DatabaseName + '.' + f_sc.SchemaName + '.' + f_t.TableName + '.' + f.FieldName AS DynamicScrapeObjectName,
	f.CreatedDate AS ScrapeObjectCreatedDate,
	f.UpdatedDate AS ScrapeObjectUpdatedDate,
	DATEDIFF(DAY, f.CreatedDate, getutcdate()) AS DaysSinceCreation,
	DATEDIFF(DAY, f.UpdatedDate, getutcdate()) AS DaysSinceLastUpdate,
	CASE WHEN f.SampleValue1 IS NULL AND (scr2.LatestUpdateDate IS NULL OR DATEDIFF(DAY, scr2.LatestUpdateDate, getutcdate()) > 90) THEN 5
		WHEN DATEDIFF(DAY, f.UpdatedDate, getutcdate()) > 90 THEN 13
		ELSE 21 END AS ScrapePriorityLevel
FROM dbo.Fields f
	LEFT JOIN dbo.Servers f_s
		ON f.ServerId = f_s.ServerId
	LEFT JOIN dbo.Databases f_d
		ON f.DatabaseId = f_d.DatabaseId
	LEFT JOIN dbo.Schemas f_sc
		ON f.SchemaId = f_sc.SchemaId
	LEFT JOIN dbo.Tables f_t
		ON f.TableId = f_t.TableId
	LEFT JOIN 
		(
			SELECT scr.DynamicScrapeObjectId, MAX(scr.UpdatedDate) LatestUpdateDate
			FROM dbo.Scrapes scr
			WHERE scr.DynamicScrapeObject = 'Field'
				AND scr.ScrapeStatusId = 5
			GROUP BY scr.DynamicScrapeObjectId 
		) scr2
		ON f.FieldId = scr2.DynamicScrapeObjectId
WHERE f_d.RemovalDate IS NULL
	AND f_sc.RemovalDate IS NULL
	AND f_t.RemovalDate IS NULL
	AND f.RemovalDate IS NULL
) suggestions

