CREATE TABLE [dbo].[cashBankCard] (
    [ID]         UNIQUEIDENTIFIER NOT NULL,
    [CorpID]     INT              NOT NULL,
    [LogType]    INT              NOT NULL,
    [Locked]     TINYINT          NOT NULL,
    [CreateTime] DATETIME         CONSTRAINT [DF_cashBankCard_CreateTime] DEFAULT (getdate()) NOT NULL,
    [CreateUser] INT              NOT NULL,
    [ModifyTime] DATETIME         CONSTRAINT [DF_cashBankCard_ModifyTime] DEFAULT (getdate()) NOT NULL,
    [ModifyUser] INT              NOT NULL,
    [CardID]     VARCHAR (30)     NOT NULL,
    [BankName]   NVARCHAR (20)    NOT NULL,
    [AccName]    NVARCHAR (20)    NOT NULL,
    [Loc1]       NVARCHAR (20)    NULL,
    [Loc2]       NVARCHAR (20)    NULL,
    [Loc3]       NVARCHAR (20)    NULL,
    CONSTRAINT [PK_cashBankCard] PRIMARY KEY CLUSTERED ([ID] ASC)
);

