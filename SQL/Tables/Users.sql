
GO

ALTER TABLE [dbo].[Users] DROP CONSTRAINT [DF__Users__Uid__756D6ECB]
GO

DROP TABLE [dbo].[Users]
GO

CREATE TABLE [dbo].[Users](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Uid] [uniqueidentifier] NOT NULL,
	[Username] [varchar](50) NOT NULL,
	[PasswordHash] [varchar](300) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO

ALTER TABLE [dbo].[Users] ADD  DEFAULT (newid()) FOR [Uid]
GO

