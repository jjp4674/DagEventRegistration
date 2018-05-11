/*    ==Scripting Parameters==

    Source Server Version : SQL Server 2016 (13.0.4001)
    Source Database Engine Edition : Microsoft SQL Server Express Edition
    Source Database Engine Type : Standalone SQL Server

    Target Server Version : SQL Server 2016
    Target Database Engine Edition : Microsoft SQL Server Express Edition
    Target Database Engine Type : Standalone SQL Server
*/

USE [EventRegistration]
GO

/****** Object:  Table [dbo].[UserRoles]    Script Date: 11/29/2017 11:55:16 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[UserRoles](
	[UserRoleId] [bigint] IDENTITY(1,1) NOT NULL,
	[UserId] [bigint] NOT NULL, 
	[RoleId] [bigint] NOT NULL,
	[CreatedBy] [bigint] NOT NULL, 
	[CreatedDate] [datetime] NOT NULL,
	CONSTRAINT [PK_UserRoles] PRIMARY KEY CLUSTERED 
	(
		[UserRoleId] ASC
	)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY], 
	CONSTRAINT [FK_UserRoles_User] FOREIGN KEY  
	(
		[UserId]
	) REFERENCES [Users]([UserId]), 
	CONSTRAINT [FK_UserRoles_Role] FOREIGN KEY  
	(
		[RoleId]
	) REFERENCES [Roles]([RoleId])
) ON [PRIMARY]
GO