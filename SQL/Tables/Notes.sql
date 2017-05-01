
GO

ALTER TABLE [dbo].[Notes] DROP CONSTRAINT [DF__Notes__IsAnonymo__282DF8C2]
GO

ALTER TABLE [dbo].[Notes] DROP CONSTRAINT [DF__Notes__UserId__2739D489]
GO

ALTER TABLE [dbo].[Notes] DROP CONSTRAINT [DF__Notes__Subject__0E6E26BF]
GO

ALTER TABLE [dbo].[Notes] DROP CONSTRAINT [DF__Notes__Score__0D7A0286]
GO

ALTER TABLE [dbo].[Notes] DROP CONSTRAINT [DF_DateCreated]
GO

ALTER TABLE [dbo].[Notes] DROP CONSTRAINT [DF__Notes__Uid__0B91BA14]
GO

DROP TABLE [dbo].[Notes]
GO

CREATE TABLE [dbo].[Notes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Uid] [uniqueidentifier] NULL,
	[DateCreated] [datetime2](7) NOT NULL,
	[Latitude] [decimal](9, 6) NOT NULL,
	[Longitude] [decimal](9, 6) NOT NULL,
	[Radius] [int] NOT NULL,
	[Body] [nvarchar](max) NULL,
	[Score] [int] NOT NULL,
	[Subject] [varchar](max) NULL,
	[UserId] [int] NOT NULL,
	[IsAnonymous] [bit] NOT NULL,
 CONSTRAINT [Ct_ID] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO

ALTER TABLE [dbo].[Notes] ADD  DEFAULT (newid()) FOR [Uid]
GO

ALTER TABLE [dbo].[Notes] ADD  CONSTRAINT [DF_DateCreated]  DEFAULT (getutcdate()) FOR [DateCreated]
GO

ALTER TABLE [dbo].[Notes] ADD  DEFAULT ((0)) FOR [Score]
GO

ALTER TABLE [dbo].[Notes] ADD  DEFAULT (NULL) FOR [Subject]
GO

ALTER TABLE [dbo].[Notes] ADD  DEFAULT ((1)) FOR [UserId]
GO

ALTER TABLE [dbo].[Notes] ADD  DEFAULT ((0)) FOR [IsAnonymous]
GO

