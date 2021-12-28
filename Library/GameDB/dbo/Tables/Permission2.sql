CREATE TABLE [dbo].[Permission2] (
    [UserType] TINYINT NOT NULL,
    [CorpID]   INT     NOT NULL,
    [GroupID]  TINYINT NOT NULL,
    [CodeID]   INT     NOT NULL,
    [Flag]     INT     NOT NULL,
    CONSTRAINT [PK_Permission2] PRIMARY KEY CLUSTERED ([UserType] ASC, [CorpID] ASC, [GroupID] ASC, [CodeID] ASC)
);

