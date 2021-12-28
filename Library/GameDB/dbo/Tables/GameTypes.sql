CREATE TABLE [dbo].[GameTypes] (
    [GameID] INT            NOT NULL,
    [src]    VARCHAR (100)  NOT NULL,
    [dst]    NVARCHAR (100) NOT NULL,
    CONSTRAINT [PK_GameTypes] PRIMARY KEY CLUSTERED ([GameID] ASC, [src] ASC)
);

