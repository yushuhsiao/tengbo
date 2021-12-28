CREATE TABLE [dbo].[GameLog_BetAmtDG] (
    [sn]           BIGINT          IDENTITY (1, 1) NOT NULL,
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
    [CreateTime]   DATETIME        CONSTRAINT [DF_GameLog_BetAmtD_CreateTime] DEFAULT (getdate()) NOT NULL,
    [CreateUser]   INT             NOT NULL,
    CONSTRAINT [PK_GameLog_BetAmtDG] PRIMARY KEY CLUSTERED ([sn] ASC),
    CONSTRAINT [IX_GameLog_BetAmtDG] UNIQUE NONCLUSTERED ([ACTime] ASC, [GameID] ASC, [GameType] ASC, [MemberID] ASC)
);

