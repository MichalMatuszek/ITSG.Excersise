CREATE TABLE [dbo].[Users](
	[Id] [int] PRIMARY KEY IDENTITY(1,1) NOT NULL,
	[Login] [varchar](255) NOT NULL,
	[PasswordHash] [varchar](1024) NOT NULL,
)

CREATE TABLE [dbo].[UserRoles](
	[UserId] [int] NOT NULL,
	[Name] [varchar](255) NOT NULL,
 CONSTRAINT [PK_UserRoles] PRIMARY KEY CLUSTERED ([UserId], [Name] )
)

ALTER TABLE [dbo].[UserRoles]  WITH CHECK ADD FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])

CREATE TABLE [dbo].[Resources](
	[Id] [int] PRIMARY KEY IDENTITY(1,1) NOT NULL,
	[Name] [varchar](255) NOT NULL,
	[LockedBy] [int] NULL,
	[LockedTo] [datetime2](7) NULL,
	[IsDeleted] [bit] NOT NULL,
	[Version] [timestamp] NOT NULL
)

ALTER TABLE [dbo].[Resources] ADD  DEFAULT ((0)) FOR [IsDeleted]

ALTER TABLE [dbo].[Resources]  WITH CHECK ADD FOREIGN KEY([LockedBy])
REFERENCES [dbo].[Users] ([Id])