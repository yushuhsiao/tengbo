CREATE TABLE [dbo].[Member_001] (
    [MemberID]       INT             NOT NULL,
    [GameID]         INT             NOT NULL,
    [Locked]         TINYINT         NOT NULL,
    [Balance]        DECIMAL (19, 6) CONSTRAINT [DF_Member_001_Balance] DEFAULT ((0)) NOT NULL,
    [ACNT]           VARCHAR (50)    NOT NULL,
    [pwd]            VARCHAR (50)    NULL,
    [mode]           INT             NULL,
    [firstname]      VARCHAR (50)    NOT NULL,
    [lastname]       VARCHAR (50)    NOT NULL,
    [currencyid]     VARCHAR (5)     NOT NULL,
    [agentid]        INT             NULL,
    [affiliateid]    VARCHAR (16)    NULL,
    [testusr]        INT             NULL,
    [playerlevel]    INT             NULL,
    [GetBalanceTime] DATETIME        NULL,
    [CreateTime]     DATETIME        CONSTRAINT [DF_Member_001_CreateTime] DEFAULT (getdate()) NOT NULL,
    [CreateUser]     INT             NOT NULL,
    [ModifyTime]     DATETIME        CONSTRAINT [DF_Member_001_ModifyTime] DEFAULT (getdate()) NOT NULL,
    [ModifyUser]     INT             NOT NULL,
    CONSTRAINT [PK_Member_001] PRIMARY KEY CLUSTERED ([MemberID] ASC, [GameID] ASC)
);

