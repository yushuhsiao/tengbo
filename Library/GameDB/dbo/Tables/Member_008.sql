CREATE TABLE [dbo].[Member_008] (
    [MemberID]   INT             NOT NULL,
    [GameID]     INT             NOT NULL,
    [gametype]   INT             NULL,
    [Locked]     TINYINT         NOT NULL,
    [Balance]    DECIMAL (19, 6) NOT NULL,
    [ACNT]       VARCHAR (20)    NOT NULL,
    [language]   NVARCHAR (8)    NULL,
    [agentid]    NVARCHAR (32)   NULL,
    [Currency]   VARCHAR (3)     NOT NULL,
    [CreateTime] DATETIME        NOT NULL,
    [CreateUser] INT             NOT NULL,
    [ModifyTime] DATETIME        NOT NULL,
    [ModifyUser] INT             NOT NULL
);

