CREATE TABLE [dbo].[MemberG] (
    [MemberID]   INT             NOT NULL,
    [GameID]     INT             NOT NULL,
    [ACNT]       VARCHAR (20)    NOT NULL,
    [Locked]     TINYINT         NOT NULL,
    [Currency]   VARCHAR (3)     NOT NULL,
    [Balance]    DECIMAL (19, 6) CONSTRAINT [DF_MemberG_Amount] DEFAULT ((0)) NOT NULL,
    [BonusW]     DECIMAL (9, 6)  NULL,
    [BonusL]     DECIMAL (9, 6)  NULL,
    [CreateTime] DATETIME        CONSTRAINT [DF_MemberG_CreateTime] DEFAULT (getdate()) NOT NULL,
    [CreateUser] INT             NOT NULL,
    [ModifyTime] DATETIME        CONSTRAINT [DF_MemberG_ModifyTime] DEFAULT (getdate()) NOT NULL,
    [ModifyUser] INT             NOT NULL,
    CONSTRAINT [PK_MemberG] PRIMARY KEY CLUSTERED ([MemberID] ASC, [GameID] ASC)
);

