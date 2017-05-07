USE [sackfacedb]

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
	[IsDeleted] [bit] NOT NULL,
 CONSTRAINT [Ct_ID] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

go
CREATE NONCLUSTERED INDEX [IX_LatLon] ON [dbo].[Notes]
(
    [Latitude] ASC,
    [Longitude] ASC,
	[IsDeleted] Asc
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = ON, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
GO

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

ALTER TABLE [dbo].[Notes] ADD  DEFAULT ((0)) FOR [IsDeleted]
GO

ALTER TABLE [dbo].[Notes]  WITH CHECK ADD  CONSTRAINT [FK_Notes_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO

ALTER TABLE [dbo].[Notes] CHECK CONSTRAINT [FK_Notes_Users]
GO

