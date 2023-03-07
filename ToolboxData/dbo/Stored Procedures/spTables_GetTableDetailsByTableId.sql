CREATE PROCEDURE [dbo].[spTables_GetTableDetailsByTableId]
	@tableId int = 0
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
        t.TableId,
        t.[ServerId],
        t.[DatabaseId],
        t.SchemaId,
        t.[TableName],
        t.[Purpose],
        t.[RemovalDate],
        t.[CreatedDate],
        t.[CreatedBy],
        t.[UpdatedDate],
        t.[UpdatedBy],
        s.ServerName,
        d.DatabaseName,
        sc.SchemaName
	FROM dbo.Tables t
        inner join dbo.Schemas sc
            on t.SchemaId = sc.SchemaId
        inner join dbo.Databases d
            on t.DatabaseId = d.DatabaseId
        inner join dbo.Servers s
            on t.ServerId = s.ServerId
	WHERE [TableId] = @tableId;
END
-- comment