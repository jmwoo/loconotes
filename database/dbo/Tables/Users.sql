CREATE TABLE [dbo].[Users]
(
	[Id] INT PRIMARY KEY IDENTITY(1,1) NOT NULL, 
    [Uid] UNIQUEIDENTIFIER DEFAULT(NEWID()) NOT NULL,
	[Username] VARCHAR(50) NOT NULL,
    [PasswordHash] VARCHAR(300) NOT NULL,
)

GO

CREATE UNIQUE INDEX [IX_Username_Unique] ON [dbo].[Users] ([Username])
