﻿CREATE TABLE [dbo].[Votes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Uid] [uniqueidentifier] NOT NULL,
	[NoteId] [int] NOT NULL,
	[UserId] [int] NOT NULL,
	[DateCreated] [datetime2](7) NOT NULL,
	[Value] [int] NOT NULL,
 CONSTRAINT [PK_Votes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON),
 CONSTRAINT [UQ_vote] UNIQUE NONCLUSTERED 
(
	[NoteId] ASC,
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO

ALTER TABLE [dbo].[Votes] ADD  DEFAULT (newid()) FOR [Uid]
GO

ALTER TABLE [dbo].[Votes] ADD  DEFAULT (getutcdate()) FOR [DateCreated]
GO

ALTER TABLE [dbo].[Votes]  WITH CHECK ADD  CONSTRAINT [FK_Votes_Users] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([Id])
GO

ALTER TABLE [dbo].[Votes] CHECK CONSTRAINT [FK_Votes_Users]
GO

ALTER TABLE [dbo].[Votes]  WITH CHECK ADD  CONSTRAINT [FK_Votes_Notes] FOREIGN KEY([NoteId])
REFERENCES [dbo].[Notes] ([Id])
GO

ALTER TABLE [dbo].[Votes] CHECK CONSTRAINT [FK_Votes_Notes]
GO