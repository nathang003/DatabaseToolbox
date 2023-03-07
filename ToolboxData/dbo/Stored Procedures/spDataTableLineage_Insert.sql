CREATE PROCEDURE [dbo].[spDataTableLineage_Insert]
	@parentTableId int = -1,
	@childTableId int = -1,
	@lineageStartDate date = '2017-01-01',
	@lineageEndDate date = null,
	@userId nvarchar(50) = 'ToolboxApp'

AS
BEGIN

	-- Check for existing data lineage records that match the parent and child relationship.
	-- If we find an existing record we need to abort the SPROC to avoid duplication.
	IF 
		(
			SELECT count(DataTableLineageId)
			FROM dbo.DataTableLineage
			WHERE ParentTableId = @parentTableId
				AND ChildTableId = @childTableId
		) > 0
	BEGIN

		RETURN 1

	END

	-- Check to see if the user id is valid to check against SQL INJECTION.
	ELSE IF 
		(
			(
				@userId = 'ToolboxApp' 
				OR 
				(
					SELECT COUNT(UserId)
					FROM dbo.Users
					WHERE @userId = UserId
				) > 0
			)
			AND @parentTableId > -1
			AND @childTableId > -1
		)
	BEGIN

		INSERT INTO dbo.DataTableLineage 
			(
				ParentTableId, 
				ChildTableId, 
				LineageStartDate, 
				LineageEndDate, 
				CreatedDate, 
				CreatedBy, 
				UpdatedDate, 
				UpdatedBy
			) VALUES
			(
				@parentTableId,
				@childTableId,
				@lineageStartDate,
				@lineageEndDate,
				GETUTCDATE(),
				@userId,
				GETUTCDATE(),
				@userId
			)

		RETURN 0

	END

	-- Return an error if the user id was invalid.
	ELSE

		RETURN -1
END
