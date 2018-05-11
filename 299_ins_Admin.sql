DECLARE @UserId BIGINT = 0, @RoleId BIGINT = 0

/* Create Admin user */
MERGE [dbo].[Users] AS t
USING (SELECT 'admin@castel.com' AS Email) AS s
	ON (t.Email = s.Email)
WHEN NOT MATCHED THEN
	INSERT (Username, PasswordHash, Email, EmailConfirmed, FirstName, LastName, Locked, FailedLogins, CreatedBy, CreatedDate)
	VALUES ('Admin', '$2a$12$9GDZBpjXZSFRbwnwr/3aT.lGQWourpVbYqQDcnBSyqF5L6tqKMHui', 'jjp4674@gmail.com', 1, 'Admin', 'User', 0, 0, 1, GETDATE())
OUTPUT Inserted.UserId;

SET @UserId = SCOPE_IDENTITY();

/* Create Administrator Role if it doesn't exist yet */
MERGE [dbo].[Roles] AS t
USING (SELECT 'Administrator' AS [Name]) AS s 
	ON (t.[Name] = s.[Name])
WHEN NOT MATCHED THEN
	INSERT ([Name], [Description], CreatedBy, CreatedDate)
	VALUES ('Administrator', 'Administrator Role with Full Access', 1, GETDATE())
OUTPUT Inserted.RoleId;

SET @RoleId = SCOPE_IDENTITY();

/* Add all Permissions to role */
MERGE [dbo].[RolePermissions] AS t
USING (SELECT PermissionId FROM [Permissions]) AS s 
	ON (t.PermissionId = s.PermissionId)
WHEN NOT MATCHED THEN
	INSERT (RoleId, PermissionId, CreatedBy, CreatedDate)
	VALUES (@RoleId, s.PermissionId, 1, GETDATE());

/* Add Role to User Roles */
MERGE [dbo].[UserRoles] AS t
USING (SELECT @UserId, @RoleId) AS s (UserId, RoleId)
	ON (t.UserId = @UserId AND t.RoleId = @RoleId)
WHEN NOT MATCHED THEN
	INSERT (UserId, RoleId, CreatedBy, CreatedDate)
	VALUES (@UserId, @RoleId, 1, GETDATE());