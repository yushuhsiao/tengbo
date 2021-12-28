CREATE TABLE [dbo].[CorpB] (
    [CorpID]     INT            NOT NULL,
    [GameID]     INT            NOT NULL,
    [MemberType] INT            NOT NULL,
    [BonusW]     DECIMAL (9, 6) NOT NULL,
    [BonusL]     DECIMAL (9, 6) NOT NULL,
    CONSTRAINT [PK_CorpB] PRIMARY KEY CLUSTERED ([CorpID] ASC, [GameID] ASC, [MemberType] ASC)
);

