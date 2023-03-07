CREATE PROCEDURE [dbo].[spDatabases_Upsert]
(
	@databases DatabaseModelType readonly,
	@serverId int
)
AS
BEGIN

	if (select count(1) from @databases) > 0
	begin
	-------------------------------
	-- UPDATE EXISTING DATABASES --
	-------------------------------

		UPDATE d
		SET d.Purpose = ed.Purpose,
			d.UpdatedDate = GETUTCDATE(),
			d.UpdatedBy = SUSER_SNAME()
		FROM dbo.Databases d
			INNER JOIN @databases ed
				ON d.ServerId = ed.ServerId
				AND d.DatabaseName = ed.DatabaseName
		WHERE d.Purpose <> ed.Purpose;

	--------------------------
	-- INSERT NEW DATABASES --
	--------------------------

		WITH newDatabases (ServerId, DatabaseName, Purpose, RemovalDate, CreatedDate, CreatedBy, UpdatedDate, UpdatedBy)
		AS
		(
			SELECT dm.ServerId, 
				dm.DatabaseName, 
				dm.Purpose, 
				dm.RemovalDate, 
				dm.CreatedDate, 
				dm.CreatedBy, 
				dm.UpdatedDate, 
				dm.UpdatedBy
			FROM @databases dm
				LEFT JOIN dbo.Databases d
					ON dm.ServerId = d.ServerId
					AND dm.DatabaseName = d.DatabaseName
			WHERE d.ServerId IS null
		)

		INSERT INTO dbo.Databases (ServerId, DatabaseName, Purpose, RemovalDate, CreatedDate, CreatedBy, UpdatedDate, UpdatedBy)
		SELECT ServerId, 
			DatabaseName,
			Purpose,
			null,
			GETUTCDATE(),
			SUSER_SNAME(),
			GETUTCDATE(),
			SUSER_SNAME()
		FROM newDatabases;

	------------------------------
	-- UPDATE REMOVED DATABASES --
	------------------------------

		WITH removedDatabases (DatabaseId, ServerId, DatabaseName, Purpose, RemovalDate, CreatedDate, CreatedBy, UpdatedDate, UpdatedBy)
		AS
		(
			SELECT d.DatabaseId,
				d.ServerId, 
				d.DatabaseName, 
				d.Purpose, 
				d.RemovalDate, 
				d.CreatedDate, 
				d.CreatedBy, 
				d.UpdatedDate, 
				d.UpdatedBy
			FROM dbo.Databases d
				LEFT JOIN @databases dm
					ON dm.ServerId = d.ServerId
					AND dm.DatabaseName = d.DatabaseName
			WHERE dm.ServerId IS NULL
				AND d.ServerId = @serverId
				AND d.RemovalDate IS null
		)

		UPDATE d
		SET d.RemovalDate = GETUTCDATE(),
			d.UpdatedDate = GETUTCDATE(),
			d.UpdatedBy = SUSER_SNAME()
		FROM dbo.Databases d
			INNER JOIN removedDatabases rd
				ON d.DatabaseId = rd.DatabaseId
	end
----------------------------------------------
-- UPDATE SERVER TO REFLECT SCRAPE ACTIVITY --
----------------------------------------------

	UPDATE dbo.Servers
	SET UpdatedDate = GETUTCDATE(),
		UpdatedBy = SUSER_SNAME()
	WHERE ServerId = @serverId

END
