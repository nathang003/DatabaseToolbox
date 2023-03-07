CREATE PROCEDURE [dbo].[spIndexes_Upsert]
(
	@indexes IndexModelType readonly,
	@tableId int
)
AS
BEGIN

	if (select count(1) from @indexes) > 0
	begin
	-----------------------------
	-- UPDATE EXISTING INDEXES --
	-----------------------------

	-- Purpose is addressed separately because we don't want to inadvertantly 
	-- remove it's value or null it out because another field needed to be updated.
	-- Purpose doesn't usually get updated through this SPROC.

		UPDATE i
		SET i.IndexType = iv.IndexType,
			i.IndexTypeDescription = iv.IndexTypeDescription,
			i.IsPrimaryKey = iv.IsPrimaryKey,
			i.IsUniqueConstraint = iv.IsUniqueConstraint,
			i.IsUnique = iv.IsUnique,
			i.IsDisabled = iv.IsDisabled,
			i.UpdatedDate = GETUTCDATE(),
			i.UpdatedBy = SUSER_SNAME()
		FROM dbo.Indexes i
			INNER JOIN @indexes iv
				ON i.ServerId = iv.ServerId
				AND i.DatabaseId = iv.DatabaseId
				AND i.SchemaId = iv.SchemaId
				AND i.TableId = iv.TableId
				AND i.IndexName = iv.IndexName
		WHERE i.IndexType <> iv.IndexType
			OR i.IndexTypeDescription <> iv.IndexTypeDescription
			OR i.IsPrimaryKey <> iv.IsPrimaryKey
			OR i.IsUniqueConstraint <> iv.IsUniqueConstraint
			OR i.IsUnique <> iv.IsUnique
			OR i.IsDisabled <> iv.IsDisabled;

	------------------------
	-- INSERT NEW INDEXES --
	------------------------

		WITH newFields (ServerId, DatabaseId,	SchemaId,			TableId, 
			IndexName,		Purpose,			IndexType,			IndexTypeDescription, 
			IsPrimaryKey,	IsUniqueConstraint,	IsUnique,			IsDisabled, 
			CreatedDate,	CreatedBy,			UpdatedDate,		UpdatedBy)
		AS
		(
			SELECT iv.ServerId,
				iv.DatabaseId,
				iv.SchemaId,
				iv.TableId,
				iv.IndexName,
				iv.Purpose,
				iv.IndexType,
				iv.IndexTypeDescription,
				iv.IsPrimaryKey,
				iv.IsUniqueConstraint,
				iv.IsUnique,
				iv.IsDisabled,
				iv.CreatedDate,
				iv.CreatedBy,
				iv.UpdatedDate,
				iv.UpdatedBy
			FROM @indexes iv
				LEFT JOIN dbo.Indexes i
					ON iv.ServerId = i.ServerId
					AND iv.DatabaseId = i.DatabaseId
					AND iv.SchemaId = i.SchemaId
					AND iv.TableId = i.TableId
					AND iv.IndexName = i.IndexName
			WHERE i.ServerId IS NULL
		)

		INSERT INTO dbo.Indexes (ServerId, DatabaseId,	SchemaId,			TableId, 
			IndexName,		Purpose,			IndexType,			IndexTypeDescription, 
			IsPrimaryKey,	IsUniqueConstraint,	IsUnique,			IsDisabled, 
			CreatedDate,	CreatedBy,			UpdatedDate,		UpdatedBy)
		SELECT ServerId, 
			DatabaseId,
			SchemaId,
			TableId,
			IndexName,
			Purpose,
			IndexType,
			IndexTypeDescription,
			IsPrimaryKey,
			IsUniqueConstraint,
			IsUnique,
			IsDisabled,
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