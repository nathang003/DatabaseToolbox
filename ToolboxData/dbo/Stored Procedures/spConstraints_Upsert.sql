CREATE PROCEDURE [dbo].[spConstraints_Upsert]
	@constraints ConstraintModelType readonly,
	@tableId int
AS
BEGIN

	if (select count(1) from @constraints) > 0
	BEGIN
	---------------------------------
	-- UPDATE EXISTING CONSTRAINTS --
	---------------------------------

	-- Purpose is addressed separately because we don't want to inadvertantly 
	-- remove it's value or null it out because another field needed to be updated.
	-- Purpose doesn't usually get updated through this SPROC.

		UPDATE c
		SET c.ConstraintType = cv.ConstraintType,
			c.ConstraintTypeDescription = cv.ConstraintTypeDescription,
			c.IsSystemNamed = cv.IsSystemNamed,
			c.UpdatedDate = GETUTCDATE(),
			c.UpdatedBy = SUSER_SNAME()
		FROM dbo.Constraints c
			INNER JOIN @constraints cv
				ON c.ServerId = cv.ServerId
				AND c.DatabaseId = cv.DatabaseId
				AND c.SchemaId = cv.SchemaId
				AND c.TableId = cv.TableId
				AND c.ConstraintName = cv.ConstraintName
		WHERE c.ConstraintType <> cv.ConstraintType
			OR c.ConstraintTypeDescription <> cv.ConstraintTypeDescription
			OR c.IsSystemNamed <> cv.IsSystemNamed;

	----------------------------
	-- INSERT NEW CONSTRAINTS --
	----------------------------

		WITH newFields (ServerId,		DatabaseId,				SchemaId,		TableId, 
			ConstraintName,				ConstraintDefinition,	Purpose,		ConstraintType,
			ConstraintTypeDescription, 	IsSystemNamed,			CreatedDate,	CreatedBy,
			UpdatedDate,				UpdatedBy)
		AS
		(
			SELECT cv.ServerId,
				cv.DatabaseId,
				cv.SchemaId,
				cv.TableId,
				cv.ConstraintName,
				cv.ConstraintDefinition,
				cv.Purpose,
				cv.ConstraintType,
				cv.ConstraintTypeDescription,
				cv.IsSystemNamed,
				cv.CreatedDate,
				cv.CreatedBy,
				cv.UpdatedDate,
				cv.UpdatedBy
			FROM @constraints cv
				LEFT JOIN dbo.Constraints c
					ON cv.ServerId = c.ServerId
					AND cv.DatabaseId = c.DatabaseId
					AND cv.SchemaId = c.SchemaId
					AND cv.TableId = c.TableId
					AND cv.ConstraintName = c.ConstraintName
			WHERE c.ServerId IS NULL
		)

		INSERT INTO dbo.Constraints (ServerId, DatabaseId,		SchemaId,		TableId, 
			ConstraintName,				ConstraintDefinition,	Purpose,		ConstraintType,
			ConstraintTypeDescription, 	IsSystemNamed,			CreatedDate,	CreatedBy,
			UpdatedDate,				UpdatedBy)
		SELECT ServerId, 
			DatabaseId,
			SchemaId,
			TableId,
			ConstraintName,
			ConstraintDefinition,
			Purpose,
			ConstraintType,
			ConstraintTypeDescription,
			IsSystemNamed,
			GETUTCDATE(),
			SUSER_SNAME(),
			GETUTCDATE(),
			SUSER_SNAME()
		FROM newFields;

------------------------------------------------
-- UPDATE TABLE TO REFLECT SCRAPE ACTIVITY --
------------------------------------------------

	UPDATE dbo.Tables
	SET UpdatedDate = GETUTCDATE(),
		UpdatedBy = SUSER_SNAME()
	WHERE TableId = @tableId
		
	END
END
