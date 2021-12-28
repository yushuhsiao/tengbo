CREATE TABLE [dbo].[GameType] (
    [GameID]   INT           NOT NULL,
    [TypeCode] INT           NOT NULL,
    [GameType] VARCHAR (20)  NOT NULL,
    [Name]     NVARCHAR (50) NOT NULL,
    CONSTRAINT [PK_GameType] PRIMARY KEY CLUSTERED ([GameID] ASC, [TypeCode] ASC),
    CONSTRAINT [IX_GameType] UNIQUE NONCLUSTERED ([GameID] ASC, [GameType] ASC)
);

