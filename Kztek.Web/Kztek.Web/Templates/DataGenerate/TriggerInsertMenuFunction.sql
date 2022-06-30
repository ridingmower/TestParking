CREATE TRIGGER [dbo].[MenuFunction_InsertTrigger]
ON [dbo].[MenuFunction]
FOR INSERT AS
UPDATE [MenuFunction]
	-- set the Dept of this "child" to be the
	-- Dept of the parent, plus one.
	SET [MenuFunction].Dept = ISNULL(parent.Dept + 1,1),
	-- the BreadCrumb is simply the BreadCrumb of the parent,
	-- plus the child's ID (and appropriate '/' characters
	[MenuFunction].Breadcrumb = ISNULL(parent.Breadcrumb,'/') + LTrim(Str([MenuFunction].Id)) + '/' 
-- we can't update the "inserted" table directly,
-- so we find the corresponding child in the
-- "real" table
FROM [MenuFunction] INNER JOIN inserted i ON i.Id=[MenuFunction].Id
-- now, we attempt to find the parent of this
-- "child" - but it might not exist, so these
-- values may well be NULL
LEFT OUTER JOIN [MenuFunction] parent ON [MenuFunction].ParentId=parent.Id