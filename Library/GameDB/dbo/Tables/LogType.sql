CREATE TABLE [dbo].[LogType] (
    [LogType] INT             NOT NULL,
    [Gain]    DECIMAL (19, 6) NOT NULL,
    CONSTRAINT [PK_LogType] PRIMARY KEY CLUSTERED ([LogType] ASC)
);

