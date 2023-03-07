CREATE PROCEDURE [dbo].[spFields_Upsert]
(
	@fields FieldModelType readonly,
	@tableId int
)
AS
BEGIN

	if (select count(1) from @fields) > 0
	begin
	----------------------------
	-- UPDATE EXISTING FIELDS --
	----------------------------

	-- Purpose is addressed separately because we don't want to inadvertantly 
	-- remove it's value or null it out because another field needed to be updated.
	-- Purpose doesn't usually get updated through this SPROC.

		UPDATE f
		SET f.Purpose = fv.Purpose,
			f.UpdatedDate = GETUTCDATE(),
			f.UpdatedBy = SUSER_SNAME()
		FROM dbo.Fields f
			INNER JOIN @fields fv
				ON f.ServerId = fv.ServerId
				AND f.DatabaseId = fv.DatabaseId
				AND f.SchemaId = fv.SchemaId
				AND f.TableId = fv.TableId
				AND f.FieldName = fv.FieldName
		WHERE fv.Purpose is not null 
			AND ISNULL(f.Purpose, '') <> fv.Purpose;

		UPDATE f
		SET f.OrdinalNumber = fv.OrdinalNumber,
			f.DefaultValue = fv.DefaultValue,
			f.Nullable = fv.IsNullable,
			f.DataType = fv.DataType,
			f.CharacterLength = fv.CharacterLength,
			f.NumericPrecision = fv.NumericPrecision,
			f.NumericScale = fv.NumericScale,
			f.DateTimePrecision = fv.DateTimePrecision,
			f.CharacterSetName = fv.CharacterSetName,
			f.[CollationName] = fv.[CollationName],
			f.PrimaryKey = fv.PrimaryKey,
			f.Indexed = fv.Indexed,
			f.UpdatedDate = GETUTCDATE(),
			f.UpdatedBy = SUSER_SNAME()
		FROM dbo.Fields f
			INNER JOIN @fields fv
				ON f.ServerId = fv.ServerId
				AND f.DatabaseId = fv.DatabaseId
				AND f.SchemaId = fv.SchemaId
				AND f.TableId = fv.TableId
				AND f.FieldName = fv.FieldName
		WHERE f.OrdinalNumber <> fv.OrdinalNumber
			OR f.DefaultValue <> fv.DefaultValue
			OR f.Nullable <> fv.IsNullable
			OR f.DataType <> fv.DataType
			OR f.CharacterLength <> fv.CharacterLength
			OR f.NumericPrecision <> fv.NumericPrecision
			OR f.NumericScale <> fv.NumericScale
			OR f.DateTimePrecision <> fv.DateTimePrecision
			OR f.CharacterSetName <> fv.CharacterSetName
			OR f.[CollationName] <> fv.[CollationName]
			OR f.PrimaryKey <> fv.PrimaryKey
			OR f.Indexed <> fv.Indexed;

	-----------------------
	-- INSERT NEW FIELDS --
	-----------------------

		WITH newFields (ServerId, DatabaseId,	SchemaId,			TableId, 
			FieldName,		Purpose,			OrdinalNumber,		DefaultValue, 
			IsNullable,		DataType,			CharacterLength,	NumericPrecision, 
			NumericScale,	DateTimePrecision,	CharacterSetName,	[CollationName], 
			PrimaryKey,		Indexed,			RemovalDate,		CreatedDate, 
			CreatedBy,		UpdatedDate,		UpdatedBy)
		AS
		(
			SELECT fv.ServerId,
				fv.DatabaseId,
				fv.SchemaId,
				fv.TableId,
				fv.FieldName,
				fv.Purpose,
				fv.OrdinalNumber,
				fv.DefaultValue,
				fv.IsNullable,
				fv.DataType,
				fv.CharacterLength,
				fv.NumericPrecision,
				fv.NumericScale,
				fv.DateTimePrecision,
				fv.CharacterSetName,
				fv.[CollationName],
				fv.PrimaryKey,
				fv.Indexed,
				fv.RemovalDate,
				fv.CreatedDate,
				fv.CreatedBy,
				fv.UpdatedDate,
				fv.UpdatedBy
			FROM @fields fv
				LEFT JOIN dbo.Fields f
					ON fv.ServerId = f.ServerId
					AND fv.DatabaseId = f.DatabaseId
					AND fv.SchemaId = fv.SchemaId
					AND fv.TableId = f.TableId
					AND fv.FieldName = f.FieldName
			WHERE f.ServerId IS NULL
		)

		INSERT INTO dbo.Fields (ServerId, DatabaseId,	SchemaId,			TableId, 
			FieldName,		Purpose,			OrdinalNumber,		DefaultValue, 
			Nullable,		DataType,			CharacterLength,	NumericPrecision, 
			NumericScale,	DateTimePrecision,	CharacterSetName,	[CollationName], 
			PrimaryKey,		Indexed,			RemovalDate,		CreatedDate, 
			CreatedBy,		UpdatedDate,		UpdatedBy)
		SELECT ServerId, 
			DatabaseId,
			SchemaId,
			TableId,
			FieldName,
			Purpose,
			OrdinalNumber,
			DefaultValue,
			IsNullable,
			DataType,
			CharacterLength,
			NumericPrecision,
			NumericScale,
			DateTimePrecision,
			CharacterSetName,
			[CollationName],
			PrimaryKey,
			Indexed,
			null,
			GETUTCDATE(),
			SUSER_SNAME(),
			GETUTCDATE(),
			SUSER_SNAME()
		FROM newFields;

	---------------------------
	-- UPDATE REMOVED FIELDS --
	---------------------------

		WITH removedFields (ServerId, DatabaseId,	SchemaId, TableId, FieldId,
			FieldName,		Purpose,			OrdinalNumber,		DefaultValue, 
			Nullable,		DataType,			CharacterLength,	NumericPrecision, 
			NumericScale,	DateTimePrecision,	CharacterSetName,	[CollationName], 
			PrimaryKey,		Indexed,			RemovalDate,		CreatedDate, 
			CreatedBy,		UpdatedDate,		UpdatedBy)
		AS
		(
			SELECT f.ServerId,
				f.DatabaseId, 
				f.SchemaId,
				f.TableId,
				f.FieldId,
				f.FieldName, 
				f.Purpose, 
				f.OrdinalNumber,
				f.DefaultValue,
				f.Nullable,
				f.DataType,
				f.CharacterLength,
				f.NumericPrecision,
				f.NumericScale,
				f.DateTimePrecision,
				f.CharacterSetName,
				f.[CollationName],
				f.PrimaryKey,
				f.Indexed,
				f.RemovalDate, 
				f.CreatedDate, 
				f.CreatedBy, 
				f.UpdatedDate, 
				f.UpdatedBy
			FROM dbo.Fields f
				LEFT JOIN @fields fv
					ON f.ServerId = fv.ServerId
					AND f.DatabaseId = fv.DatabaseId
					AND f.SchemaId = fv.SchemaId
					AND fv.TableId = f.TableId
					AND fv.FieldName = f.FieldName
			WHERE fv.ServerId IS NULL
				AND f.TableId = @tableId
				AND f.RemovalDate IS null
		)

		UPDATE f
		SET f.RemovalDate = GETUTCDATE(),
			f.UpdatedDate = GETUTCDATE(),
			f.UpdatedBy = SUSER_SNAME()
		FROM dbo.Fields f
			INNER JOIN removedFields rf
				ON f.FieldId = rf.FieldId
	end

------------------------------------------------
-- UPDATE TABLE TO REFLECT SCRAPE ACTIVITY --
------------------------------------------------

	UPDATE dbo.Tables
	SET UpdatedDate = GETUTCDATE(),
		UpdatedBy = SUSER_SNAME()
	WHERE TableId = @tableId

END
