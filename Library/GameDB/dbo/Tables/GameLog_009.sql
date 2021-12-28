﻿CREATE TABLE [dbo].[GameLog_009] (
    [ID]             BIGINT          IDENTITY (1, 1) NOT NULL,
    [ACTime]         SMALLDATETIME   NOT NULL,
    [CorpID]         INT             NOT NULL,
    [MemberID]       INT             NOT NULL,
    [ACNT]           VARCHAR (20)    NOT NULL,
    [AgentID]        INT             NOT NULL,
    [AgentACNT]      VARCHAR (20)    NOT NULL,
    [gamekind]       INT             NOT NULL,
    [UserName]       VARCHAR (50)    NOT NULL,
    [WagersID]       BIGINT          NOT NULL,
    [WagersDate]     DATETIME        NOT NULL,
    [SerialID]       VARCHAR (20)    NULL,
    [RoundNo]        VARCHAR (10)    NULL,
    [GameType]       VARCHAR (10)    NULL,
    [GameTypei]      INT             NULL,
    [GameCode]       VARCHAR (10)    NULL,
    [Result]         VARCHAR (100)   NULL,
    [ResultType]     VARCHAR (10)    NULL,
    [Card]           VARCHAR (100)   NULL,
    [BetAmount]      DECIMAL (19, 6) CONSTRAINT [DF_GameLog_009_BetAmount] DEFAULT ((0)) NOT NULL,
    [Payoff]         DECIMAL (19, 6) CONSTRAINT [DF_GameLog_009_Payoff] DEFAULT ((0)) NOT NULL,
    [Currency]       VARCHAR (3)     NULL,
    [ExchangeRate]   DECIMAL (19, 6) NULL,
    [Commissionable] DECIMAL (19, 6) CONSTRAINT [DF_GameLog_009_Commissionable] DEFAULT ((0)) NOT NULL,
    [Commission]     DECIMAL (19, 6) NULL,
    [IsPaid]         VARCHAR (5)     NULL,
    [f_sum]          DECIMAL (19, 6) CONSTRAINT [DF_GameLog_009_f_fum] DEFAULT ((1)) NOT NULL,
    CONSTRAINT [PK_GameLog_009] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [IX_GameLog_009] UNIQUE NONCLUSTERED ([MemberID] ASC, [WagersID] ASC)
);

