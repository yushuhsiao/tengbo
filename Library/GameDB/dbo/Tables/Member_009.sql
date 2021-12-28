CREATE TABLE [dbo].[Member_009] (
    [MemberID]       INT             NOT NULL,
    [GameID]         INT             NOT NULL,
    [Locked]         TINYINT         NOT NULL,
    [Balance]        DECIMAL (19, 6) NOT NULL,
    [ACNT]           VARCHAR (20)    NOT NULL,
    [pwd]            VARCHAR (50)    NULL,
    [uppername]      VARCHAR (20)    NOT NULL,
    [TotalBalance]   DECIMAL (19, 6) NOT NULL,
    [Currency]       VARCHAR (3)     NOT NULL,
    [GetBalanceTime] DATETIME        NULL,
    [CreateTime]     DATETIME        CONSTRAINT [DF_Member_009_CreateTime] DEFAULT (getdate()) NOT NULL,
    [CreateUser]     INT             NOT NULL,
    [ModifyTime]     DATETIME        CONSTRAINT [DF_Member_009_ModifyTime] DEFAULT (getdate()) NOT NULL,
    [ModifyUser]     INT             NOT NULL,
    CONSTRAINT [PK_Member_009] PRIMARY KEY CLUSTERED ([MemberID] ASC, [GameID] ASC)
);

