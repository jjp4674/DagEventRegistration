/* This script inserts the permission groups into the permission groups table */

MERGE [dbo].[PermissionGroups] AS t
USING (SELECT 'Administration' AS [Name]) AS s
	ON (t.[Name] = s.[Name])
WHEN NOT MATCHED THEN
	INSERT ([Name], [ParentId], [Description])
	VALUES ('Administration', NULL, 'Administration Permissions Group');

MERGE [dbo].[PermissionGroups] as t
USING (SELECT 'User Administration' AS [Name]) AS s 
	ON (t.[Name] = s.[Name])
WHEN NOT MATCHED THEN 
	INSERT ([Name], [ParentId], [Description])
	VALUES ('User Administration', 1, 'DVR User Administration Group');

MERGE [dbo].[PermissionGroups] as t 
USING (SELECT 'Role Administration' AS [Name]) AS s
	ON (t.[Name] = s.[Name])
WHEN NOT MATCHED THEN
	INSERT ([Name], [ParentId], [Description])
	VALUES ('Role Administration', 1, 'DVR Role Administration Group');

MERGE [dbo].[PermissionGroups] as t 
USING (SELECT 'Event Management' AS [Name]) AS s
	ON (t.[Name] = s.[Name])
WHEN NOT MATCHED THEN
	INSERT ([Name], [ParentId], [Description])
	VALUES ('Event Management', NULL, 'Event Management Group');