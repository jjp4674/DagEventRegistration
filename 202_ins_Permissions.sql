/* This script inserts the permissions into the permissions table */

/* ADMINISTRATION PERMISSIONS */

/* User Administration Group */

MERGE [dbo].[Permissions] AS t
USING (SELECT 'View User Administration' AS [Name]) AS s
	ON (t.[Name] = s.[Name])
WHEN NOT MATCHED THEN
	INSERT (GroupId, [Name], [Description])
	VALUES (2, 'View User Administration', 'Permission to view User Administration page');

MERGE [dbo].[Permissions] AS t
USING (SELECT 'Create New Users' AS [Name]) AS s
	ON (t.[Name] = s.[Name])
WHEN NOT MATCHED THEN
	INSERT (GroupId, [Name], [Description])
	VALUES (2, 'Create New Users', 'Permission to create new users');

MERGE [dbo].[Permissions] AS t
USING (SELECT 'Edit Existing Users' AS [Name]) AS s
	ON (t.[Name] = s.[Name])
WHEN NOT MATCHED THEN
	INSERT (GroupId, [Name], [Description])
	VALUES (2, 'Edit Existing Users', 'Permission to edit existing users');

MERGE [dbo].[Permissions] AS t
USING (SELECT 'Delete Users' AS [Name]) AS s
	ON (t.[Name] = s.[Name])
WHEN NOT MATCHED THEN
	INSERT (GroupId, [Name], [Description])
	VALUES (2, 'Delete Users', 'Permission to delete users');

/* Role Administration Group */

MERGE [dbo].[Permissions] AS t 
USING (SELECT 'View Role Administration' AS [Name]) AS s
	ON (t.[Name] = s.[Name])
WHEN NOT MATCHED THEN 
	INSERT (GroupId, [Name], [Description])
	VALUES (3, 'View Role Administration', 'Permission to view Role Administration page');

MERGE [dbo].[Permissions] AS t
USING (SELECT 'Create New Roles' AS [Name]) AS s
	ON (t.[Name] = s.[Name])
WHEN NOT MATCHED THEN
	INSERT (GroupId, [Name], [Description])
	VALUES (3, 'Create New Roles', 'Permission to create new roles');

MERGE [dbo].[Permissions] AS t
USING (SELECT 'Edit Existing Roles' AS [Name]) AS s
	ON (t.[Name] = s.[Name])
WHEN NOT MATCHED THEN
	INSERT (GroupId, [Name], [Description])
	VALUES (3, 'Edit Existing Roles', 'Permission to edit existing roles');

MERGE [dbo].[Permissions] AS t
USING (SELECT 'Delete Roles' AS [Name]) AS s
	ON (t.[Name] = s.[Name])
WHEN NOT MATCHED THEN
	INSERT (GroupId, [Name], [Description])
	VALUES (2, 'Delete Roles', 'Permission to delete roles');

/* Event Management Group */

MERGE [dbo].[Permissions] AS t 
USING (SELECT 'Manage User Events' AS [Name]) AS s
	ON (t.[Name] = s.[Name])
WHEN NOT MATCHED THEN 
	INSERT (GroupId, [Name], [Description])
	VALUES (4, 'Manage User Events', 'Permission to manage user''s events');

MERGE [dbo].[Permissions] AS t 
USING (SELECT 'Manage All Events' AS [Name]) AS s
	ON (t.[Name] = s.[Name])
WHEN NOT MATCHED THEN 
	INSERT (GroupId, [Name], [Description])
	VALUES (4, 'Manage All Events', 'Permission to manage all events');