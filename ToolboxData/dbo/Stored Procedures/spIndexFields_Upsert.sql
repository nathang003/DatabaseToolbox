CREATE PROCEDURE [dbo].[spIndexFields_Upsert]
(
	@indexFields IndexFieldModelType readonly,
	@indexId int
)
AS
BEGIN

	if (select count(1) from @indexFields) > 0
	begin
	-----------------------------
	-- UPDATE EXISTING INDEXES --
	-----------------------------

		UPDATE i
		SET i.KeyOrdinal = iv.KeyOrdinal,
			i.PartialOrdinal = iv.PartialOrdinal,
			i.IsDescendingKey = iv.IsDescendingKey,
			i.IsIncludedColumn = iv.IsIncludedColumn,
			i.UpdatedDate = GETUTCDATE(),
			i.UpdatedBy = SUSER_SNAME()
		FROM dbo.IndexFields i
			INNER JOIN @indexFields iv
				ON i.ServerId = iv.ServerId
				AND i.DatabaseId = iv.DatabaseId
				AND i.SchemaId = iv.SchemaId
				AND i.TableId = iv.TableId
				AND i.IndexId = iv.IndexId
				AND i.FieldId = iv.FieldId
		WHERE i.KeyOrdinal <> iv.KeyOrdinal
			OR i.PartialOrdinal <> iv.PartialOrdinal
			OR i.IsDescendingKey <> iv.IsDescendingKey
			OR i.IsIncludedColumn <> iv.IsIncludedColumn;

	------------------------
	-- INSERT NEW INDEXES --
	------------------------

		WITH newFields (ServerId, DatabaseId,	SchemaId,			TableId, 
			IndexId,			FieldId,			KeyOrdinal,			PartialOrdinal, 
			IsDescendingKey,	IsIncludedColumn,
			CreatedDate,	CreatedBy,			UpdatedDate,		UpdatedBy)
		AS
		(
			SELECT iv.ServerId,
				iv.DatabaseId,
				iv.SchemaId,
				iv.TableId,
				iv.IndexId,
				iv.FieldId,
				iv.KeyOrdinal,
				iv.PartialOrdinal,
				iv.IsDescendingKey,
				iv.IsIncludedColumn,
				iv.CreatedDate,
				iv.CreatedBy,
				iv.UpdatedDate,
				iv.UpdatedBy
			FROM @indexFields iv
				LEFT JOIN dbo.IndexFields i
					ON i.ServerId = iv.ServerId
					AND i.DatabaseId = iv.DatabaseId
					AND i.SchemaId = iv.SchemaId
					AND i.TableId = iv.TableId
					AND i.IndexId = iv.IndexId
					AND i.FieldId = iv.FieldId
			WHERE i.ServerId IS NULL
		)

		INSERT INTO dbo.IndexFields (ServerId, DatabaseId,	SchemaId,	TableId, 
			IndexId,			FieldId,			KeyOrdinal,			PartialOrdinal, 
			IsDescendingKey,	IsIncludedColumn,	CreatedDate,		CreatedBy,			
			UpdatedDate,		UpdatedBy)
		SELECT ServerId,
			DatabaseId,
			SchemaId,
			TableId,
			IndexId,
			FieldId,
			KeyOrdinal,
			PartialOrdinal,
			IsDescendingKey,
			IsIncludedColumn,
			GETUTCDATE(),
			SUSER_SNAME(),
			GETUTCDATE(),
			SUSER_SNAME()
		FROM newFields;

------------------------------------------------
-- UPDATE TABLE TO REFLECT SCRAPE ACTIVITY --
------------------------------------------------

	UPDATE dbo.Indexes
	SET UpdatedDate = GETUTCDATE(),
		UpdatedBy = SUSER_SNAME()
	WHERE IndexId = @indexId
	END
END
