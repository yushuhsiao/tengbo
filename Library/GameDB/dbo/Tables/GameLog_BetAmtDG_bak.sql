CREATE TABLE [dbo].[GameLog_BetAmtDG_bak] (
    [sn]           BIGINT          NOT NULL,
    [ACTime]       SMALLDATETIME   NOT NULL,
    [GameID]       INT             NOT NULL,
    [GameType]     NVARCHAR (100)  NOT NULL,
    [CorpID]       INT             NOT NULL,
    [MemberID]     INT             NOT NULL,
    [ACNT]         VARCHAR (20)    NOT NULL,
    [AgentID]      INT             NOT NULL,
    [AgentACNT]    VARCHAR (20)    NOT NULL,
    [BetAmount]    DECIMAL (19, 6) NOT NULL,
    [BetAmountAct] DECIMAL (19, 6) NOT NULL,
    [Payout]       DECIMAL (19, 6) NOT NULL,
    [CreateTime]   DATETIME        NOT NULL,
    [CreateUser]   INT             NOT NULL,
    CONSTRAINT [PK_GameLog_BetAmtDG_bak] PRIMARY KEY CLUSTERED ([sn] ASC),
    CONSTRAINT [IX_GameLog_BetAmtDG_bak] UNIQUE NONCLUSTERED ([ACTime] ASC, [GameID] ASC, [MemberID] ASC, [GameType] ASC)
);

