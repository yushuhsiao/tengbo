CREATE TABLE [dbo].[BankCard] (
    [ID]         INT           IDENTITY (1, 1) NOT NULL,
    [CorpID]     INT           NOT NULL,
    [GroupID]    TINYINT       NOT NULL,
    [LogType]    INT           NOT NULL,
    [CardID]     VARCHAR (30)  NOT NULL,
    [BankName]   NVARCHAR (20) NOT NULL,
    [AccName]    NVARCHAR (20) NOT NULL,
    [Loc1]       NVARCHAR (20) NULL,
    [Loc2]       NVARCHAR (20) NULL,
    [Loc3]       NVARCHAR (20) NULL,
    [Locked]     TINYINT       NOT NULL,
    [pwd]        VARCHAR (50)  NULL,
    [ExpireTime] DATETIME      NULL,
    [CreateTime] DATETIME      CONSTRAINT [DF_BankCard_CreateTime] DEFAULT (getdate()) NOT NULL,
    [CreateUser] INT           NOT NULL,
    [ModifyTime] DATETIME      CONSTRAINT [DF_BankCard_ModifyTime] DEFAULT (getdate()) NOT NULL,
    [ModifyUser] INT           NOT NULL,
    CONSTRAINT [PK_BankCard] PRIMARY KEY CLUSTERED ([ID] ASC)
);

