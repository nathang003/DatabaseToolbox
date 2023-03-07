CREATE PROCEDURE [dbo].[spConstraintFields_Upsert]
	@constraintFields ConstraintFieldModelType readonly,
	@constraintId int
AS
BEGIN

	if (select count(1) from @constraintFields) > 0
	BEGIN
	
	---------------------------------
	-- UPDATE EXISTING CONSTRAINTS --
	---------------------------------

		UPDATE c
		SET c.IsNullable = cv.IsNullable,
			c.IsAnsiPadded = cv.IsAnsiPadded,
			c.IsRowGuidColumn = cv.IsRowGuidColumn,
			c.IsIdentity = cv.IsIdentity,
			c.GeneratedAlwaysType = cv.GeneratedAlwaysType,
			c.GeneratedAlwaysTypeDescription = cv.GeneratedAlwaysTypeDescription,
			c.UpdatedDate = GETUTCDATE(),
			c.UpdatedBy = SUSER_SNAME()
		FROM dbo.ConstraintFields c
			INNER JOIN @constraintFields cv
				ON c.ServerId = cv.ServerId
				AND c.DatabaseId = cv.DatabaseId
				AND c.SchemaId = cv.SchemaId
				AND c.TableId = cv.TableId
				AND c.ConstraintId = cv.ConstraintId
				AND c.FieldId = cv.FieldId
		WHERE c.IsNullable <> cv.IsNullable
			OR c.IsAnsiPadded <> cv.IsAnsiPadded
			OR c.IsRowGuidColumn <> cv.IsRowGuidColumn
			OR c.IsIdentity <> cv.IsIdentity
			OR c.GeneratedAlwaysType <> cv.GeneratedAlwaysType
			OR c.GeneratedAlwaysTypeDescription <> cv.GeneratedAlwaysTypeDescription;
	
	----------------------------
	-- INSERT NEW CONSTRAINTS --
	----------------------------

		WITH newFields (ServerId, DatabaseId, SchemaId, TableId, 
			ConstraintId, FieldId, IsNullable, IsAnsiPadded,
			IsRowGuidColumn, IsIdentity, GeneratedAlwaysType, GeneratedAlwaysTypeDescription,
			CreatedDate, CreatedBy, UpdatedDate, UpdatedBy)
		AS
		(
			SELECT cv.ServerId,
				cv.DatabaseId,
				cv.SchemaId,
				cv.TableId,
				cv.ConstraintId,
				cv.FieldId,
				cv.IsNullable,
				cv.IsAnsiPadded,
				cv.IsRowGuidColumn,
				cv.IsIdentity,
				cv.GeneratedAlwaysType,
				cv.GeneratedAlwaysTypeDescription,
				cv.CreatedDate,
				cv.CreatedBy,
				cv.UpdatedDate,
				cv.UpdatedBy
			FROM @constraintFields cv
				LEFT JOIN dbo.ConstraintFields c
					ON c.ServerId = cv.ServerId
					AND c.DatabaseId = cv.DatabaseId
					AND c.SchemaId = cv.SchemaId
					AND c.TableId = cv.TableId
					AND c.ConstraintId = cv.ConstraintId
					AND c.FieldId = cv.FieldId
			WHERE c.ServerId IS NULL
		)

		INSERT INTO dbo.ConstraintFields (ServerId, DatabaseId, SchemaId, TableId,
			ConstraintId, FieldId, IsNullable, IsAnsiPadded,
			IsRowGuidColumn, IsIdentity, GeneratedAlwaysType, GeneratedAlwaysTypeDescription,
			CreatedDate, CreatedBy, UpdatedDate, UpdatedBy)
		SELECT ServerId,
			DatabaseId,
			SchemaId,
			TableId,
			ConstraintId,
			FieldId,
			IsNullable,
			IsAnsiPadded,
			IsRowGuidColumn,
			IsIdentity,
			GeneratedAlwaysType,
			GeneratedAlwaysTypeDescription,
			GETUTCDATE(),
			SUSER_SNAME(),
			GETUTCDATE(),
			SUSER_SNAME()
		FROM newFields;

------------------------------------------------
-- UPDATE TABLE TO REFLECT SCRAPE ACTIVITY --
------------------------------------------------

	UPDATE dbo.Constraints
	SET UpdatedDate = GETUTCDATE(),
		UpdatedBy = SUSER_SNAME()
	WHERE ConstraintId = @constraintId
	END
END
