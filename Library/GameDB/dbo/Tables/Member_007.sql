CREATE TABLE [dbo].[Member_007] (
    [MemberID]   INT             NOT NULL,
    [GameID]     INT             NOT NULL,
    [Locked]     TINYINT         NOT NULL,
    [Balance]    DECIMAL (19, 6) NOT NULL,
    [ACNT]       VARCHAR (20)    NOT NULL,
    [mode]       INT             NOT NULL,
    [gametype]   NVARCHAR (8)    NOT NULL,
    [language]   NVARCHAR (8)    NOT NULL,
    [username]   NVARCHAR (48)   NOT NULL,
    [currencyid] VARCHAR (5)     NOT NULL,
    [stake]      NVARCHAR (48)   NOT NULL,
    [CreateTime] DATETIME        NOT NULL,
    [CreateUser] INT             NOT NULL,
    [ModifyTime] DATETIME        NOT NULL,
    [ModifyUser] INT             NOT NULL
);

