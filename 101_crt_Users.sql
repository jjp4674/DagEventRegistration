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

/****** Object:  Table [dbo].[Users]    Script Date: 11/29/2017 11:55:16 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Users](
	[UserId] [bigint] IDENTITY(1,1) NOT NULL,
	[Username] [nvarchar](255) NOT NULL, 
	[PasswordHash] [varchar](MAX) NULL,
	[PasswordToken] [varchar](MAX) NULL, 
	[Email] [nvarchar](255) NOT NULL,
	[EmailToken] [varchar](MAX) NULL, 
	[EmailConfirmed] [bit] NOT NULL,
	[FirstName] [nvarchar](50) NOT NULL,
	[LastName] [nvarchar](150) NOT NULL,
	[Locked] [bit] NOT NULL,
	[LockedDate] [datetime] NULL,
	[FailedLogins] [int] NOT NULL,
	[CreatedBy] [bigint] NOT NULL, 
	[CreatedDate] [datetime] NOT NULL,
	[UpdatedBy] [bigint] NULL, 
	[UpdatedDate] [datetime] NULL, 
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO