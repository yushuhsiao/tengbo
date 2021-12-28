CREATE TABLE [dbo].[Member_011] (
    [MemberID]       INT             NOT NULL,
    [GameID]         INT             NOT NULL,
    [Locked]         TINYINT         NOT NULL,
    [Balance]        DECIMAL (19, 6) CONSTRAINT [DF_Member_011_Balance] DEFAULT ((0)) NOT NULL,
    [ACNT]           VARCHAR (20)    NOT NULL,
    [pwd]            VARCHAR (20)    NULL,
    [Currency]       VARCHAR (3)     NOT NULL,
    [actype]         TINYINT         NULL,
    [oddtype]        TINYINT         NULL,
    [GetBalanceTime] DATETIME        NULL,
    [CreateTime]     DATETIME        CONSTRAINT [DF_Member_011_CreateTime] DEFAULT (getdate()) NOT NULL,
    [CreateUser]     INT             NOT NULL,
    [ModifyTime]     DATETIME        CONSTRAINT [DF_Member_011_ModifyTime] DEFAULT (getdate()) NOT NULL,
    [ModifyUser]     INT             NOT NULL,
    CONSTRAINT [PK_Member_011] PRIMARY KEY CLUSTERED ([MemberID] ASC, [GameID] ASC)
);

