CREATE TRIGGER [dbo].[MenuFunction_UpdateTrigger]
ON [dbo].[MenuFunction]
FOR UPDATE AS

-- if we've modified the parentId, then we
-- need to do some calculations
IF UPDATE (ParentId) OR UPDATE (Breadcrumb)
BEGIN
	DECLARE @CatId NVARCHAR(50)
	SELECT @CatId = Id FROM inserted
	IF UPDATE (ParentId)
		UPDATE [MenuFunction]
			SET [MenuFunction].Dept = parent.Dept + 1,
			[MenuFunction].Breadcrumb = ISNULL(parent.Breadcrumb,'/') + old.Id + '/'
		FROM [MenuFunction] 
		INNER JOIN inserted old ON [MenuFunction].Id = old.Id
		INNER JOIN [MenuFunction] parent ON old.ParentId=Parent.Id

	UPDATE [MenuFunction]
		SET [MenuFunction].Dept = parent.Dept + 1,
		[MenuFunction].Breadcrumb = ISNULL(parent.Breadcrumb,'/') + [MenuFunction].Id + '/'
	FROM [MenuFunction] 
	INNER JOIN [MenuFunction] parent ON [MenuFunction].ParentId=parent.Id AND parent.Id = @CatId
END