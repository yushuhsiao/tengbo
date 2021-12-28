CREATE TABLE [dbo].[Permission3] (
    [UserID] INT NOT NULL,
    [CodeID] INT NOT NULL,
    [Flag]   INT NOT NULL,
    CONSTRAINT [PK_Permission3] PRIMARY KEY CLUSTERED ([UserID] ASC, [CodeID] ASC)
);

