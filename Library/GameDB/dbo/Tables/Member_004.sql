CREATE TABLE [dbo].[Member_004] (
    [MemberID]   INT             NOT NULL,
    [GameID]     INT             NOT NULL,
    [Locked]     TINYINT         NOT NULL,
    [Balance]    DECIMAL (19, 6) CONSTRAINT [DF_Member_004_Balance] DEFAULT ((0)) NOT NULL,
    [ACNT]       VARCHAR (20)    NOT NULL,
    [Currency]   VARCHAR (3)     NOT NULL,
    [CreateTime] DATETIME        CONSTRAINT [DF_Member_004_CreateTime] DEFAULT (getdate()) NOT NULL,
    [CreateUser] INT             NOT NULL,
    [ModifyTime] DATETIME        CONSTRAINT [DF_Member_004_ModifyTime] DEFAULT (getdate()) NOT NULL,
    [ModifyUser] INT             NOT NULL,
    CONSTRAINT [PK_Member_004] PRIMARY KEY CLUSTERED ([MemberID] ASC, [GameID] ASC)
);

