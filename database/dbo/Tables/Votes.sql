CREATE TABLE [dbo].[Votes] (
    [Id]          INT              IDENTITY (1, 1) NOT NULL,
    [Uid]         UNIQUEIDENTIFIER DEFAULT (newid()) NOT NULL,
    [NoteId]      INT              NOT NULL,
    [UserId]      INT              NOT NULL,
    [DateCreated] DATETIME2 (7)    DEFAULT (getutcdate()) NOT NULL,
    [Value]       INT              NOT NULL,
    CONSTRAINT [PK_Votes] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [UQ_vote] UNIQUE NONCLUSTERED ([NoteId] ASC, [UserId] ASC)
);

