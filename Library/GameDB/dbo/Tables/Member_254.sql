CREATE TABLE [dbo].[Member_254] (
    [MemberID]   INT             NOT NULL,
    [GameID]     INT             NOT NULL,
    [Locked]     TINYINT         NOT NULL,
    [Balance]    DECIMAL (19, 6) CONSTRAINT [DF_Member_254_Balance] DEFAULT ((0)) NOT NULL,
    [ACNT]       VARCHAR (20)    NOT NULL,
    [pwd]        VARCHAR (20)    NULL,
    [Currency]   VARCHAR (3)     NOT NULL,
    [CreateTime] DATETIME        CONSTRAINT [DF_Member_254_CreateTime] DEFAULT (getdate()) NOT NULL,
    [CreateUser] INT             NOT NULL,
    [ModifyTime] DATETIME        CONSTRAINT [DF_Member_254_ModifyTime] DEFAULT (getdate()) NOT NULL,
    [ModifyUser] INT             NOT NULL,
    CONSTRAINT [PK_Member_254] PRIMARY KEY CLUSTERED ([MemberID] ASC, [GameID] ASC)
);

