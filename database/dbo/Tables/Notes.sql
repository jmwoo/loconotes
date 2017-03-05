CREATE TABLE [dbo].[Notes] (
    [Id]          INT              IDENTITY (1, 1) NOT NULL,
    [Uid]         UNIQUEIDENTIFIER DEFAULT (newid()) NULL,
    [DateCreated] DATETIME2 (7)    CONSTRAINT [DF_DateCreated] DEFAULT (getutcdate()) NOT NULL,
    [Latitude]    DECIMAL (9, 6)   NOT NULL,
    [Longitude]   DECIMAL (9, 6)   NOT NULL,
    [Radius]      INT              NOT NULL,
    [Body]        NVARCHAR (MAX)   NULL,
    [Score]       INT              DEFAULT ((0)) NOT NULL,
    [Subject]     VARCHAR (MAX)    DEFAULT (NULL) NULL,
    [UserId]      INT              DEFAULT ((1)) NOT NULL,
    [IsAnonymous] BIT              DEFAULT ((0)) NOT NULL,
    CONSTRAINT [Ct_ID] PRIMARY KEY CLUSTERED ([Id] ASC)
);

