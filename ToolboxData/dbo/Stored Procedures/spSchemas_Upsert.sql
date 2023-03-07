CREATE PROCEDURE [dbo].[spSchemas_Upsert]
(
	@schemas SchemaModelType readonly,
	@databaseId int
)
AS
BEGIN
	if (select count(1) from @schemas) > 0
	begin
	-----------------------------
	-- UPDATE EXISTING SCHEMAS --
	-----------------------------

		UPDATE sc
		SET sc.Purpose = scv.Purpose,
			sc.UpdatedDate = GETUTCDATE(),
			sc.UpdatedBy = SUSER_SNAME()
		FROM dbo.Schemas sc
			INNER JOIN @schemas scv
				ON sc.ServerId = scv.ServerId
				AND sc.DatabaseId = scv.DatabaseId
				AND sc.SchemaName = scv.SchemaName
		WHERE sc.Purpose <> scv.Purpose;

	------------------------
	-- INSERT NEW SCHEMAS --
	------------------------

		WITH newSchemas (ServerId, DatabaseId, SchemaName, Purpose, RemovalDate, CreatedDate, CreatedBy, UpdatedDate, UpdatedBy)
		AS
		(
			SELECT scv.ServerId,
				scv.DatabaseId,
				scv.SchemaName,
				scv.Purpose,
				scv.RemovalDate,
				scv.CreatedDate,
				scv.CreatedBy,
				scv.UpdatedDate,
				scv.UpdatedBy
			FROM @schemas scv
				LEFT JOIN dbo.Schemas sc
					ON scv.ServerId = sc.ServerId
					AND scv.DatabaseId = sc.DatabaseId
					AND scv.SchemaName = sc.SchemaName
			WHERE sc.ServerId IS NULL
		)

		INSERT INTO dbo.Schemas (ServerId, DatabaseId, SchemaName, Purpose, RemovalDate, CreatedDate, CreatedBy, UpdatedDate, UpdatedBy)
		SELECT ServerId, 
			DatabaseId,
			SchemaName,
			Purpose,
			null,
			GETUTCDATE(),
			SUSER_SNAME(),
			GETUTCDATE(),
			SUSER_SNAME()
		FROM newSchemas;

	------------------------------
	-- UPDATE REMOVED DATABASES --
	------------------------------

		WITH removedSchemas (ServerId, DatabaseId, SchemaId, SchemaName, Purpose, RemovalDate, CreatedDate, CreatedBy, UpdatedDate, UpdatedBy)
		AS
		(
			SELECT sc.ServerId,
				sc.DatabaseId, 
				sc.SchemaId,
				sc.SchemaName, 
				sc.Purpose, 
				sc.RemovalDate, 
				sc.CreatedDate, 
				sc.CreatedBy, 
				sc.UpdatedDate, 
				sc.UpdatedBy
			FROM dbo.Schemas sc
				LEFT JOIN @schemas scv
					ON sc.ServerId = scv.ServerId
					AND sc.DatabaseId = scv.DatabaseId
					AND sc.SchemaName = scv.SchemaName
			WHERE scv.ServerId IS NULL
				AND sc.DatabaseId = @databaseId
				AND sc.RemovalDate IS null
		)

		UPDATE sc
		SET sc.RemovalDate = GETUTCDATE(),
			sc.UpdatedDate = GETUTCDATE(),
			sc.UpdatedBy = SUSER_SNAME()
		FROM dbo.Schemas sc
			INNER JOIN removedSchemas rsc
				ON sc.SchemaId = rsc.SchemaId
	end

------------------------------------------------
-- UPDATE DATABASE TO REFLECT SCRAPE ACTIVITY --
------------------------------------------------

	UPDATE dbo.Databases
	SET UpdatedDate = GETUTCDATE(),
		UpdatedBy = SUSER_SNAME()
	WHERE DatabaseId = @databaseId

END
