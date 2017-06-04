USE [sackfacedb]
GO

CREATE TABLE [dbo].[ReportedNotes] (
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Uid] [uniqueidentifier] NOT NULL,
	[NoteId] [int] NOT NULL,
	[UserId] [int] NOT NULL,
	[DateCreated] [datetime2](7) NOT NULL
 CONSTRAINT [PK_ReportedNotes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON),
 CONSTRAINT [UQ_ReportedNote] UNIQUE NONCLUSTERED 
(
	[NoteId] ASC,
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO

ALTER TABLE [dbo].[ReportedNotes] ADD  DEFAULT (newid()) FOR [Uid]
GO

ALTER TABLE [dbo].[ReportedNotes] ADD  DEFAULT (getutcdate()) FOR [DateCreated]
GO

ALTER TABLE [dbo].[ReportedNotes]  WITH CHECK ADD  CONSTRAINT [FK_ReportedNotes_Notes] FOREIGN KEY([NoteId])
REFERENCES [dbo].[Notes] ([Id])
GO

ALTER TABLE [dbo].[ReportedNotes] CHECK CONSTRAINT [FK_ReportedNotes_Notes]
GO

ALTER TABLE [dbo].[ReportedNotes]  WITH CHECK ADD  CONSTRAINT [FK_ReportedNotes_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO

ALTER TABLE [dbo].[ReportedNotes] CHECK CONSTRAINT [FK_ReportedNotes_Users]
GO


