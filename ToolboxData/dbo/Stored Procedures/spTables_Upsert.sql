-- commment

CREATE PROCEDURE [dbo].[spTables_Upsert]
(
	@tables TableModelType readonly,
	@schemaId int
)
AS
BEGIN

	if (select count(1) from @tables) > 0
	begin
	----------------------------
	-- UPDATE EXISTING TABLES --
	----------------------------

		UPDATE t
		SET t.Purpose = tv.Purpose,
			t.UpdatedDate = GETUTCDATE(),
			t.UpdatedBy = SUSER_SNAME()
		FROM dbo.Tables t
			INNER JOIN @tables tv
				ON t.ServerId = tv.ServerId
				AND t.DatabaseId = tv.DatabaseId
				AND t.SchemaId = tv.SchemaId
				AND t.TableName = tv.TableName
		WHERE t.Purpose <> tv.Purpose;

	-----------------------
	-- INSERT NEW TABLES --
	-----------------------

		WITH newTables (ServerId, DatabaseId, SchemaId, TableName, Purpose, RemovalDate, CreatedDate, CreatedBy, UpdatedDate, UpdatedBy)
		AS
		(
			SELECT tv.ServerId,
				tv.DatabaseId,
				tv.SchemaId,
				tv.TableName,
				tv.Purpose,
				tv.RemovalDate,
				tv.CreatedDate,
				tv.CreatedBy,
				tv.UpdatedDate,
				tv.UpdatedBy
			FROM @tables tv
				LEFT JOIN dbo.Tables t
					ON tv.ServerId = t.ServerId
					AND tv.DatabaseId = t.DatabaseId
					AND t.SchemaId = tv.SchemaId
					AND t.TableName = tv.TableName
			WHERE t.ServerId IS NULL
		)

		INSERT INTO dbo.Tables (ServerId, DatabaseId, SchemaId, TableName, Purpose, RemovalDate, CreatedDate, CreatedBy, UpdatedDate, UpdatedBy)
		SELECT ServerId, 
			DatabaseId,
			SchemaId,
			TableName,
			Purpose,
			null,
			GETUTCDATE(),
			SUSER_SNAME(),
			GETUTCDATE(),
			SUSER_SNAME()
		FROM newTables;

	---------------------------
	-- UPDATE REMOVED TABLES --
	---------------------------

		WITH removedTables (ServerId, DatabaseId, SchemaId, TableId, TableName, Purpose, RemovalDate, CreatedDate, CreatedBy, UpdatedDate, UpdatedBy)
		AS
		(
			SELECT t.ServerId,
				t.DatabaseId, 
				t.SchemaId,
				t.TableId,
				t.TableName, 
				t.Purpose, 
				t.RemovalDate, 
				t.CreatedDate, 
				t.CreatedBy, 
				t.UpdatedDate, 
				t.UpdatedBy
			FROM dbo.Tables t
				LEFT JOIN @tables tv
					ON t.ServerId = tv.ServerId
					AND t.DatabaseId = tv.DatabaseId
					AND t.SchemaId = tv.SchemaId
					AND t.TableName = tv.TableName
			WHERE tv.ServerId IS NULL
				AND t.SchemaId = @schemaId
				AND t.RemovalDate IS null
		)

		UPDATE t
		SET t.RemovalDate = GETUTCDATE(),
			t.UpdatedDate = GETUTCDATE(),
			t.UpdatedBy = SUSER_SNAME()
		FROM dbo.Tables t
			INNER JOIN removedTables rt
				ON t.TableId = rt.TableId
	end
------------------------------------------------
-- UPDATE SCHEMA TO REFLECT SCRAPE ACTIVITY --
------------------------------------------------

	UPDATE dbo.Schemas
	SET UpdatedDate = GETUTCDATE(),
		UpdatedBy = SUSER_SNAME()
	WHERE SchemaId = @schemaId

END
