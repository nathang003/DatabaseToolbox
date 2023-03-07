CREATE PROCEDURE [dbo].[spForeignKeys_Upsert]
(
	@foreignKeys ForeignKeyModelType readonly,
	@tableId int
)
AS
BEGIN
	if (select count(1) from @foreignKeys) > 0
	begin
	----------------------------------
	-- UPDATE EXISTING FOREIGN KEYS --
	----------------------------------

		UPDATE fk
		SET fk.IsDisabled = fkv.IsDisabled,
			fk.IsNotTrusted = fkv.IsNotTrusted,
			fk.DeleteReferentialActionDescription = fkv.DeleteReferentialActionDescription,
			fk.UpdateReferentialActionDescription = fkv.UpdateReferentialActionDescription,
			fk.UpdatedDate = GETUTCDATE(),
			fk.UpdatedBy = SUSER_SNAME()
		FROM dbo.ForeignKeys fk
			INNER JOIN @foreignKeys fkv
				ON fk.ServerId = fkv.ServerId
				AND fk.DatabaseId = fkv.DatabaseId
				AND fk.SchemaId = fkv.SchemaId
				AND fk.TableId = fkv.TableId
				AND fk.ForeignKeyName = fkv.ForeignKeyName
		WHERE fk.IsDisabled <> fkv.IsDisabled
			OR fk.IsNotTrusted <> fkv.IsNotTrusted
			OR fk.DeleteReferentialActionDescription <> fkv.DeleteReferentialActionDescription
			OR fk.UpdateReferentialActionDescription <> fkv.UpdateReferentialActionDescription;

	-----------------------------
	-- INSERT NEW FOREIGN KEYS --
	-----------------------------

		WITH newForeignKeys (ServerId, DatabaseId, SchemaId, TableId, ForeignKeyName, Purpose, ConstraintSchemaId, ConstraintTableId, ConstraintFieldId, ReferencedSchemaId, ReferencedTableId, ReferencedFieldId, IsDisabled, IsNotTrusted, DeleteReferentialActionDescription, UpdateReferentialActionDescription)
		AS
		(
			SELECT fkv.ServerId,
				fkv.DatabaseId,
				fkv.SchemaId,
				fkv.TableId,
				fkv.ForeignKeyName,
				fkv.Purpose,
				fkv.ConstraintSchemaId,
				fkv.ConstraintTableId,
				fkv.ConstraintFieldId,
				fkv.ReferencedSchemaId,
				fkv.ReferencedTableId,
				fkv.ReferencedFieldId,
				fkv.IsDisabled,
				fkv.IsNotTrusted,
				fkv.DeleteReferentialActionDescription,
				fkv.UpdateReferentialActionDescription
			FROM @foreignKeys fkv
				LEFT JOIN dbo.ForeignKeys fk
					ON fkv.ServerId = fk.ServerId
					AND fkv.DatabaseId = fk.DatabaseId
					AND fkv.SchemaId = fk.SchemaId
					AND fkv.TableId = fk.TableId
					AND fkv.ForeignKeyName = fk.ForeignKeyName
			WHERE fk.ServerId IS NULL
		)

		INSERT INTO dbo.ForeignKeys (ServerId, DatabaseId, SchemaId, TableId, ForeignKeyName, Purpose, ConstraintSchemaId, ConstraintTableId, ConstraintFieldId, ReferencedSchemaId, ReferencedTableId, ReferencedFieldId, IsDisabled, IsNotTrusted, DeleteReferentialActionDescription, UpdateReferentialActionDescription, CreatedDate, CreatedBy, UpdatedDate, UpdatedBy)
		SELECT ServerId, 
			DatabaseId,
			SchemaId,
			TableId,
			ForeignKeyName,
			Purpose,
			ConstraintSchemaId,
			ConstraintTableId,
			ConstraintFieldId,
			ReferencedSchemaId,
			ReferencedTableId,
			ReferencedFieldId,
			IsDisabled,
			IsNotTrusted,
			DeleteReferentialActionDescription,
			UpdateReferentialActionDescription,
			GETUTCDATE(),
			SUSER_SNAME(),
			GETUTCDATE(),
			SUSER_SNAME()
		FROM newForeignKeys;

	------------------------------------------------
	-- UPDATE DATABASE TO REFLECT SCRAPE ACTIVITY --
	------------------------------------------------

		UPDATE dbo.Tables
		SET UpdatedDate = GETUTCDATE(),
			UpdatedBy = SUSER_SNAME()
		WHERE TableId = @tableId;

	END
END
